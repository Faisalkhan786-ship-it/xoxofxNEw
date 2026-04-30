using Common;
using Dapper;
using EmailSystem;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Data.SqlClient;
using MimeKit;
using RepositoryContract;
using System;
using System.Data;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using ViewModel;
using static Model.ModelType;


namespace Repository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly DapperContext _dapperContext;
        private readonly EmailService _emailService;
        //private readonly EmailService _emailService;
        public AuthenticationRepository(DapperContext dapperContext, EmailService emailService)
        {
            _dapperContext = dapperContext;
            _emailService = emailService;
            //_emailService = emailService; 
        }
        public async Task<ResponseViewModel> appLogin(AppLoginViewModel appLogin)
        {
            var procedureName = Constant.userLogin;
            var parameters = new DynamicParameters();
            parameters.Add("@Email", appLogin.username, DbType.String);
            parameters.Add("@Password", appLogin.password, DbType.String);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
                ResponseViewModel returnData;
                if (result != null && result.Any())
                {
                    var validation = result.First();
                    if (validation.statusCode == 1)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = validation.message,
                            data = result
                        };
                    }
                    else if (validation.statusCode == 0)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.Conflict,
                            message = validation.message
                        };
                    }
                    else if (validation.statusCode == -1)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.Conflict,
                            message = validation.message
                        };
                    }
                    else
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.BadRequest,
                            message = validation.message
                        };
                    }
                }
                else
                {
                    returnData = new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "Something went to wrong with server error."
                    };
                }
                return returnData;
            }
        }
        public async Task<ResponseViewModel> adminUserLogin(AppUserAdminLoginViewModel appUserAdminLoginViewModel)
        {
            var procedureName = Constant.adminUserLogin;
            var parameters = new DynamicParameters();
            parameters.Add("@LoginID", appUserAdminLoginViewModel.username, DbType.String);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
                ResponseViewModel returnData;
                if (result != null && result.Any())
                {
                    var validation = result.First();
                    if (validation.statusCode == 1)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = validation.message,
                            data = result
                        };
                    }
                    else if (validation.statusCode == 0)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.Conflict,
                            message = validation.message
                        };
                    }
                    else if (validation.statusCode == -1)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.Conflict,
                            message = validation.message
                        };
                    }
                    else
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.BadRequest,
                            message = validation.message
                        };
                    }
                }
                else
                {
                    returnData = new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "Something went to wrong with server error."
                    };
                }
                return returnData;
            }
        }
                      
        public async Task<ResponseViewModellogin> addAppUser(AddAppUserViewModel addAppUser)
        {
            var procedureName = Constant.addUsersAccount;
            var welcomeProc = Constant.welcomeDetails;

            var parameters = new DynamicParameters();

            // ------------------------------
            //  MOBILE VALIDATION + CLEANUP
            // ------------------------------
            addAppUser.PhoneNo = addAppUser.PhoneNo?
                .Trim()
                .Replace(" ", "")
                .Replace("+91", "");

            if (string.IsNullOrEmpty(addAppUser.PhoneNo) ||
                !Regex.IsMatch(addAppUser.PhoneNo, @"^[0-9]{7,13}$"))
            {
                return new ResponseViewModellogin
                {
                    statusCode = (int)HttpStatusCode.BadRequest,
                    message = "Mobile number must be numeric and between 7 to 13 digits."
                };
            }

            // ------------------------------
            //       EMAIL VALIDATION
            // ------------------------------
            if (string.IsNullOrWhiteSpace(addAppUser.Email) ||
                !Regex.IsMatch(addAppUser.Email.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase))
            {
                return new ResponseViewModellogin
                {
                    statusCode = (int)HttpStatusCode.BadRequest,
                    message = "Invalid Email Id format."
                };
            }

            // ------------------------------
            //      STRONG PASSWORD CHECK
            // ------------------------------
            var strongPasswordRegex =
                 new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#]).{8,}$");

            if (!strongPasswordRegex.IsMatch(addAppUser.PasswordHash))
            {
                return new ResponseViewModellogin
                {
                    statusCode = (int)HttpStatusCode.BadRequest,
                    message = "Password must include at least one uppercase, one lowercase, one number, and one special character."
                };
            }

            // ------------------------------
            //    PARAMETERS FOR PROCEDURE
            // ------------------------------
            parameters.Add("@FullName", addAppUser.FullName);
            parameters.Add("@Email", addAppUser.Email);
            parameters.Add("@PasswordHash", addAppUser.PasswordHash);
            parameters.Add("@PhoneNo", addAppUser.PhoneNo);
            parameters.Add("@countryid", addAppUser.countryId);
            parameters.Add("@intResult", dbType: DbType.Int64, direction: ParameterDirection.Output);

            using var connection = _dapperContext.createConnection();

            // ------------------------------
            //    CALL MAIN INSERT PROC
            // ------------------------------
            var insertedUser = await connection.QueryFirstOrDefaultAsync<dynamic>(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
            );

            long intResult = parameters.Get<long>("@intResult");

            if (intResult == 1 && insertedUser != null)
            {
                string email = insertedUser.Email;
                string plainPassword = "";

                // ------------------------------
                //     CALL WELCOME PROC
                // ------------------------------
                var welcomeParams = new DynamicParameters();
                welcomeParams.Add("@Email", email);

                var welcomeResult = await connection.QueryFirstOrDefaultAsync<dynamic>(
                    welcomeProc,
                    welcomeParams,
                    commandType: CommandType.StoredProcedure
                );

                if (welcomeResult != null && welcomeResult.statusCode == 1)
                {
                    plainPassword = welcomeResult.AuthPass;

                    _emailService.SendOtpEmailForUserRegistrationWelcomletter(
                        plainPassword,
                        email,
                        addAppUser.FullName
                    );
                }

                return new ResponseViewModellogin
                {
                    statusCode = (int)HttpStatusCode.OK,
                    message = "User Registered Successfully and Login Credentials Sent to Email.",
                    Email = email,
                    AuthPassword = plainPassword,
                    Name = addAppUser.FullName
                };
            }

            // ------------------------------
            //    FAILURE RESPONSE
            // ------------------------------
            return new ResponseViewModellogin
            {
                statusCode = intResult == -1 ? (int)HttpStatusCode.Conflict : (int)HttpStatusCode.BadRequest,
                message = intResult == -1 ? "Email or Mobile already exists" : "Something went wrong"
            };
        }

        public async Task<ResponseViewModel> getByReferralId(string loginId)
        {
            var procedureName = Constant.spGetByReferralId;
            var parameters = new DynamicParameters();
            parameters.Add("@LoginID", loginId, DbType.String);

            using (var connection = _dapperContext.createConnection())
            {
                using (var multi = await connection.QueryMultipleAsync(procedureName, parameters, commandType: CommandType.StoredProcedure))
                {
                    var data = (await multi.ReadAsync<ReferralDetails>()).FirstOrDefault();
                    var resultStatus = await multi.ReadFirstOrDefaultAsync<dynamic>();

                    int statusCode = resultStatus?.statusCode ?? -1;
                    string message = resultStatus?.message ?? "Unknown error";

                    int httpStatusCode = statusCode == 1 ? 200 :
                                         statusCode == 0 || statusCode == -1 ? 401 :
                                         500;

                    return new ResponseViewModel
                    {
                        statusCode = httpStatusCode,
                        message = message,
                        data = statusCode == 1 ? data : null
                    };
                }
            }
        }

        public async Task<ResponseViewModel> changePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            var procedureName = Constant.spUpdatePassword;
            var parameters = new DynamicParameters();
            parameters.Add("@LoginID", changePasswordViewModel.UserId, DbType.String);
            parameters.Add("@OldPassword", changePasswordViewModel.OldPassword, DbType.String);
            parameters.Add("@newPassword", changePasswordViewModel.NewPass, DbType.String);

            ResponseViewModel returnData = new ResponseViewModel();

            try
            {
                using (var connection = _dapperContext.createConnection())
                {
                    var result = await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);

                    if (result != null && result.Any())
                    {
                        var validation = result.First();
                        int statusCode = (int)validation.statusCode;
                        string message = (string)validation.message;

                        returnData = new ResponseViewModel
                        {
                            statusCode = statusCode == 1 ? (int)HttpStatusCode.OK :
                                         statusCode == 0 ? (int)HttpStatusCode.Conflict :
                                         (int)HttpStatusCode.BadRequest,
                            message = message
                        };
                    }
                    else
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.NotFound,
                            message = "Something went wrong with server error."
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                returnData = new ResponseViewModel
                {
                    statusCode = (int)HttpStatusCode.InternalServerError,
                    message = $"Exception occurred: {ex.Message}"
                };
            }

            return returnData;
        }

        public async Task<ResponseViewModel> GetUserKycByLoginId(string loginId)
        {
            var procedureName = Constant.spGetUserKyc;
            var parameters = new DynamicParameters();
            parameters.Add("@loginId", loginId, DbType.String);

            var response = new ResponseViewModel();

            try
            {
                using (var connection = _dapperContext.createConnection())
                {
                    var result = await connection.QueryAsync<UserKyc>(procedureName, parameters, commandType: CommandType.StoredProcedure);

                    response.statusCode = result.Any() ? (int)HttpStatusCode.OK : (int)HttpStatusCode.NotFound;
                    response.message = result.Any() ? "Data Found" : "Data Not Found";
                    response.data = result;
                }
            }
            catch (Exception ex)
            {
                response.statusCode = (int)HttpStatusCode.InternalServerError;
                response.message = $"Exception occurred: {ex.Message}";
                response.data = null;
            }

            return response;
        }


        public async Task<ResponseViewModel> GetAllUserRegitration()
        {
            var procedureName = Constant.spGetAllUserRegitration;
            var parameters = new DynamicParameters();
            ResponseViewModel returnData;

            try
            {
                using (var connection = _dapperContext.createConnection())
                {
                    var result = await connection.QueryAsync<UserRegistrationModel>(procedureName, parameters, commandType: CommandType.StoredProcedure);

                    if (result != null && result.Any())
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = "Get All User Registration Details.",
                            data = result.ToList()
                        };
                    }
                    else
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.NotFound,
                            message = "No records found."
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                returnData = new ResponseViewModel
                {
                    statusCode = (int)HttpStatusCode.InternalServerError,
                    message = "An error occurred: " + ex.Message
                };
            }

            return returnData;
        }

        public async Task<ResponseViewModel> updateUserProfile(UpdateUserProfileViewModel updateUserProfileViewModel)
        {
            var procedureName = Constant.updateUserProfile;
            var parameters = new DynamicParameters();
            parameters.Add("@LoginID", updateUserProfileViewModel.LoginID, DbType.String);
            parameters.Add("@FName", updateUserProfileViewModel.FName, DbType.String);
            parameters.Add("@LName", updateUserProfileViewModel.LName, DbType.String);
            parameters.Add("@Address", updateUserProfileViewModel.Address, DbType.String);
            parameters.Add("@WalletBep20", updateUserProfileViewModel.WalletBep20, DbType.String);
            parameters.Add("@Email", updateUserProfileViewModel.Email, DbType.String);
            parameters.Add("@mobile", updateUserProfileViewModel.mobile, DbType.String);
            parameters.Add("@countryid", updateUserProfileViewModel.countryid, DbType.Int32);

            ResponseViewModel returnData = new ResponseViewModel();

            try
            {
                using (var connection = _dapperContext.createConnection())
                {
                    var result = await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);

                    if (result != null && result.Any())
                    {
                        var validation = result.First();
                        int statusCode = (int)validation.statusCode;
                        string message = (string)validation.message;

                        returnData = new ResponseViewModel
                        {
                            statusCode = statusCode == 1 ? (int)HttpStatusCode.OK :
                                         statusCode == -1 ? (int)HttpStatusCode.NotFound :
                                         (int)HttpStatusCode.BadRequest,
                            message = message
                        };
                    }
                    else
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.NotFound,
                            message = "No response from stored procedure."
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                returnData = new ResponseViewModel
                {
                    statusCode = (int)HttpStatusCode.InternalServerError,
                    message = $"Exception occurred: {ex.Message}"
                };
            }

            return returnData;
        }


        
        public async Task<ResponseViewModel> updateUserProfileImage(UpdateUserImageViewModel updateUserImageViewModel)
        {
            var uploadedImageUrls = new List<string>();
            var finalResult = new ResponseViewModel();

            try
            {
                if (updateUserImageViewModel.ProfileImage != null && updateUserImageViewModel.ProfileImage.Count > 0)
                {
                    using var client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ExtractToken.ApiToken);

                    using var connection = _dapperContext.createConnection();

                    foreach (var file in updateUserImageViewModel.ProfileImage)
                    {
                        // 🔹 Upload to Cloudflare
                        using var content = new MultipartFormDataContent();
                        using var stream = file.OpenReadStream();
                        content.Add(new StreamContent(stream), "file", file.FileName);

                        var response = await client.PostAsync(
                            $"https://api.cloudflare.com/client/v4/accounts/{ExtractToken.AccountId}/images/v1", content
                        );

                        var jsonResponse = await response.Content.ReadAsStringAsync();

                        if (!response.IsSuccessStatusCode)
                        {
                            return new ResponseViewModel
                            {
                                statusCode = 0,
                                message = $"Image upload failed for {file.FileName}",
                                data = jsonResponse
                            };
                        }

                        var json = System.Text.Json.JsonDocument.Parse(jsonResponse);
                        var imagePath = json.RootElement.GetProperty("result").GetProperty("variants")[0].GetString();
                        uploadedImageUrls.Add(imagePath);

                        // 🔹 Insert into DB
                        var procedureName = Constant.updateUserProfileImage;
                        var parameters = new DynamicParameters();
                        parameters.Add("@LoginID", updateUserImageViewModel.LoginID, DbType.String);
                        parameters.Add("@ProfileImage", imagePath ?? "", DbType.String);

                        var result = await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);

                        if (result == null || !result.Any())
                        {
                            return new ResponseViewModel
                            {
                                statusCode = (int)HttpStatusCode.NotFound,
                                message = "No response from stored procedure."
                            };
                        }

                        var validation = result.First();
                        int statusCode = (int)validation.statusCode;
                        string message = (string)validation.message;

                        if (statusCode != 1)
                        {
                            return new ResponseViewModel
                            {
                                statusCode = statusCode,
                                message = $"Database update failed: {message}"
                            };
                        }
                    }

                    // 🔹 All done successfully
                    finalResult.statusCode = (int)HttpStatusCode.OK;
                    finalResult.message = "Profile Updated Successfully.";
                    finalResult.data = uploadedImageUrls;
                }
                else
                {
                    finalResult.statusCode = (int)HttpStatusCode.BadRequest;
                    finalResult.message = "No image file provided.";
                }
            }
            catch (Exception ex)
            {
                finalResult.statusCode = (int)HttpStatusCode.InternalServerError;
                finalResult.message = $"Exception occurred: {ex.Message}";
            }

            return finalResult;
        }




        public async Task<ResponseViewModel> UserDashboardDetails(Guid URID)
        {
            var procedureName = Constant.getUserDashboardDetails;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", URID, DbType.Guid);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
                ResponseViewModel returnData;
                if (result != null && result.Any())
                {
                    var validation = result.First();
                    if (validation.statusCode == 1)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = validation.message,
                            data = result
                        };
                    }
                    else if (validation.statusCode == 0)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.Conflict,
                            message = validation.message
                        };
                    }
                    else if (validation.statusCode == -1)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.Conflict,
                            message = validation.message
                        };
                    }
                    else
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.BadRequest,
                            message = validation.message
                        };
                    }
                }
                else
                {
                    returnData = new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "Something went to wrong with server error."
                    };
                }
                return returnData;
            }
        }
        public async Task<ResponseViewModel> validateOtpbyEmail(ValidateOtpViewModelbyemail validateOtpViewModelbyemail)
        {
            var procedureName = Constant.validateOtpbtEmailId;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", validateOtpViewModelbyemail.Email, DbType.String);
            parameters.Add("@otp", validateOtpViewModelbyemail.otp, DbType.String);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
                ResponseViewModel returnData;
                if (result != null && result.Any())
                {
                    var validation = result.First();
                    if (validation.statusCode == 1)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = validation.message,
                            data = result
                        };
                    }
                    else if (validation.statusCode == 0)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.Conflict,
                            message = validation.message
                        };
                    }
                    else if (validation.statusCode == -1)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.Conflict,
                            message = validation.message
                        };
                    }
                    else
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.BadRequest,
                            message = validation.message
                        };
                    }
                }
                else
                {
                    returnData = new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "Something went to wrong with server error."
                    };
                }
                return returnData;
            }
        }

        public async Task<ResponseViewModel> validateOtp(ValidateOtpViewModel validateOtpViewModel)
        {
            var procedureName = Constant.validateOtp;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", validateOtpViewModel.URID, DbType.Guid);
            parameters.Add("@otp", validateOtpViewModel.otp, DbType.String);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
                ResponseViewModel returnData;
                if (result != null && result.Any())
                {
                    var validation = result.First();
                    if (validation.statusCode == 1)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = validation.message,
                            data = result
                        };
                    }
                    else if (validation.statusCode == 0)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.Conflict,
                            message = validation.message
                        };
                    }
                    else if (validation.statusCode == -1)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.Conflict,
                            message = validation.message
                        };
                    }
                    else
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.BadRequest,
                            message = validation.message
                        };
                    }
                }
                else
                {
                    returnData = new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "Something went to wrong with server error."
                    };
                }
                return returnData;
            }
        }

        public async Task<ResponseViewModel> UserUserRentelligenceDashboard(Guid URID)
        {
            var procedureName = Constant.getUserAffiliateDashboard;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", URID, DbType.Guid);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
                ResponseViewModel returnData;
                if (result != null && result.Any())
                {
                    var validation = result.First();
                    if (validation.statusCode == 1)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = validation.message,
                            data = result
                        };
                    }
                    else if (validation.statusCode == 0)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.Conflict,
                            message = validation.message
                        };
                    }
                    else if (validation.statusCode == -1)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.Conflict,
                            message = validation.message
                        };
                    }
                    else
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.BadRequest,
                            message = validation.message
                        };
                    }
                }
                else
                {
                    returnData = new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "Something went to wrong with server error."
                    };
                }
                return returnData;
            }
        }

        public async Task<ResponseViewModel> getLBRank()
        {
            var procedureName = Constant.getLBRank;
            var parameters = new DynamicParameters();

            using var connection = _dapperContext.createConnection();

            using var multi = await connection.QueryMultipleAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);

            var status = await multi.ReadFirstAsync(); // reads first result set: statusCode and message
            var ranks = (await multi.ReadAsync()).ToList(); // reads second result set: rank data

            var returnData = new ResponseViewModel();

            if (status.statusCode == 1)
            {
                returnData.statusCode = (int)HttpStatusCode.OK;
                returnData.message = status.message;
                returnData.data = ranks;
            }
            else
            {
                returnData.statusCode = (int)HttpStatusCode.Conflict; // or as per your logic
                returnData.message = status.message;
                returnData.data = null;
            }

            return returnData;
        }

        public async Task<ResponseViewModel> getAgentAnalyticsUser(Guid URID)
        {
            var procedureName = Constant.getAgentAnalyticsUser;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", URID, DbType.Guid);

            using var connection = _dapperContext.createConnection();

            // Stored procedure ek hi result set return karta hai
            var result = (await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure)).ToList();

            var returnData = new ResponseViewModel();

            if (result.Any())
            {
                // Pehle row me statusCode aur message hoga
                var firstRow = result.First();

                returnData.statusCode = (int)firstRow.statusCode;
                returnData.message = (string)firstRow.message;
                returnData.data = result;  // baaki data list me assign kar diya
            }
            else
            {
                returnData.statusCode = (int)HttpStatusCode.NoContent;
                returnData.message = "No records found.";
                returnData.data = null;
            }

            return returnData;
        }

        public class EmailActionModel
        {
            public int ActionType { get; set; }
            public string? EmailTo { get; set; }
            //public string? EmailId { get; set; }
        }

        //-------Send OtP User Registration
        public async Task<ResponseViewModel> sendOtp(SendOtpViewModel sendOtp)
        {
            var otp = new Random().Next(100000, 999999).ToString();
            await Task.Delay(10);
            // Save OTP in DB
            //var procedureName = Constant.updateOtp;
            using (var connection = _dapperContext.createConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@EmailId", sendOtp.EmailId, DbType.String);
                parameters.Add("@otp", otp, DbType.Int32);

                await connection.ExecuteAsync("SpUpdateOtp", parameters, commandType: CommandType.StoredProcedure);
            }
            using (var connection = _dapperContext.createConnection())
            {
                //  Get Email Routing info from SP
                var result = await connection.QueryFirstOrDefaultAsync<EmailActionModel>(
                    "Sp_GetEmailByActionType",
                    commandType: CommandType.StoredProcedure
                );

                int actionType = result?.ActionType ?? 1; // default = 1
                //  Single common method
                _emailService.SendOtpEmailForUser(otp, sendOtp.EmailId, actionType);
            }

            return new ResponseViewModel
            {
                statusCode = 200,
                message = "OTP sent to email successfully.",
                data = new { otp = otp }
            };
        }

        //--------Send OtP User Withdrawal
        public async Task<ResponseViewModel> sendOtpWithdrawal(SendOtpWithdrawalViewModel sendOtp)
        {
            var otp = new Random().Next(100000, 999999);
            await Task.Delay(10);
            string userName = "User";
            //  Step 1: User ka naam nikaal lo
            using (var connection = _dapperContext.createConnection())
            {
                var result = connection.QueryFirstOrDefault<dynamic>(
                    Constant.getUserNameByEmailId,    // 👈 isme email se user name milega
                    new { Email = sendOtp.EmailId },
                    commandType: CommandType.StoredProcedure
                );

                if (result != null)
                {
                    userName = result.FullName ?? "User";
                }
            }

            // 🔹 Step 2: Email ActionType decide karo
            int actionType = 1;
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<EmailActionModel>(
                    "Sp_GetEmailByActionType",
                    commandType: CommandType.StoredProcedure
                );
                actionType = result?.ActionType ?? 1;
            }

            // 🔹 Step 3: OTP DB me save karo
            using (var connection = _dapperContext.createConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@EmailId", sendOtp.EmailId, DbType.String);
                parameters.Add("@otp", otp, DbType.Int32);

                await connection.ExecuteAsync("SpUpdateOtp", parameters, commandType: CommandType.StoredProcedure);
            }
            string ukTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "GMT Standard Time")
                     .ToString("dd-MMM-yyyy hh:mm tt");

            // 🔹 Step 4: OTP Email bhejo
            _emailService.SendOtpEmailForRequestFundWithdrawal(otp.ToString(), sendOtp.EmailId, sendOtp.WalletAddress,
                userName, actionType, ukTime);
            // 🔹 Step 5: Response
            return new ResponseViewModel
            {
                statusCode = 200,
                message = "OTP sent to email successfully.",
                data = null
            };
        }

        //--------Send OtP Fund Request

        public async Task<ResponseViewModel> sendOtpRequest(SendOtpFundRequestViewModel sendOtp)
        {
            var otp = new Random().Next(100000, 999999);
            await Task.Delay(10);
            string userName = "User";
            //  Step 1: User ka naam nikaal lo
            using (var connection = _dapperContext.createConnection())
            {
                var result = connection.QueryFirstOrDefault<dynamic>(
                    Constant.getUserNameByEmailId,    
                    new { Email = sendOtp.EmailId },
                    commandType: CommandType.StoredProcedure
                );

                if (result != null)
                {
                    userName = result.FullName ?? "User";
                }
            }

            // 🔹 Step 2: Email ActionType decide karo
            int actionType = 1;
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<EmailActionModel>(
                    "Sp_GetEmailByActionType",
                    commandType: CommandType.StoredProcedure
                );
                actionType = result?.ActionType ?? 1;
            }

            // 🔹 Step 3: OTP DB me save karo
            using (var connection = _dapperContext.createConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@EmailId", sendOtp.EmailId, DbType.String);
                parameters.Add("@otp", otp, DbType.Int32);

                await connection.ExecuteAsync("SpUpdateOtp", parameters, commandType: CommandType.StoredProcedure);
            }

            // 🔹 Step 4: OTP Email bhejo
            _emailService.SendOtpEmailForRequestFund(otp.ToString(), sendOtp.EmailId,
                userName, actionType);
            // 🔹 Step 5: Response
            return new ResponseViewModel
            {
                statusCode = 200,
                message = "OTP sent to email successfully.",
                data = null
            };
        }

        //--------Forgot password

        public async Task<ResponseViewModel> forgotPassword(ForgotPasswordViewModel forgotPassword)
        {
            var procedureName = Constant.userForgotPassword;
            var parameters = new DynamicParameters();
            parameters.Add("@Email", forgotPassword.Email, DbType.String);
            int actionType = 1;
        
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<dynamic>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (result != null)
                {
                    int status = result.statusCode ?? 0;

                    if (status == 1)
                    {
                        string authPass = result.AuthPass ?? string.Empty;
                        string email = result.Email ?? string.Empty;

                        if (!string.IsNullOrEmpty(email))
                        {
                            _emailService.SendOtpEmailForForgotPassword(authPass, email);
                        }

                        return new ResponseViewModel
                        {
                            statusCode = 200,
                            message = result.message ?? "Password reset email sent.",
                            data = new
                            {
                                AuthPass = authPass,
                                Email = email
                            }
                        };
                    }
                    else
                    {
                        return new ResponseViewModel
                        {
                            statusCode = 401,
                            message = result.message ?? "Invalid login ID.",
                            data = null
                        };
                    }
                }
                else
                {
                    return new ResponseViewModel
                    {
                        statusCode = 500,
                        message = "No response from server.",
                        data = null
                    };
                }
            }
        }

        public async Task<ResponseViewModel> VerifyLoginid(verifyloginidViewModel verifyloginid)
        {
            var procedureName = Constant.userVerifyloginid;
            var parameters = new DynamicParameters();
            parameters.Add("@LoginId", verifyloginid.Loginid, DbType.String);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<dynamic>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (result != null)
                {
                    int status = result.Status ?? 0;

                    if (status == 1)
                    {
                        return new ResponseViewModel
                        {
                            statusCode = 200,
                            message = result.Message ?? "LoginId verified successfully.",
                            data = new
                            {
                                Package = result.Package
                            }
                        };
                    }
                    else
                    {
                        return new ResponseViewModel
                        {
                            statusCode = 401,
                            message = result.Message ?? "Invalid LoginId.",
                            data = null
                        };
                    }
                }
                else
                {
                    return new ResponseViewModel
                    {
                        statusCode = 500,
                        message = "No response from server.",
                        data = null
                    };
                }
            }
        }

        //public async Task<ResponseViewModel> sendOtpEvent(SendOtpFundRequestViewModel sendOtp)
        //{
        //    var otp = new Random().Next(100000, 999999).ToString();
        //    await Task.Delay(10);
        //    // Save OTP in DB
        //    var procedureName = Constant.updateOtp;
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@EmailId", sendOtp.EmailId, DbType.String);
        //    parameters.Add("@otp", otp, DbType.Int32);
        //    using (var connection = _dapperContext.createConnection())
        //    {
        //        //  Get Email Routing info from SP
        //        var result = await connection.QueryFirstOrDefaultAsync<EmailActionModel>(
        //            "Sp_GetEmailByActionType",
        //            commandType: CommandType.StoredProcedure
        //        );

        //        int actionType = result?.ActionType ?? 1; // default = 1
        //        //  Single common method
        //        _emailService.SendOtpEmailForEventUser(otp, sendOtp.EmailId, actionType);
        //    }

        //    return new ResponseViewModel
        //    {
        //        statusCode = 200,
        //        message = "OTP sent to email successfully.",
        //        //data = new { otp = otp }
        //    };
        //}
        public async Task<ResponseViewModel> sendOtpEvent(SendOtpFundRequestViewModel sendOtp)
        {
            var otp = new Random().Next(100000, 999999).ToString();
            await Task.Delay(10);

            // Save OTP in DB
            var procedureName = Constant.updateOtp;
            var parameters = new DynamicParameters();
            parameters.Add("@EmailId", sendOtp.EmailId, DbType.String);
            parameters.Add("@otp", otp, DbType.Int32);

            using (var connection = _dapperContext.createConnection())
            {
                // **** IMPORTANT PART (Missing execute) ****
                var updateResult = await connection.QueryFirstOrDefaultAsync<dynamic>(
                    procedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                //  Get Email Routing info from SP
                var result = await connection.QueryFirstOrDefaultAsync<EmailActionModel>(
                    "Sp_GetEmailByActionType",
                    commandType: CommandType.StoredProcedure
                );

                int actionType = result?.ActionType ?? 1;

                // Send OTP Email
                _emailService.SendOtpEmailForEventUser(otp, sendOtp.EmailId, actionType);
            }

            return new ResponseViewModel
            {
                statusCode = 200,
                message = "OTP sent to email successfully."
            };
        }
        public async Task<ResponseViewModel> userDashboard(Guid URID)
        {
            var procedureName = Constant.getUserDashboardDetails;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", URID, DbType.Guid);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
                ResponseViewModel returnData;
                if (result != null && result.Any())
                {
                    var validation = result.First();
                    if (validation.statusCode == 1)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = validation.message,
                            data = result
                        };
                    }
                    else if (validation.statusCode == 0)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.Conflict,
                            message = validation.message
                        };
                    }
                    else if (validation.statusCode == -1)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.Conflict,
                            message = validation.message
                        };
                    }
                    else
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.BadRequest,
                            message = validation.message
                        };
                    }
                }
                else
                {
                    returnData = new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "Something went to wrong with server error."
                    };
                }
                return returnData;
            }
        }


        public async Task<ResponseViewModel> getTransactionLog(Guid URID)
        {
            var procedureName = Constant.getTransactionLog;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", URID, DbType.Guid);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
                ResponseViewModel returnData;
                if (result != null && result.Any())
                {
                    var validation = result.First();
                    if (validation.statusCode == 1)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = validation.message,
                            data = result
                        };
                    }
                    else if (validation.statusCode == 0)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.Conflict,
                            message = validation.message
                        };
                    }
                    else if (validation.statusCode == -1)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.Conflict,
                            message = validation.message
                        };
                    }
                    else
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.BadRequest,
                            message = validation.message
                        };
                    }
                }
                else
                {
                    returnData = new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "Something went to wrong with server error."
                    };
                }
                return returnData;
            }
        }

        public async Task<ResponseViewModel> getABREngine(Guid URID)
        {
            var procedureName = Constant.getABREngine;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", URID, DbType.Guid);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
                ResponseViewModel returnData;
                if (result != null && result.Any())
                {
                    var validation = result.First();
                    if (validation.statusCode == 1)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = validation.message,
                            data = result
                        };
                    }
                    else if (validation.statusCode == 0)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.Conflict,
                            message = validation.message
                        };
                    }
                    else if (validation.statusCode == -1)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.Conflict,
                            message = validation.message
                        };
                    }
                    else
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.BadRequest,
                            message = validation.message
                        };
                    }
                }
                else
                {
                    returnData = new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "Something went to wrong with server error."
                    };
                }
                return returnData;
            }
        }

        public async Task<ResponseViewModel> getUserAnalytics(Guid URID)
        {
            var procedureName = Constant.getUserAnalytics;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", URID, DbType.Guid);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
                ResponseViewModel returnData;
                if (result != null && result.Any())
                {
                    var validation = result.First();
                    if (validation.statusCode == 1)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = validation.message,
                            data = result
                        };
                    }
                    else if (validation.statusCode == 0)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.Conflict,
                            message = validation.message
                        };
                    }
                    else if (validation.statusCode == -1)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.Conflict,
                            message = validation.message
                        };
                    }
                    else
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.BadRequest,
                            message = validation.message
                        };
                    }
                }
                else
                {
                    returnData = new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "Something went to wrong with server error."
                    };
                }
                return returnData;
            }
        }


        public async Task<ResponseViewModel> getUserLinkedIds(Guid URID)
        {
            var procedureName = Constant.getUserLinkedIds;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", URID, DbType.Guid);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
                ResponseViewModel returnData;
                if (result != null && result.Any())
                {
                    var validation = result.First();
                    if (validation.statusCode == 1)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = validation.message,
                            data = result
                        };
                    }
                    else if (validation.statusCode == 0)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.Conflict,
                            message = validation.message
                        };
                    }
                    else if (validation.statusCode == -1)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.Conflict,
                            message = validation.message
                        };
                    }
                    else
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.BadRequest,
                            message = validation.message
                        };
                    }
                }
                else
                {
                    returnData = new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "Something went to wrong with server error."
                    };
                }
                return returnData;
            }
        }
    }
}
