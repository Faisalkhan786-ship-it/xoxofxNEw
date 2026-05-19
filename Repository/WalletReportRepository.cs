using Common;
using Dapper;
using Dapper;
using EmailSystem;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using RepositoryContract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ViewModel;
using static Repository.AuthenticationRepository;
using static System.Net.WebRequestMethods;

namespace Repository
{
    public class WalletReportRepository : IWalletReportRepository
    {
        private readonly DapperContext _dapperContext;
        private readonly EmailService _emailService;
        public WalletReportRepository(DapperContext dapperContext, EmailService emailService)
        {
            _dapperContext = dapperContext;
            _emailService = emailService;
        }
        public async Task<ResponseViewModel> getIncomeWalletWallerReport(WalletReportViewModel walletReportViewModel)
        {
            var procedureName = Constant.getInComeWalletStatement;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", walletReportViewModel.URID, DbType.Guid);
            parameters.Add("@transtype", walletReportViewModel.transtype, DbType.String);
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
                        message = "Data Not Found."
                    };
                }
                return returnData;
            }
        }

        public async Task<ResponseViewModel> getIncomeAndDepositTransType(Guid URID)
        {
            var incomeProc = Constant.getIncomeWalletTransType;
            var depositProc = Constant.getDepositWalletTransType;
            var roiProc = Constant.getRoiWalletTransType;

            var parameters = new DynamicParameters();
            parameters.Add("@URID", URID, DbType.Guid);

            using (var connection = _dapperContext.createConnection())
            {
                try
                {
                    var incomeResult = await connection.QueryAsync(incomeProc, parameters, commandType: CommandType.StoredProcedure);
                    var incomeList = incomeResult.ToList();

                    var depositResult = await connection.QueryAsync(depositProc, parameters, commandType: CommandType.StoredProcedure);
                    var depositList = depositResult.ToList();


                    var roiProcResult = await connection.QueryAsync(roiProc, parameters, commandType: CommandType.StoredProcedure);
                    var roilist = roiProcResult.ToList();

                    if ((incomeList != null && incomeList.Any()) || (depositList != null && depositList.Any()))
                    {
                        var message = "Data fetched successfully";
                        var combinedData = new
                        {
                            incomeTransTypes = incomeList,
                            depositTransTypes = depositList,
                            roiTransTypes = roilist
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
        //public async Task<ResponseViewModel> getDepositWalletReport(DepositReportViewModel depositReportViewModel)
        //{
        //    var procedureName = Constant.getDepositWalletStatement;
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@URID", depositReportViewModel.URID, DbType.Guid);
        //    parameters.Add("@transtype", depositReportViewModel.transtype, DbType.String);
        //    using (var connection = _dapperContext.createConnection())
        //    {
        //        var result = await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
        //        ResponseViewModel returnData;
        //        if (result != null && result.Any())
        //        {
        //            var validation = result.First();
        //            if (validation.statusCode == 1)
        //            {
        //                returnData = new ResponseViewModel
        //                {
        //                    statusCode = (int)HttpStatusCode.OK,
        //                    message = validation.message,
        //                    data = result
        //                };
        //            }
        //            else if (validation.statusCode == 0)
        //            {
        //                returnData = new ResponseViewModel
        //                {
        //                    statusCode = (int)HttpStatusCode.Conflict,
        //                    message = validation.message
        //                };
        //            }
        //            else if (validation.statusCode == -1)
        //            {
        //                returnData = new ResponseViewModel
        //                {
        //                    statusCode = (int)HttpStatusCode.Conflict,
        //                    message = validation.message
        //                };
        //            }
        //            else
        //            {
        //                returnData = new ResponseViewModel
        //                {
        //                    statusCode = (int)HttpStatusCode.BadRequest,
        //                    message = validation.message
        //                };
        //            }
        //        }
        //        else
        //        {
        //            returnData = new ResponseViewModel
        //            {
        //                statusCode = (int)HttpStatusCode.NotFound,
        //                message = "Something went to wrong with server error."
        //            };
        //        }
        //        return returnData;
        //    }
        //}
        public async Task<ResponseViewModel> getDepositWalletReport(DepositReportViewModel depositReportViewModel)
        {
            var procedureName = Constant.getDepositWalletStatement;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", depositReportViewModel.URID, DbType.Guid);
            parameters.Add("@transtype", depositReportViewModel.transtype, DbType.String);

            using (var connection = _dapperContext.createConnection())
            {
                var result = (await connection.QueryAsync(procedureName, parameters,
                    commandType: CommandType.StoredProcedure)).ToList();

                ResponseViewModel returnData;

                if (result != null && result.Any())
                {
                    var validation = result.First();
                    int statusCode = Convert.ToInt32(validation.statusCode);

                    if (statusCode == 1)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = validation.message.ToString(),
                            data = result
                        };
                    }
                    else if (statusCode == 0 || statusCode == -1)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.Conflict,
                            message = validation.message.ToString()
                        };
                    }
                    else
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.BadRequest,
                            message = validation.message.ToString()
                        };
                    }
                }
                else
                {
                    returnData = new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "Something went wrong with server error."
                    };
                }

                return returnData;
            }
        }
        public async Task<ResponseViewModel> getIncomeWithdrawalHistory(IncomeWithdrawalHistoryViewModel1 incomeWithdrawalHistoryViewModel)
        {
            var procedureName = Constant.incomeOrRoiWithdrawalHistory;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", incomeWithdrawalHistoryViewModel.URID, DbType.Guid);
            parameters.Add("@transtype", incomeWithdrawalHistoryViewModel.transtype ?? "", DbType.String);
            parameters.Add("@type", incomeWithdrawalHistoryViewModel.type, DbType.Int32);

            using (var connection = _dapperContext.createConnection())
            {
                using (var multi = await connection.QueryMultipleAsync(procedureName, parameters, commandType: CommandType.StoredProcedure))
                {
                    var status = await multi.ReadFirstOrDefaultAsync<ResponseViewModel>();

                    if (status != null && status.statusCode == 1)
                    {
                        var dataList = multi.Read<IncomeWithdrawalModel>().ToList();

                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = status.message,
                            data = dataList
                        };
                    }
                    else
                    {
                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.NotFound,
                            message = status?.message ?? "No data found",
                            data = null
                        };
                    }
                }
            }
        }
        public class IncomeWithdrawalModel
        {
            public Guid URID { get; set; }
            public decimal credit { get; set; }
            public decimal debit { get; set; }
            public string? TransType { get; set; }
            public string? Remark { get; set; }
            public string? payMode { get; set; }
            public string? TransHash { get; set; }
            public decimal TotWithdl { get; set; }
            public string? status { get; set; }
            public string? withdrawalmode { get; set; }
            public decimal AdminCharges { get; set; }
            public string? Transhash2 { get; set; }
            public string? CreatedDate { get; set; }
        }
        public async Task<ResponseViewModel> getRechargeTransact(Guid URID)
        {
            var incomeProc = Constant.getRechargeTransaction_ByTId;

            var parameters = new DynamicParameters();
            parameters.Add("@URID", URID, DbType.Guid);

            using (var connection = _dapperContext.createConnection())
            {
                try
                {
                    var incomeResult = await connection.QueryAsync<dynamic>(incomeProc, parameters, commandType: CommandType.StoredProcedure);
                    var incomeList = incomeResult.ToList();

                    // Check if it returned a fake 'no data' row
                    bool isFakeEmptyRow = incomeList.Count == 1 &&
                                          incomeList[0] is IDictionary<string, object> row &&
                                          row.ContainsKey("statusCode") &&
                                          row["statusCode"]?.ToString() == "0" &&
                                          row.ContainsKey("message") &&
                                          row["message"]?.ToString().ToLower().Contains("no data") == true;

                    if (incomeList != null && incomeList.Any() && !isFakeEmptyRow)
                    {
                        var message = "Data fetched successfully";
                        var combinedData = new
                        {
                            incomeTransTypes = incomeList
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
                            data = new { incomeTransTypes = new List<object>() }
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

        public async Task<ResponseViewModel> getRentWalletByURID(Guid URID)
        {
            var incomeProc = Constant.getRentWalletByURID;

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
                            RentWallet = incomeList,
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

        public async Task<ResponseViewModel> getRentWalletWallerReport(RentWalletReportViewModel rentWalletReportViewModel)
        {
            var procedureName = "";
            var parameters = new DynamicParameters();
            parameters.Add("@URID", rentWalletReportViewModel.URID, DbType.Guid);
            parameters.Add("@transtype", rentWalletReportViewModel.transtype, DbType.String);
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

        public async Task<ResponseViewModel> getleaderShipURID(Guid URID)
        {
            var incomeProc = Constant.get_Diff_Rank_LeaderShip;

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
                            leaderShip = incomeList,
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

        public async Task<ResponseViewModel> getPerformanceRewardList(Guid URID)
        {
            var incomeProc = Constant.getPerformanceRewardList;

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
                            PerformanceReward = incomeList,
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
        public async Task<ResponseViewModel> addRechargeTransact(AddRechargeTransactionViewModel model)
        {
            var procedureName = Constant.addRechargeTransaction;

            var parameters = new DynamicParameters();
            parameters.Add("@URID", model.URID, DbType.Guid);
            parameters.Add("@ProductId", model.ProductId, DbType.Guid);
            parameters.Add("@Rkprice", model.Rkprice, DbType.Decimal);
            parameters.Add("@LeaseDuration", model.LeaseDuration, DbType.Int32);
            parameters.Add("@createdBy", model.CreatedBy, DbType.Guid);
            parameters.Add("@ByURID", model.ByURID, DbType.Guid);

            using (var connection = _dapperContext.createConnection())
            {
                try
                {
                    // Step 1: Execute Stored Procedure
                    var result = await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);

                    // Step 2: Fetch First Row (for status/message)
                    var validation = result.FirstOrDefault();

                    // Step 3: If SP did not return anything
                    if (validation == null)
                    {
                        return new ResponseViewModel
                        {
                            statusCode = -2,
                            message = "No response from stored procedure.",
                            data = null
                        };
                    }

                    // Step 4: If successful
                    if (validation.statusCode == 1)
                    {
                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = validation.message ?? "Recharge successful.",
                            data = result  // Complete inserted record details
                        };
                    }

                    // Step 5: If failed (e.g. Insufficient balance)
                    return new ResponseViewModel
                    {
                        statusCode = validation.statusCode,
                        message = validation.message ?? "Something went wrong.",
                        data = null
                    };
                }
                catch (Exception ex)
                {
                    // Step 6: Catch and return error
                    return new ResponseViewModel
                    {
                        statusCode = -2,
                        message = "Exception: " + ex.Message,
                        data = null
                    };
                }
            }
        }


        //public async Task<ResponseViewModel> addRechargeTransact(AddRechargeTransactionViewModel model)
        //{
        //    var procedureName = Constant.addRechargeTransaction;
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@URID", model.URID, DbType.Guid);
        //    parameters.Add("@ProductId", model.ProductId, DbType.Guid);
        //    parameters.Add("@Rkprice", model.Rkprice, DbType.Decimal);
        //    parameters.Add("@LeaseDuration", model.LeaseDuration, DbType.Int32);
        //    parameters.Add("@createdBy", model.CreatedBy, DbType.Guid);
        //    parameters.Add("@ByURID", model.ByURID, DbType.Guid);


        //    using (var connection = _dapperContext.createConnection())
        //    {
        //        //  Recharge Transaction Insert
        //        var result = await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
        //        var validation = result.FirstOrDefault();

        //        int finalStatusCode = (validation?.statusCode == 1)
        //            ? (int)HttpStatusCode.OK
        //            : (validation?.statusCode ?? -2);

        //        object? finalData = null;
        //        if (validation?.statusCode == 1 && validation?.RechargeId != null)
        //        {
        //            Guid rechargeId = validation.RechargeId;
        //            var recDetailSp = Constant.getRecDetails_ByTId;
        //            var recParams = new DynamicParameters();
        //            recParams.Add("@RechargeId", rechargeId, DbType.Guid);

        //            //var recDetails = await connection.QueryAsync(recDetailSp, recParams, commandType: CommandType.StoredProcedure);
        //            //finalData = recDetails;  // response me ye data jayega
        //            //var emailSp = Constant.getorderLeaseSendEmail;
        //            //var emailParams = new DynamicParameters();
        //            //emailParams.Add("@RechargeId", rechargeId, DbType.Guid);

        //            //var emailData = await connection.QueryFirstOrDefaultAsync(emailSp, emailParams, commandType: CommandType.StoredProcedure);
        //        }

        //        return new ResponseViewModel
        //        {
        //            statusCode = finalStatusCode,
        //            message = validation?.message ?? "Something went wrong",
        //            data = finalData
        //        };
        //    }
        //}

        //public async Task<ResponseViewModel> addRechargeTransact(AddRechargeTransactionViewModel model)
        //{
        //    var procedureName = Constant.addRechargeTransaction;
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@URID", model.URID, DbType.Guid);
        //    parameters.Add("@ProductId", model.ProductId, DbType.Guid);
        //    parameters.Add("@Rkprice", model.Rkprice, DbType.Decimal);
        //    parameters.Add("@LeaseDuration", model.LeaseDuration, DbType.Int32);
        //    parameters.Add("@createdBy", model.CreatedBy, DbType.Guid);
        //    parameters.Add("@ByURID", model.ByURID, DbType.Guid);
        //    // 🔹 Step 2: Email ActionType decide karo
        //    int actionType = 1;
        //    using (var connection = _dapperContext.createConnection())
        //    {
        //        var result = await connection.QueryFirstOrDefaultAsync<EmailActionModel>(
        //            "Sp_GetEmailByActionType",
        //            commandType: CommandType.StoredProcedure
        //        );
        //        actionType = result?.ActionType ?? 1;
        //    }

        //    using (var connection = _dapperContext.createConnection())
        //    {
        //        // 1️⃣ Recharge Transaction Insert
        //        var result = await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
        //        var validation = result.FirstOrDefault();

        //        int finalStatusCode = (validation?.statusCode == 1)
        //            ? (int)HttpStatusCode.OK
        //            : (validation?.statusCode ?? -2);

        //        object? finalData = null;

        //        // अगर Insert Success हुआ
        //        if (validation?.statusCode == 1 && validation?.RechargeId != null)
        //        {
        //            Guid rechargeId = validation.RechargeId;
        //            var recDetailSp = Constant.getRecDetails_ByTId;
        //            var recParams = new DynamicParameters();
        //            recParams.Add("@RechargeId", rechargeId, DbType.Guid);

        //            var recDetails = await connection.QueryAsync(recDetailSp, recParams, commandType: CommandType.StoredProcedure);
        //            finalData = recDetails;  // response me ye data jayega
        //            var emailSp = Constant.getorderLeaseSendEmail;
        //            var emailParams = new DynamicParameters();
        //            emailParams.Add("@RechargeId", rechargeId, DbType.Guid);

        //            var emailData = await connection.QueryFirstOrDefaultAsync(emailSp, emailParams, commandType: CommandType.StoredProcedure);

        //            if (emailData != null && emailData.statusCode == 1)
        //            {
        //                _emailService.SendOrderLeaseEmail(emailData, actionType);
        //            }
        //        }

        //        return new ResponseViewModel
        //        {
        //            statusCode = finalStatusCode,
        //            message = validation?.message ?? "Something went wrong",
        //            data = finalData
        //        };
        //    }
        //}

        //public async Task<ResponseViewModel> addRechargeTransact(AddRechargeTransactionViewModel model)
        //{
        //    var procedureName = Constant.addRechargeTransaction;
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@URID", model.URID, DbType.Guid);
        //    parameters.Add("@ProductId", model.ProductId, DbType.Guid);
        //    parameters.Add("@Rkprice", model.Rkprice, DbType.Decimal);
        //    parameters.Add("@LeaseDuration", model.LeaseDuration, DbType.Int32);
        //    parameters.Add("@createdBy", model.CreatedBy, DbType.Guid);
        //    parameters.Add("@ByURID", model.ByURID, DbType.Guid);

        //    using (var connection = _dapperContext.createConnection())
        //    {
        //        var result = await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
        //        var validation = result.FirstOrDefault();

        //        int finalStatusCode = (validation?.statusCode == 1)
        //            ? (int)HttpStatusCode.OK
        //            : (validation?.statusCode ?? -2);
        //        object? finalData = null;               
        //        return new ResponseViewModel
        //        {
        //            statusCode = finalStatusCode,
        //            message = validation?.message ?? "Something went wrong",
        //            data = finalData
        //        };
        //    }
        //}

        public async Task<ResponseViewModel> getNetworkTree(string authlogin)
        {
            var procedureName = Constant.getNetworkTree;
            var parameters = new DynamicParameters();
            parameters.Add("@AuthLogin", authlogin, DbType.String);
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

        public async Task<ResponseViewModel> getTransactionHistory(IncomeWithdrawalHistoryViewModel incomeWithdrawalHistoryViewModel)
        {
            var procedureName = Constant.get_TransactionIncome_History;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", incomeWithdrawalHistoryViewModel.URID, DbType.Guid);
            parameters.Add("@transtype", incomeWithdrawalHistoryViewModel.transtype, DbType.String);
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

        public async Task<ResponseViewModel> updateRentWalletAdress(UpdateRentWalletAdressViewModel updateRentWalletAdressViewModel)
        {
            //var procedureName = Constant.updateRentWalletAdress;
            var procedureName = "";
            var parameters = new DynamicParameters();
            parameters.Add("@AuthLoginId", updateRentWalletAdressViewModel.AuthLoginId, DbType.String);
            parameters.Add("@debit", updateRentWalletAdressViewModel.debit, DbType.Decimal);
            parameters.Add("@WalletAdreess", updateRentWalletAdressViewModel.Wallet, DbType.String);
            parameters.Add("@TransHash", updateRentWalletAdressViewModel.TransHash, DbType.String);
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

        public async Task<ResponseViewModel> updateIncomeWalletAdress(UpdateIncometWalletAdressViewModel updateIncometWalletAdressViewModel)
        {
            var procedureName = Constant.updateIncomeWalletAdress;
            var parameters = new DynamicParameters();
            parameters.Add("@AuthLoginId", updateIncometWalletAdressViewModel.AuthLoginId, DbType.String);
            parameters.Add("@debit", updateIncometWalletAdressViewModel.debit, DbType.Decimal);
            parameters.Add("@WalletAdreess", updateIncometWalletAdressViewModel.Wallet, DbType.String);
            parameters.Add("@TransHash", updateIncometWalletAdressViewModel.TransHash, DbType.String);
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

        public async Task<ResponseViewModel> getAccStatemtnt(accStateMent accStateMent)
        {
            var procedureName = Constant.accStatementAccType;
            var parameters = new DynamicParameters();

            // Default wtype to 1 if 0 or invalid
            var walletType = accStateMent.wtype <= 0 ? 1 : accStateMent.wtype;

            // Default dates if empty
            var fromDate = string.IsNullOrWhiteSpace(accStateMent.FromDate) ? "01-01-2025" : accStateMent.FromDate;
            var toDate = string.IsNullOrWhiteSpace(accStateMent.ToDate) ? DateTime.Now.ToString("dd-MM-yyyy") : accStateMent.ToDate;

            parameters.Add("@AuthLogin", accStateMent.AuthLogin ?? "", DbType.String);
            parameters.Add("@wtype", walletType, DbType.Int64);
            parameters.Add("@transtype", accStateMent.transtype ?? "", DbType.String);
            parameters.Add("@FromDate", fromDate, DbType.String);
            parameters.Add("@ToDate", toDate, DbType.String);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
                ResponseViewModel returnData;

                if (result != null && result.Any())
                {
                    var validation = result.First();
                    int status = validation.statusCode ?? -1;

                    switch (status)
                    {
                        case 1:
                            returnData = new ResponseViewModel
                            {
                                statusCode = (int)HttpStatusCode.OK,
                                message = validation.message,
                                data = result
                            };
                            break;
                        case 0:
                        case -1:
                            returnData = new ResponseViewModel
                            {
                                statusCode = (int)HttpStatusCode.Conflict,
                                message = validation.message
                            };
                            break;
                        default:
                            returnData = new ResponseViewModel
                            {
                                statusCode = (int)HttpStatusCode.BadRequest,
                                message = validation.message ?? "Unknown error occurred"
                            };
                            break;
                    }
                }
                else
                {
                    returnData = new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "No data found for the given criteria."
                    };
                }

                return returnData;
            }
        }

        public async Task<ResponseViewModel> getAllWalletHistory(AllWalletHistory allWalletHistory)
        {
            var procedureName = Constant.getAllWalletHistory;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", allWalletHistory.URID, DbType.Guid);
            parameters.Add("@WalletType", allWalletHistory.WalletType, DbType.String);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    var validation = result.First();
                    int? status = validation.statusCode;

                    // ✅ If data exists, status 200 regardless of SP value
                    if (status == 1 || result.Count() > 0)
                    {
                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = validation.message ?? "Success",
                            data = result
                        };
                    }
                    else
                    {
                        // Data found but SP says -1 or 0
                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = "Data fetched successfully.",
                            data = result
                        };
                    }
                }
                else
                {
                    // ❌ No data case
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "No data found for the given criteria."
                    };
                }
            }
        }

        public async Task<ResponseViewModel> getRechargeTransactionAdmin(RechargeTransactionAdminViewModel rechargeTransactionAdminViewModel)
        {
            var procedureName = Constant.addRechargeTransactionAdmin;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", rechargeTransactionAdminViewModel.URID, DbType.Guid);
            parameters.Add("@ProductId", rechargeTransactionAdminViewModel.ProductId, DbType.Guid);
            parameters.Add("@LeaseDuration", rechargeTransactionAdminViewModel.LeaseDuration, DbType.Int32);
            parameters.Add("@PackageType", rechargeTransactionAdminViewModel.PackageType, DbType.Int32);
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
                        message = "Data Not Found."
                    };
                }
                return returnData;
            }
        }


        public async Task<ResponseViewModel> getDownloadleaseagentbyRID(Guid RechargeId)
        {
            var incomeProc = Constant.recDetails_ByTId;
            var parameters = new DynamicParameters();
            parameters.Add("@RechargeId", RechargeId, DbType.Guid);

            using (var connection = _dapperContext.createConnection())
            {
                try
                {
                    var incomeResult = await connection.QueryAsync(incomeProc, parameters, commandType: CommandType.StoredProcedure);
                    var incomeList = incomeResult.ToList();

                    if (incomeList != null && incomeList.Any())
                    {
                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = "Data fetched successfully",
                            data = new { leaseagent = incomeList }
                        };
                    }
                    else
                    {
                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.NotFound,
                            message = "No data found for this RechargeId.",
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
        public async Task<ResponseViewModel> addRechargeTransactionAdmin(AddRechargeTransactionAdminViewModel addRechargeTransactionAdminViewModel)
        {
            var procedureName = Constant.SpAddRechargeTransactionAdmin;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", addRechargeTransactionAdminViewModel.URID, DbType.Guid);
            parameters.Add("@PackageType", addRechargeTransactionAdminViewModel.PackageType, DbType.Int32);
            parameters.Add("@USDTValue", addRechargeTransactionAdminViewModel.USDTValue, DbType.Int32);
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
                        message = "Data Not Found."
                    };
                }
                return returnData;
            }
        }

        public async Task<ResponseViewModel> addRechargeTransactionUser(AddRechargeTransactionUserViewModel addRechargeTransactionUserViewModel)
        {
            var procedureName = Constant.SpAddRechargeTransactionUser;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", addRechargeTransactionUserViewModel.URID, DbType.Guid);
            parameters.Add("@PackageType", addRechargeTransactionUserViewModel.PackageType, DbType.Int32);
            parameters.Add("@createdBy", addRechargeTransactionUserViewModel.createdBy, DbType.Guid);
            parameters.Add("@ByURID", addRechargeTransactionUserViewModel.ByURID, DbType.Guid);
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
                        message = "Data Not Found."
                    };
                }
                return returnData;
            }
        }
        public async Task<ResponseViewModel> getBindBuyPackageList(Guid URID)
        {
            var incomeProc = Constant.bindBuyPackage;

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
                            bindBuyPackage = incomeList,
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
        public async Task<ResponseViewModel> getSingleLeg_Report(String AuthLogin)
        {
            var incomeProc = Constant.getSingleLeg_Report;
            var parameters = new DynamicParameters();
            parameters.Add("@AuthLogin", AuthLogin, DbType.String);

            using (var connection = _dapperContext.createConnection())
            {
                try
                {
                    var incomeResult = await connection.QueryAsync(incomeProc, parameters, commandType: CommandType.StoredProcedure);
                    var incomeList = incomeResult.ToList();

                    if (incomeList != null && incomeList.Any())
                    {
                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = "Data fetched successfully",
                            data = new { leaseagent = incomeList }
                        };
                    }
                    else
                    {
                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.NotFound,
                            message = "No data found for this RechargeId.",
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
        public async Task<ResponseViewModel> getUserAllWalletBalance(Guid URID)
        {
            var incomeProc = Constant.getUser_WalletBalance;

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
                            WalletBalance = incomeList,
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

        public async Task<ResponseViewModel> genrateROI_BOTCLICK(Guid URID)
        {
            var incomeProc = "";
            //var incomeProc = Constant.genrateROI_BOTCLICK;

            var parameters = new DynamicParameters();
            parameters.Add("@URID", URID, DbType.Guid);

            using (var connection = _dapperContext.createConnection())
            {
                try
                {
                    // ❗ SP only performs INSERT/UPDATE — No SELECT result  
                    var rowsAffected = await connection.ExecuteAsync(
                        incomeProc,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    // SP has no result, so only success message
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "ROI generated successfully.",
                        data = new { AffectedRows = rowsAffected }
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


        public async Task<ResponseViewModel> checkROI_BOTCLICK(Guid URID)
        {
            //var incomeProc = Constant.checkROI_BOTCLICK;
            var incomeProc = "";

            var parameters = new DynamicParameters();
            parameters.Add("@URID", URID, DbType.Guid);

            using (var connection = _dapperContext.createConnection())
            {
                try
                {
                    // SP returns exactly ONE integer: checkROItoday
                    var result = await connection.QueryFirstOrDefaultAsync<int>(
                        incomeProc,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    var response = new
                    {
                        checkROItoday = result   // 0 = not generated, 1 = already generated today
                    };

                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "Data fetched successfully",
                        data = response
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

        public async Task<ResponseViewModel> getSettings()
        {
            //var incomeProc = Constant.getSettingsDetails;
            var incomeProc = "";
            var parameters = new DynamicParameters();

            using (var connection = _dapperContext.createConnection())
            {
                try
                {
                    var incomeResult = await connection.QueryAsync(incomeProc, parameters, commandType: CommandType.StoredProcedure);
                    var incomeList = incomeResult.ToList();

                    if (incomeList != null && incomeList.Any())
                    {
                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = "Data fetched successfully",
                            data = new { leaseagent = incomeList }
                        };
                    }
                    else
                    {
                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.NotFound,
                            message = "No data found for this RechargeId.",
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


        public async Task<ResponseViewModel> updateSettings(updateSettingsViewModel updateSettingsViewModel)
        {
            var procedureName = "";
            //var procedureName = Constant.updateSettingsAdmin;
            var parameters = new DynamicParameters();
            parameters.Add("@sid", updateSettingsViewModel.sid, DbType.Int32);
            parameters.Add("@limits", updateSettingsViewModel.limits, DbType.Decimal);

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
        public async Task<ResponseViewModel> getROIWalletWallerReport(ROIWalletReportViewModel rOIWalletReportViewModel)
        {
            try
            {
                var procedureName = "SpGetROIWalletStatement";
                var parameters = new DynamicParameters();
                parameters.Add("@URID", rOIWalletReportViewModel.URID, DbType.Guid);
                parameters.Add("@transtype", rOIWalletReportViewModel.transtype ?? "", DbType.String);

                using (var connection = _dapperContext.createConnection())
                {
                    var result = (await connection.QueryAsync(
                        procedureName,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    )).ToList();

                    if (result != null && result.Count > 0)
                    {
                        var validation = result.FirstOrDefault();

                        // 🔥 SAFE access (dynamic crash avoid)
                        int statusCode = validation?.statusCode ?? -99;
                        string message = validation?.message ?? "Something went wrong";

                        if (statusCode == 1)
                        {
                            return new ResponseViewModel
                            {
                                statusCode = (int)HttpStatusCode.OK,
                                message = message,
                                data = result
                            };
                        }
                        else if (statusCode == 0 || statusCode == -1)
                        {
                            return new ResponseViewModel
                            {
                                statusCode = (int)HttpStatusCode.Conflict,
                                message = message,
                                data = null
                            };
                        }
                        else
                        {
                            return new ResponseViewModel
                            {
                                statusCode = (int)HttpStatusCode.BadRequest,
                                message = message,
                                data = null
                            };
                        }
                    }
                    else
                    {
                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.NotFound,
                            message = "Data Not Found.",
                            data = null
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                //  THIS was missing → 500 error ka main fix
                return new ResponseViewModel
                {
                    statusCode = 500,
                    message = ex.Message,
                    data = null
                };
            }
        }
        public async Task<ResponseViewModel> getUplineTeamList(string authlogin)
        {
            var procedureName = "";
            //var procedureName = Constant.getUplineTeamList;
            var parameters = new DynamicParameters();
            parameters.Add("@AuthLogin", authlogin, DbType.String);
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
        public async Task<ResponseViewModel> userSearchBindBuyPackage(string AuthLogin)
        {
            //var procedureName = Constant.userSearchBindBuyPackage;
            var procedureName = "";
            var parameters = new DynamicParameters();
            parameters.Add("@AuthLogin", AuthLogin, DbType.String);
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

        public async Task<ResponseViewModel> getSalaryRankList(Guid URID)
        {
            var procedureName = "";
            //var procedureName = Constant.getSalaryRankList;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", URID, DbType.Guid);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    var validation = result.First();

                    int statusCode = Convert.ToInt32(validation.statuscode);  
                    string message = Convert.ToString(validation.message);   

                    if (statusCode == 1)
                    {
                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = message,
                            data = result
                        };
                    }
                    else if (statusCode == 0 || statusCode == -1)
                    {
                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.Conflict,
                            message = message
                        };
                    }
                    else
                    {
                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.BadRequest,
                            message = message
                        };
                    }
                }
                else
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "Something went wrong with server."
                    };
                }
            }
        }

       
    }
}


