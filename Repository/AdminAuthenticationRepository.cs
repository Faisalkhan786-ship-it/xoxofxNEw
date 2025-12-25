using Azure;
using Common;
using Dapper;
using MailKit.Security;
using MimeKit;
using RepositoryContract;
using System.Data;
using System.Net;
using System.Text;
using ViewModel;
using static Model.ModelType;

namespace Repository
{
    class AdminAuthenticationRepository : IAdminAuthenticationRepository
    {
        private readonly DapperContext _dapperContext;
        public AdminAuthenticationRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }
        public async Task<ResponseViewModel> adminUserLogin(AdminUserLoginViewModel adminUserLogin)
        {
            var procedureName = Constant.spAdminUserLogin;
            var parameters = new DynamicParameters();
            parameters.Add("@username", adminUserLogin.username, DbType.String);
            parameters.Add("@password", adminUserLogin.password, DbType.String);

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

        public async Task<ResponseViewModel> addAdminUser(AddAdminUserViewModel addAdminUser)
        {
            var procedureName = Constant.addAdminUser;
            var parameters = new DynamicParameters();
            parameters.Add("@username", addAdminUser.username, DbType.String);
            parameters.Add("@fname", addAdminUser.fname, DbType.String);
            parameters.Add("@lname", addAdminUser.lname, DbType.String);
            parameters.Add("@email", addAdminUser.email, DbType.String);
            parameters.Add("@phoneNumber", addAdminUser.phoneNumber, DbType.String);
            parameters.Add("@password", addAdminUser.password, DbType.String);
            parameters.Add("@type", addAdminUser.type, DbType.String);


            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModel>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                if (result.statusCode == 1)
                {
                    result.statusCode = (int)HttpStatusCode.OK;
                    result.message = result.message;
                }
                else if (result.statusCode == 0)
                {
                    result.statusCode = (int)HttpStatusCode.ExpectationFailed;
                    result.message = result.message;
                }
                else
                {
                    result.statusCode = (int)HttpStatusCode.ExpectationFailed;
                    result.message = result.message;
                }
                return result;
            }
        }
   

