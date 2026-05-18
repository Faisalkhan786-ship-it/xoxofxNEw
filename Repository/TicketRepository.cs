using Common;
using Dapper;
using FirebaseAdmin.Messaging;
using RepositoryContract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ViewModel;
using static Model.ModelType;
using static ViewModel.TicketViewModel;

namespace Repository
{
    public class TicketRepository : ITicketRepository
    {
        private readonly DapperContext _dapperContext;
        public TicketRepository(DapperContext dapperContext) =>
            _dapperContext = dapperContext;

        public async Task<ResponseViewModel> getUserNotificationList(Guid URID)
        {
            var incomeProc = Constant.getUserNotificationList;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", URID, DbType.Guid);
            using (var connection = _dapperContext.createConnection())
            {
                try
                {
                    var incomeResult = await connection.QueryAsync(incomeProc, parameters, commandType: CommandType.StoredProcedure);
                    var incomeList = incomeResult.ToList();


                    if ((incomeList != null && incomeList.Any()))
                    {
                        var message = "Data fetched successfully";
                        var combinedData = new
                        {
                            NotificationList = incomeList,
                        };

                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = message,
                            data = combinedData
                        };
                    }
                    else
                    {
                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.NotFound,
                            message = "No transaction types found.",
                            data = null
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.InternalServerError,
                        message = $"Error occurred: {ex.Message}",
                        data = null
                    };
                }
            }
        }
        public async Task<ResponseViewModel> getUserUnseenNotiCount(Guid URID)
        {
            var incomeProc = Constant.getUserUnseenNotiCount;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", URID, DbType.Guid);
            using (var connection = _dapperContext.createConnection())
            {
                try
                {
                    var incomeResult = await connection.QueryAsync(incomeProc, parameters, commandType: CommandType.StoredProcedure);
                    var incomeList = incomeResult.ToList();

                    if ((incomeList != null && incomeList.Any()))
                    {
                        var message = "Data fetched successfully";
                        var combinedData = new
                        {
                            NotificationList = incomeList,
                        };

                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = message,
                            data = combinedData
                        };
                    }
                    else
                    {
                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.NotFound,
                            message = "No transaction types found.",
                            data = null
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.InternalServerError,
                        message = $"Error occurred: {ex.Message}",
                        data = null
                    };
                }
            }
        }
        public async Task<ResponseViewModel> updateUserNotiSeenStatus(Guid URID)
        {
            var incomeProc = Constant.updateUserNotiSeenStatus;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", URID, DbType.Guid);
            using (var connection = _dapperContext.createConnection())
            {
                try
                {
                    var incomeResult = await connection.QueryAsync(incomeProc, parameters, commandType: CommandType.StoredProcedure);
                    var incomeList = incomeResult.ToList();
                    if ((incomeList != null && incomeList.Any()))
                    {
                        var message = "Data fetched successfully";
                        var combinedData = new
                        {
                            NotificationList = incomeList,
                        };

                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = message,
                            data = combinedData
                        };
                    }
                    else
                    {
                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.NotFound,
                            message = "No transaction types found.",
                            data = null
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.InternalServerError,
                        message = $"Error occurred: {ex.Message}",
                        data = null
                    };
                }
            }
        }

        public async Task<ResponseViewModel> getAllUserNotificationList(Guid URID)
        {
            var incomeProc = Constant.getAllUserDashbaordNotifyList;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", URID, DbType.Guid);
            using (var connection = _dapperContext.createConnection())
            {
                try
                {
                    var incomeResult = await connection.QueryAsync(incomeProc, parameters, commandType: CommandType.StoredProcedure);
                    var incomeList = incomeResult.ToList();


                    if ((incomeList != null && incomeList.Any()))
                    {
                        var message = "Data fetched successfully";
                        var combinedData = new
                        {
                            NotificationList = incomeList,
                        };

                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = message,
                            data = combinedData
                        };
                    }
                    else
                    {
                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.NotFound,
                            message = "No transaction types found.",
                            data = null
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.InternalServerError,
                        message = $"Error occurred: {ex.Message}",
                        data = null
                    };
                }
            }
        }

        public async Task<ResponseViewModel> addExpoTokens(AddExpoTokensViewModel addExpoTokensViewModel)
        {
            var incomeProc = "";
            var parameters = new DynamicParameters();
            parameters.Add("@URID", addExpoTokensViewModel.URID, DbType.Guid);
            parameters.Add("@ExpoToken", addExpoTokensViewModel.ExpoToken, DbType.String);

            using (var connection = _dapperContext.createConnection())
            {
                try
                {
                    var incomeResult = await connection.QueryAsync(incomeProc, parameters, commandType: CommandType.StoredProcedure);
                    var incomeList = incomeResult.ToList();


                    if ((incomeList != null && incomeList.Any()))
                    {
                        var message = "Data fetched successfully";
                        var combinedData = new
                        {
                            NotificationList = incomeList,
                        };

                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = message,
                            data = combinedData
                        };
                    }
                    else
                    {
                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.NotFound,
                            message = "No transaction types found.",
                            data = null
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.InternalServerError,
                        message = $"Error occurred: {ex.Message}",
                        data = null
                    };
                }
            }
        }

        public async Task<ResponseViewModel> getExpoNotiByURID(Guid URID)
        {
            var spExpoTokens ="" ;
            var spExpoNoti = "";

            using (var connection = _dapperContext.createConnection())
            {
                try
                {
                    // Step 1: Get all tokens by URID
                    var tokenParams = new DynamicParameters();
                    tokenParams.Add("@URID", URID, DbType.Guid);

                    var tokenResult = await connection.QueryAsync<dynamic>(
                        spExpoTokens, tokenParams, commandType: CommandType.StoredProcedure);

                    var tokenList = tokenResult.ToList();

                    if (tokenList == null || !tokenList.Any())
                    {
                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.NotFound,
                            message = "No tokens found for this URID.",
                            data = null
                        };
                    }

                    // Step 2: For each token, get related notifications
                    var allNotifications = new List<dynamic>();

                    foreach (var token in tokenList)
                    {
                        var notiParams = new DynamicParameters();
                        notiParams.Add("@URID", URID, DbType.Guid);

                        var notiResult = await connection.QueryAsync<dynamic>(
                            spExpoNoti, notiParams, commandType: CommandType.StoredProcedure);

                        allNotifications.AddRange(notiResult);
                    }

                    // Step 3: Combine data
                    var combinedData = new
                    {
                        ExpoTokens = tokenList,
                        data = allNotifications
                    };

                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "Data fetched successfully.",
                        data = combinedData
                    };
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.InternalServerError,
                        message = $"Error occurred: {ex.Message}",
                        data = null
                    };
                }
            }
        }

        //------------------Ticket
        public async Task<ResponseViewModel> addTicket(AddTicket addTicket)
        {
            //string imagePath = null;

            //// Use addCategory.image
            //var file = addTicket.ImagePath;

            //if (file != null && file.Length > 0)
            //{
            //    // Cloudflare Upload using ExtractToken
            //    using var client = new HttpClient();
            //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ExtractToken.ApiToken);

            //    using var content = new MultipartFormDataContent();
            //    using var stream = file.OpenReadStream();
            //    content.Add(new StreamContent(stream), "file", file.FileName);

            //    var response = await client.PostAsync(
            //        $"https://api.cloudflare.com/client/v4/accounts/{ExtractToken.AccountId}/images/v1", content
            //    );

            //    var jsonResponse = await response.Content.ReadAsStringAsync();

            //    if (!response.IsSuccessStatusCode)
            //        return new ResponseViewModel { statusCode = 0, message = "Image upload failed", data = jsonResponse };

            //    // Cloudflare URL 
            //    var json = System.Text.Json.JsonDocument.Parse(jsonResponse);
            //    imagePath = json.RootElement.GetProperty("result").GetProperty("variants")[0].GetString();
            //}

            string imagePath = "NaN";
            string videoPath = "NaN";


            // =========================
            // Upload Image
            // =========================
            if (addTicket.ImagePath != null && addTicket.ImagePath.Length > 0)
            {
                string uniqueFileName = Guid.NewGuid() + Path.GetExtension(addTicket.ImagePath.FileName);

                string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TicketImage");

                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                string filePath = Path.Combine(uploadDir, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await addTicket.ImagePath.CopyToAsync(fileStream);
                }

                imagePath = uniqueFileName;
            }

            var procedureName = Constant.insert_TicketTest;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", addTicket.URID, DbType.Guid);
            parameters.Add("@Subject", addTicket.Subject, DbType.String);
            parameters.Add("@Message", addTicket.Message, DbType.String);
            parameters.Add("@ImagePath", imagePath, DbType.String);
            parameters.Add("@TicketType", addTicket.TicketType, DbType.String);


            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModel>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (result.statusCode == 1)
                {
                    result.statusCode = (int)HttpStatusCode.OK;
                }
                else
                {
                    result.statusCode = (int)HttpStatusCode.ExpectationFailed;
                }

                return result;
            }
        }

        public async Task<ResponseViewModel> getAllTicketBYURID(Guid URID)
        {
            var procedureName = Constant.getAllTicketBYURIDTEst;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", URID, DbType.Guid);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "Get Ticket Details.",
                        data = result
                    };
                }
                else
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "No tickets found for this URID."
                    };
                }
            }
        }

        public async Task<ResponseViewModel> getTicketBYTicketId(Guid TicketId)
        {
            var procedureName = Constant.getAllTicketBYTicketIdTEst;
            var procedureName1 = Constant.getAllTicketReplyByTicketIDTest;
            var parameters = new DynamicParameters();
            parameters.Add("@TicketId", TicketId, DbType.Guid);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
                var result1 = await connection.QueryAsync(procedureName1, parameters, commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    var combinedData = new
                    {
                        ticket = result.ToList(),
                        replies = result1.ToList()
                    };

                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "Get Ticket Details.",
                        data = combinedData
                    };
                }
                else
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "No tickets found for this TicketId."
                    };
                }
            }
        }
        public async Task<ResponseViewModel> addTicketReply(AddTicketReply addTicketReply)
        {
            //string imagePath = null;

            //// Use addCategory.image
            //var file = addTicketReply.ImagePath;

            //if (file != null && file.Length > 0)
            //{
            //    // Cloudflare Upload using ExtractToken
            //    using var client = new HttpClient();
            //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ExtractToken.ApiToken);

            //    using var content = new MultipartFormDataContent();
            //    using var stream = file.OpenReadStream();
            //    content.Add(new StreamContent(stream), "file", file.FileName);

            //    var response = await client.PostAsync(
            //        $"https://api.cloudflare.com/client/v4/accounts/{ExtractToken.AccountId}/images/v1", content
            //    );

            //    var jsonResponse = await response.Content.ReadAsStringAsync();

            //    if (!response.IsSuccessStatusCode)
            //        return new ResponseViewModel { statusCode = 0, message = "Image upload failed", data = jsonResponse };

            //    // Cloudflare URL 
            //    var json = System.Text.Json.JsonDocument.Parse(jsonResponse);
            //    imagePath = json.RootElement.GetProperty("result").GetProperty("variants")[0].GetString();
            //}

            string imagePath = "NaN";
            string videoPath = "NaN";


            // =========================
            // Upload Image
            // =========================
            if (addTicketReply.ImagePath != null && addTicketReply.ImagePath.Length > 0)
            {
                string uniqueFileName = Guid.NewGuid() + Path.GetExtension(addTicketReply.ImagePath.FileName);

                string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TicketImage");

                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                string filePath = Path.Combine(uploadDir, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await addTicketReply.ImagePath.CopyToAsync(fileStream);
                }

                imagePath = uniqueFileName;
            }

            var procedureName = Constant.insert_TicketReplyTest;
            var parameters = new DynamicParameters();
            parameters.Add("@TicketId", addTicketReply.TicketId, DbType.Guid);
            parameters.Add("@CreatedBy", addTicketReply.CreatedBy, DbType.Guid);
            parameters.Add("@Message", addTicketReply.Message, DbType.String);
            parameters.Add("@ImagePath", imagePath, DbType.String);
            parameters.Add("@Status", addTicketReply.Status, DbType.Int32);
            parameters.Add("@Seen", addTicketReply.Seen, DbType.Int32);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModel>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (result.statusCode == 1)
                {
                    result.statusCode = (int)HttpStatusCode.OK;
                }
                else
                {
                    result.statusCode = (int)HttpStatusCode.ExpectationFailed;
                }

                return result;
            }
        }
        public async Task<ResponseViewModel> getAllTicketAdmin()
        {
            var procedureName = Constant.getAllTicket;

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "Get Ticket Details.",
                        data = result
                    };
                }
                else
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "No tickets found for this URID."
                    };
                }
            }
        }

        public async Task<ResponseViewModel> closeTicket(Guid TicketId)
        {
            var procedureName = Constant.closeTicketTest;
            var parameters = new DynamicParameters();
            parameters.Add("@TicketId", TicketId, DbType.Guid);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    var combinedData = new
                    {
                        ticket = result.ToList(),
                    };

                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "Closed Ticket Successfully.",
                        data = combinedData
                    };
                }
                else
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "No tickets found for this TicketId."
                    };
                }
            }
        }

        public async Task<ResponseViewModel> GetAllclosedTicket()
        {
            var procedureName = Constant.getAllClosedlistTicket;

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "Get Ticket Details.",
                        data = result
                    };
                }
                else
                {
                    // Extra query nahi → sirf NotFound (404) return
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "No tickets found for this URID."
                    };
                }
            }
        }

        public async Task<ResponseViewModel> sendNotification(SendNotificationViewModel model)
        {
            try
            {
                string procedureName = "SpGetAllExpoTokens";
                var parameters = new DynamicParameters();

                using (var connection = _dapperContext.createConnection())
                {
                    //  Fetch all Expo tokens from DB
                    var tokens = await connection.QueryAsync<dynamic>(
                        procedureName,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (tokens == null || !tokens.Any())
                    {
                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.NotFound,
                            message = "No Expo tokens found ❌",
                            data = null
                        };
                    }

                    var results = new List<object>();
                    int successCount = 0, failedCount = 0;

                    foreach (var t in tokens)
                    {
                        string token = t.To; 

                        if (string.IsNullOrWhiteSpace(token))
                            continue;

                        try
                        {
                            var message = new Message
                            {
                                Token = token,
                                Notification = new Notification
                                {
                                    Title = model.Title,
                                    Body = model.Body,
                                    ImageUrl = model.ImageUrl
                                }
                            };

                            var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);

                            successCount++;
                            results.Add(new
                            {
                                Token = token,
                                Status = "Success",
                                MessageId = response
                            });
                        }
                        catch (FirebaseMessagingException ex)
                        {
                            failedCount++;
                            results.Add(new
                            {
                                Token = token,
                                Status = "Failed",
                                Error = ex.Message
                            });
                        }
                        catch (Exception ex)
                        {
                            failedCount++;
                            results.Add(new
                            {
                                Token = token,
                                Status = "Failed",
                                Error = ex.Message
                            });
                        }
                    }

                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "Notifications sent successfully ✅",
                        data = new
                        {
                            SuccessCount = successCount,
                            FailedCount = failedCount,
                            Results = results
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseViewModel
                {
                    statusCode = (int)HttpStatusCode.InternalServerError,
                    message = "Error sending notifications ❌",
                    data = ex.Message
                };
            }
        }


        //-----------------------Ticket Reply Count
        public async Task<ResponseViewModel> adminReplyCount(Guid URID, Guid TicketId)
        {
            var procedureName = Constant.adminReplyCount;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", URID, DbType.Guid);
            parameters.Add("@TicketId", TicketId, DbType.Guid);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    var combinedData = new
                    {
                        adminReplyCount = result.ToList(),
                    };

                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "admin Reply Count.",
                        data = combinedData
                    };
                }
                else
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "admin Reply Count."
                    };
                }
            }
        }
        public async Task<ResponseViewModel> userReplyCount(Guid URID, Guid TicketId)
        {
            var procedureName = Constant.userReplyCount;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", URID, DbType.Guid);
            parameters.Add("@TicketId", TicketId, DbType.Guid);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    var combinedData = new
                    {
                        userReplyCount = result.ToList(),
                    };

                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "user Reply Count.",
                        data = combinedData
                    };
                }
                else
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "No user Reply Count."
                    };
                }
            }
        }

        public async Task<ResponseViewModel> updateAdminReplyCount(Guid URID, Guid TicketId)
        {
            var procedureName = Constant.updateAdminReplyCount;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", URID, DbType.Guid);
            parameters.Add("@TicketId", TicketId, DbType.Guid);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    var combinedData = new
                    {
                        updateAdminReplyCountticket = result.ToList(),
                    };

                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "update Admin Reply Count.",
                        data = combinedData
                    };
                }
                else
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "No update Admin Reply Count."
                    };
                }
            }
        }

        public async Task<ResponseViewModel> updateUserReplyCount(Guid URID, Guid TicketId)
        {
            var procedureName = Constant.updateUserReplyCount;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", URID, DbType.Guid);
            parameters.Add("@TicketId", TicketId, DbType.Guid);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    var combinedData = new
                    {
                        updateUserReplyCount = result.ToList(),
                    };

                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "update User Reply Count.",
                        data = combinedData
                    };
                }
                else
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "No update User Reply Count."
                    };
                }
            }
        }
    }
}
