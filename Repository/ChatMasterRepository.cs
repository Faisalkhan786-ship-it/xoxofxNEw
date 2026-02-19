using Dapper;
using RepositoryContract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ViewModel;
using Common;
using MimeKit.Encodings;

namespace Repository
{
    public class ChatMasterRepository : IChatMasterRepository
    {
        private readonly DapperContext _dapperContext;
        public ChatMasterRepository(DapperContext dapperContext) =>
            _dapperContext = dapperContext;

        //public async Task<ResponseViewModelchatmaster> addChatMessage(ChatMasterViewModel chatMasterViewModel)
        //{
        //    var procedureName = Constant.insertChatMessage;
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@ChatId", chatMasterViewModel.ChatId, DbType.Int32);
        //    parameters.Add("@UserId", chatMasterViewModel.UserId, DbType.Guid);
        //    parameters.Add("@MessageText", chatMasterViewModel.MessageText, DbType.String);
        //    parameters.Add("@IsUser", chatMasterViewModel.IsUser, DbType.Int32);

        //    using (var connection = _dapperContext.createConnection())
        //    {
        //        var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModelchatmaster>(procedureName, parameters, commandType: CommandType.StoredProcedure);
        //        if (result.statusCode == 1)
        //        {
        //            result.statusCode = (int)HttpStatusCode.OK;
        //            result.message = result.message;
        //        }
        //        else if (result.statusCode == 0)
        //        {
        //            result.statusCode = (int)HttpStatusCode.ExpectationFailed;
        //            result.message = result.message;
        //        }
        //        else
        //        {
        //            result.statusCode = (int)HttpStatusCode.ExpectationFailed;
        //            result.message = result.message;
        //        }
        //        return result;
        //    }
        //}

        //public async Task<ResponseViewModelchatmaster> addChatMessage(ChatMasterViewModel chatMasterViewModel)
        //{
        //    var procedureName = Constant.insertChatMessage;
        //    var parameters = new DynamicParameters();

        //    parameters.Add("@ChatId", chatMasterViewModel.ChatId, DbType.Int32);
        //    parameters.Add("@UserId", chatMasterViewModel.UserId, DbType.Guid);
        //    parameters.Add("@MessageText", chatMasterViewModel.MessageText, DbType.String);
        //    parameters.Add("@IsUser", chatMasterViewModel.IsUser, DbType.Int32);

        //    using (var connection = _dapperContext.createConnection())
        //    {
        //        var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModelchatmaster>(
        //            procedureName, parameters,
        //            commandType: CommandType.StoredProcedure
        //        );

        //        // status mapping
        //        if (result != null && result.statusCode == 1)
        //        {
        //            result.statusCode = (int)HttpStatusCode.OK;
        //        }
        //        else
        //        {
        //            result.statusCode = (int)HttpStatusCode.ExpectationFailed;
        //        }

        //        return result;
        //    }
        //}


        //ye wali use ho rahi hai me 
        public async Task<ResponseViewModelchatmaster> addChatMessage(ChatMasterViewModel chatMasterViewModel)
        {
            var procedureName = Constant.insertChatMessage;
            var parameters = new DynamicParameters();

            parameters.Add("@ChatId", chatMasterViewModel.ChatId, DbType.Int32);
            parameters.Add("@UserId", chatMasterViewModel.UserId, DbType.Guid);
            parameters.Add("@MessageText", chatMasterViewModel.MessageText, DbType.String);
            parameters.Add("@IsUser", chatMasterViewModel.IsUser, DbType.Int32);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModelchatmaster>(
                    procedureName, parameters,
                    commandType: CommandType.StoredProcedure
                );

                if (result != null && result.statusCode == 1)
                    result.statusCode = (int)HttpStatusCode.OK;
                else
                    result.statusCode = (int)HttpStatusCode.ExpectationFailed;

                return result;
            }
        }