        public async Task<ResponseViewModel> adminSendOtp(AdminSendOtpViewModel adminSendOtp)
        {
            var procedureName = Constant.spAdminSendOtp;
            var parameters = new DynamicParameters();
            parameters.Add("@username", adminSendOtp.username, DbType.String);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
                var emailDetail = new ResponseViewModel
                {
                    statusCode = result.Count() == 0 ? (int)HttpStatusCode.NotFound : (int)HttpStatusCode.OK,
                    message = result.Count() == 0 ? "Error in sending otp, please enter correct username." : "Otp sent to your email address, please enter otp.",
                    data = result
                };
                return emailDetail;
            }
        }
        public async Task<ResponseViewModel> adminVerifyOtp(AdminVerifyOtpViewModel adminVerifyOtp)
        {
            var procedureName = Constant.spAdminVerifyOtp;
            var parameters = new DynamicParameters();
            parameters.Add("@username", adminVerifyOtp.username, DbType.String);
            parameters.Add("@otp", adminVerifyOtp.otp, DbType.String);
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
                            message = validation.message
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
        public async Task<ResponseViewModel> adminForgotPassword(AdminForgotPasswordViewModel adminForgotPassword)
        {
            var procedureName = Constant.spAdminUpdatePassword;
            var parameters = new DynamicParameters();
            parameters.Add("@userName", adminForgotPassword.username, DbType.String);
            parameters.Add("@password", adminForgotPassword.password, DbType.String);
            //parameters.Add("@password", EncryptDecrypt.EnryptString(updatePassword?.password ?? string.Empty), DbType.String);
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
                            message = validation.message
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

        public async Task<ResponseViewModel> getAdminUserDetails(AdminUserGuidViewModel AdminUserGuidViewModel)
        {
            var procedureName = Constant.spGetAdminDetails;
            var parameters = new DynamicParameters();
            parameters.Add("@adminUserId", AdminUserGuidViewModel.adminUserId, DbType.Guid);
            parameters.Add("@username", AdminUserGuidViewModel.username, DbType.String);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<AdminUserDetails>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                var getAllAppRole = new ResponseViewModel
                {
                    statusCode = result.Count() == 0 ? (int)HttpStatusCode.NotFound : (int)HttpStatusCode.OK,
                    message = result.Count() == 0 ? "Data Not Found" : "Get Admin User Details.",
                    data = result
                };
                return getAllAppRole;
            }
        }
        //public class AdminDashboardToday
        //{
        //    public int TotalJoining { get; set; }
        //    public int TotalToadyJoining { get; set; }
        //    public int Totalactive { get; set; }
        //    public decimal totalBusiness { get; set; }
        //    public decimal TodayBusiness { get; set; }
        //    public decimal totalIncomeWallet { get; set; }
        //    public decimal totalDepositWallet { get; set; }
        //    public decimal todayDeposit { get; set; }
        //    public decimal TotalWithdrawal { get; set; }
        //    public decimal TotalROI { get; set; }
        //    public decimal WorkingIncomeToday { get; set; }
        //    public decimal WorkingIncomeTotal { get; set; }

        //}


        public async Task<ResponseViewModel> getAdminDashboardDetails(Guid adminUserId)
        {
            var procedureName = Constant.getAdminDashboardDetails;
            var parameters = new DynamicParameters();
            parameters.Add("@adminUserId", adminUserId, DbType.Guid);
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

        //public async Task<ResponseViewModel> getAdminDashboardDetails(Guid adminUserId)
        //{
        //    var procedureName = Constant.getAdminDashboardDetails;
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@adminUserId", adminUserId); 

        //    try
        //    {
        //        using (var connection = _dapperContext.createConnection())
        //        {
        //            var result = await connection.QueryAsync<AdminDashboardToday>(
        //                procedureName,
        //                parameters,   
        //                commandType: CommandType.StoredProcedure
        //            );

        //            return new ResponseViewModel
        //            {
        //                statusCode = result.Any() ? (int)HttpStatusCode.OK : (int)HttpStatusCode.NotFound,
        //                message = result.Any() ? "Get Admin Dashboard Users Details." : "Data Not Found",
        //                data = result
        //            };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseViewModel
        //        {
        //            statusCode = 500,
        //            message = ex.Message,
        //            data = null
        //        };
        //    }
        //}



        public async Task<ResponseViewModel> getAllAdminList()
        {
            var procedureName = Constant.spGetAllAdminList;
            var parameters = new DynamicParameters();

            try
            {
                using (var connection = _dapperContext.createConnection())
                {
                    var result = await connection.QueryAsync<AdminAllUserDetails>(procedureName, commandType: CommandType.StoredProcedure);

                    var getAllAppRole = new ResponseViewModel
                    {
                        statusCode = result.Count() == 0 ? (int)HttpStatusCode.NotFound : (int)HttpStatusCode.OK,
                        message = result.Count() == 0 ? "Data Not Found" : "Get Admin User Details.",
                        data = result
                    };

                    return getAllAppRole;
                }
            }
            catch (Exception ex)
            {
                // Exception log karlo agar chaho toh
                Console.Error.WriteLine($"Error in getAllAdminList: {ex.Message}");

                return new ResponseViewModel
                {
                    statusCode = (int)HttpStatusCode.InternalServerError,
                    message = "Something went wrong while fetching admin list."
                };
            }
        }
        public async Task<ResponseViewModel> updateAdminStatusActivate(Guid adminuserId)
        {
            var procedureName = Constant.spUpdateAdminStatusActivate;
            var parameters = new DynamicParameters();
            parameters.Add("@userId", adminuserId);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<dynamic>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);

                var response = new ResponseViewModel
                {
                    statusCode = result?.statusCode ?? 500,
                    message = result?.message ?? "Unexpected error occurred.",
                    data = result
                };

                return response;
            }
        }

        

        public async Task<ResponseViewModel> updateAdminStatusDeActivate(Guid adminuserId)
        {
            var procedureName = Constant.spUpdateAdminStatusDeActivate;
            var parameters = new DynamicParameters();
            parameters.Add("@userId", adminuserId);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<dynamic>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);

                var response = new ResponseViewModel
                {
                    statusCode = result?.statusCode ?? 500,
                    message = result?.message ?? "Unexpected error occurred.",
                    data = result
                };

                return response;
            }
        }

        public async Task<ResponseViewModel> addBulkRegsitration(BulkRegsitrationViewModel bulkRegsitrationViewModel)
        {
            var procedureName = Constant.bulkRegistrationAdmin;
            var parameters = new DynamicParameters();
            parameters.Add("@IntroURID", bulkRegsitrationViewModel.IntroURID, DbType.Guid);
            parameters.Add("@IntroSide", bulkRegsitrationViewModel.IntroSide, DbType.String);
            parameters.Add("@FName", bulkRegsitrationViewModel.FName, DbType.String);
            parameters.Add("@LName", bulkRegsitrationViewModel.LName, DbType.String);
            parameters.Add("@Mobile", bulkRegsitrationViewModel.Mobile, DbType.String);
            parameters.Add("@Email", bulkRegsitrationViewModel.Email, DbType.String);
            parameters.Add("@Password", bulkRegsitrationViewModel.Password, DbType.String);
            parameters.Add("@NoOfId", bulkRegsitrationViewModel.NoOfId, DbType.Int32);
            parameters.Add("@CountryId", bulkRegsitrationViewModel.CountryId, DbType.String);


            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModel>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                if (result.statusCode == 1)
                {
                    result.statusCode = (int)HttpStatusCode.OK;
                    result.message = result.message;
                }
                else if (result.statusCode == 0)
                {
                    result.statusCode = (int)HttpStatusCode.ExpectationFailed;
                    result.message = result.message;
                }
                else
                {
                    result.statusCode = (int)HttpStatusCode.ExpectationFailed;
                    result.message = result.message;
                }
                return result;
            }
        }


        public async Task<ResponseViewModel> adminForgotPassword(string username)
        {
            var procedureName = Constant.adminForgotPassword;
            var parameters = new DynamicParameters();
            parameters.Add("@userName", username, DbType.String);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<dynamic>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (result != null)
                {
                    int status = result.statusCode;

                    if (status == 1)
                    {
                        SendOtpEmailForForgotPassword(
                            (string)result.email,
                            username,
                            (string)result.password
                        );

                        return new ResponseViewModel
                        {
                            statusCode = 200,
                            message = result.message,
                            data = new
                            {
                                email = result.email,
                                userName = username,
                                password = result.password
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
        //Send otp Forgot Password
        public void SendOtpEmailForForgotPassword(string emailid, string userName, string userPassword)
        {
            string Name = "Rentelligence Admin";
            string EmailID = emailid.Trim();
            try
            {
                StringBuilder html = new StringBuilder();
                html.Append("Hello " + Name + ",<br/><br/>");
                html.Append("Your LoginId: <strong>" + userName + "</strong><br/><br/>");
                html.Append("Your Password : <strong>" + userPassword + "</strong><br/><br/>");
                html.Append("Thank you for using Rentelligence.");

                bool result = SendEmailCommonForForgotPassword(EmailID, "User Rentelligence Id And Password", html.ToString(), true);
            }
            catch (Exception ex)
            {
                // Log exception
            }
        }

        //Send otp Forgot Password
        public bool SendEmailCommonForForgotPassword(string emailId, string subject, string body, bool isHtml = true)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Rentelligence", "contact@zaddycare.com"));
                message.To.Add(new MailboxAddress(emailId, emailId));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder();
                if (isHtml)
                    bodyBuilder.HtmlBody = body;
                else
                    bodyBuilder.TextBody = body;

                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Connect("zaddycare.com", 465, SecureSocketOptions.SslOnConnect);
                    client.Authenticate("contact@zaddycare.com", "Zaddy@2025#");
                    client.Send(message);
                    client.Disconnect(true);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("MailKit Error: " + ex.Message);
                return false;
            }
        }
        public async Task<ResponseViewModel> updateAdminProfile(UpdateAdminProfileViewModel updateAdminProfileViewModel)
        {
            var procedureName = Constant.updateAdminProfile;
            var parameters = new DynamicParameters();
            parameters.Add("@username", updateAdminProfileViewModel.username, DbType.String);
            parameters.Add("@firstName", updateAdminProfileViewModel.firstName, DbType.String);
            parameters.Add("@phoneNumber", updateAdminProfileViewModel.phoneNumber, DbType.String);
            parameters.Add("@email", updateAdminProfileViewModel.email, DbType.String);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<dynamic>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);

                var response = new ResponseViewModel
                {
                    statusCode = result?.statusCode ?? 500,
                    message = result?.message ?? "Unexpected error occurred.",
                    data = result
                };

                return response;
            }
        }

    }
}

