using Common;
using Dapper;
using MailKit.Security;
using MimeKit;
using RepositoryContract;
using System.Data;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using ViewModel;
using static Model.ModelType;

namespace Repository
{
    public class SellerRepository : ISellerRepository
    {
        private readonly DapperContext _dapperContext;
        public SellerRepository(DapperContext dapperContext) =>
            _dapperContext = dapperContext;
        public async Task<ResponseViewModel> getByIdSeller(Guid sellerId)
        {
            var procedureName = Constant.spGetByIdSeller;
            var parameters = new DynamicParameters();
            parameters.Add("@sellerId", sellerId, DbType.Guid);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<Seller>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                var getbyIdSeller = new ResponseViewModel
                {
                    statusCode = result.Count() == 0 ? (int)HttpStatusCode.NotFound : (int)HttpStatusCode.OK,
                    message = result.Count() == 0 ? "Data Not Found" : "Data Found",
                    data = result
                };
                return getbyIdSeller;
            }
        }

        public async Task<ResponseViewModel> getAllSeller()
        {
            var procedureName = Constant.spGetAllSeller;

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<Seller>(procedureName, null, commandType: CommandType.StoredProcedure);
                var getAllSeller = new ResponseViewModel
                {
                    statusCode = result.Count() == 0 ? (int)HttpStatusCode.NotFound : (int)HttpStatusCode.OK,
                    message = result.Count() == 0 ? "Data Not Found" : "Data Found",
                    data = result
                };
                return getAllSeller;
            }
        }
        public async Task<ResponseViewModel> getAllSellerForUser()
        {
            var procedureName = Constant.spGetAllSellerForUser;

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<Seller>(procedureName, null, commandType: CommandType.StoredProcedure);
                var getAllSeller = new ResponseViewModel
                {
                    statusCode = result.Count() == 0 ? (int)HttpStatusCode.NotFound : (int)HttpStatusCode.OK,
                    message = result.Count() == 0 ? "Data Not Found" : "Data Found",
                    data = result
                };
                return getAllSeller;
            }
        }

        public async Task<ResponseViewModel> addSeller(AddSellerViewModel addSeller)
        {
            var procedureName = Constant.spAddSeller;
            var parameters = new DynamicParameters();
            parameters.Add("@name", addSeller.name, DbType.String);
            parameters.Add("@mobile", addSeller.@mobile, DbType.String);
            parameters.Add("@email", addSeller.@email, DbType.String);
            parameters.Add("@streetAddress", addSeller.@streetAddress, DbType.String);
            parameters.Add("@state", addSeller.state, DbType.String);
            parameters.Add("@city", addSeller.@city, DbType.String);
            parameters.Add("@pincode", addSeller.@pincode, DbType.String);
            parameters.Add("@country", addSeller.@country, DbType.String);
            parameters.Add("@description", addSeller.@description, DbType.String);
            parameters.Add("@createdBy", addSeller.createdBy, DbType.Guid);
            parameters.Add("@userName", addSeller.@userName, DbType.String);
            parameters.Add("@userPassword", addSeller.@userPassword, DbType.String);
            parameters.Add("@otp", addSeller.@otp, DbType.String);


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
        public async Task<ResponseViewModel> updateSeller(UpdateSellerViewModel updateSeller)
        {
            var procedureName = Constant.spUpdateSeller;
            var parameters = new DynamicParameters();
            parameters.Add("@sellerId", updateSeller.sellerId, DbType.Guid);
            parameters.Add("@name", updateSeller.name, DbType.String);
            parameters.Add("@mobile", updateSeller.mobile, DbType.String);
            parameters.Add("@email", updateSeller.email, DbType.String);
            parameters.Add("@streetAddress", updateSeller.streetAddress, DbType.String);
            parameters.Add("@state", updateSeller.state, DbType.String);
            parameters.Add("@city", updateSeller.city, DbType.String);
            parameters.Add("@pincode", updateSeller.pincode, DbType.String);
            parameters.Add("@country", updateSeller.country, DbType.String);
            parameters.Add("@description", updateSeller.description, DbType.String);
            parameters.Add("@active", updateSeller.active ? 1 : 0, DbType.Boolean);
            parameters.Add("@updatedBy", updateSeller.updatedBy, DbType.Guid);
            parameters.Add("@userPassword", updateSeller.@userPassword, DbType.String);
            parameters.Add("@otp", updateSeller.@otp, DbType.String);
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
        public async Task<ResponseViewModel> deleteSeller(DeleteSellerViewModel deleteSeller)
        {
            var procedureName = Constant.spDeleteSeller;
            var parameters = new DynamicParameters();
            parameters.Add("@sellerId", deleteSeller.sellerId, DbType.Guid);
            parameters.Add("@updatedBy", deleteSeller.updatedBy, DbType.Guid);
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

        public async Task<ResponseViewModel> forgotPassword(ForgotSellerPasswordViewModel forgotSellerPasswordViewModel)
        {
            var procedureName = Constant.spSellerForgotPassword;
            var parameters = new DynamicParameters();
            parameters.Add("@userName", forgotSellerPasswordViewModel.LoginId, DbType.String);

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
                            (string)result.userName,
                            (string)result.userPassword
                        );

                        return new ResponseViewModel
                        {
                            statusCode = 200,
                            message = result.message,
                            data = new
                            {
                                email = result.email,
                                userName = result.userName,
                                userPassword = result.userPassword
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
            string Name = "Rentelligence Seller User";
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

        public async Task<ResponseViewModel> sellerIsActive(Guid sellerId)
        {
            var procedureName = Constant.spActiveSeller;
            var parameters = new DynamicParameters();
            parameters.Add("@sellerId", sellerId, DbType.Guid);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<dynamic>(
                    procedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                int dbStatus = result?.statusCode ?? -1;
                var response = new ResponseViewModel
                {
                    statusCode = dbStatus == 1 ? 200 : dbStatus,
                    message = result?.message ?? "Unexpected error",
                    data = result
                };

                return response;
            }
        }

        public async Task<ResponseViewModel> SellerLogin(SellerLoginViewModel SellerLoginViewModel)
        {
            var procedureName = Constant.spAppLogin;
            var parameters = new DynamicParameters();
            parameters.Add("@username", SellerLoginViewModel.username, DbType.String);
            parameters.Add("@password", SellerLoginViewModel.password, DbType.String);

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