        public async Task<ResponseViewModelNewChat> addNewChat(NewChatViewModel newChatViewModel)
        {
            var procedureName = Constant.insertNewChat;
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", newChatViewModel.UserId, DbType.Guid);
            parameters.Add("@ChatName", newChatViewModel.ChatName, DbType.String);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModelNewChat>(
                    procedureName, parameters,
                    commandType: CommandType.StoredProcedure
                );

                // status mapping
                if (result != null && result.statusCode == 1)
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

        public async Task<ResponseViewModel> getUserAllChatsbyUserId(Guid USERID)
        {
            var procedureName = Constant.getUserChats;

            using (var connection = _dapperContext.createConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@USERID", USERID, DbType.Guid);

                var result = await connection.QueryAsync<dynamic>(
                    procedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                if (result == null || !result.Any())
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "No chats found",
                        data = new List<dynamic>()
                    };
                }

                var validation = result.First();

                // statusCode from SQL result
                int status = validation.statuscode;

                if (status == 1)
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = validation.message,
                        data = result
                    };
                }
                else if (status == 0)
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.Conflict,
                        message = validation.message
                    };
                }
                else
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.BadRequest,
                        message = validation.message
                    };
                }
            }
        }


        public async Task<ResponseViewModel> getChatMessagesChatId(ChatMessagesViewModel chatMessagesViewModel)
        {
            var procedureName = Constant.getChatMessages;

            using (var connection = _dapperContext.createConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ChatId", chatMessagesViewModel.ChatId, DbType.Int32);
                parameters.Add("@UserId", chatMessagesViewModel.UserId, DbType.Guid);

                var result = await connection.QueryAsync<dynamic>(
                    procedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                if (result == null || !result.Any())
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "Something went wrong with the server."
                    };
                }

                var validation = result.First();

                // statusCode from SQL result
                int status = validation.statuscode;

                if (status == 1)
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = validation.message,
                        data = result
                    };
                }
                else if (status == 0)
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.Conflict,
                        message = validation.message
                    };
                }
                else
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.BadRequest,
                        message = validation.message
                    };
                }
            }
        }

        public async Task<ResponseViewModel> getUserAllChatsAdmin(Guid USERID)
        {
            var procedureName = Constant.getUserChatsAdmin;

            using (var connection = _dapperContext.createConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@USERID", USERID, DbType.Guid);

                var result = await connection.QueryAsync<dynamic>(
                    procedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                if (result == null || !result.Any())
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "No chats found",
                        data = new List<dynamic>()
                    };
                }

                var validation = result.First();

                // statusCode from SQL result
                int status = validation.statuscode;

                if (status == 1)
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = validation.message,
                        data = result
                    };
                }
                else if (status == 0)
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.Conflict,
                        message = validation.message
                    };
                }
                else
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.BadRequest,
                        message = validation.message
                    };
                }
            }
        }

        public async Task<ResponseViewModel> chatMsgByIdAdmin(int ChatId)
        {
            var procedureName = Constant.chatMsgByIdAdmin;

            using (var connection = _dapperContext.createConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ChatId", ChatId, DbType.Int32);

                var result = await connection.QueryAsync<dynamic>(
                    procedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                if (result == null || !result.Any())
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "No chats found",
                        data = new List<dynamic>()
                    };
                }

                var validation = result.First();

                // statusCode from SQL result
                int status = validation.statuscode;

                if (status == 1)
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = validation.message,
                        data = result
                    };
                }
                else if (status == 0)
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.Conflict,
                        message = validation.message
                    };
                }
                else
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.BadRequest,
                        message = validation.message
                    };
                }
            }
        }

        public async Task<ResponseViewModel> useCredit(UseCreditViewModel useCreditViewModel)
        {
            var procedureName = Constant.updateCredit;
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", useCreditViewModel.UserId, DbType.Guid);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModel>(
                    procedureName, parameters,
                    commandType: CommandType.StoredProcedure
                );

                // status mapping
                if (result != null && result.statusCode == 1)
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

        public async Task<ResponseViewModel> userDeleteChat(ChatMessagesViewModel chatMessagesViewModel)
        {
            var procedureName = Constant.userDeleteChat;
            var parameters = new DynamicParameters();
            parameters.Add("@USERID", chatMessagesViewModel.UserId, DbType.Guid);
            parameters.Add("@ChatId", chatMessagesViewModel.ChatId, DbType.Int32);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModel>(
                    procedureName, parameters,
                    commandType: CommandType.StoredProcedure
                );

                // status mapping
                if (result != null && result.statusCode == 1)
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
    }
}
