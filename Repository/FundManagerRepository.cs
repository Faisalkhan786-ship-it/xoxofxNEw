using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ViewModel;
using RepositoryContract;
using static Model.ModelType;
using Common;
using Azure;

namespace Repository
{
    public class FundManagerRepository : IFundManagerRepository
    {
        private readonly DapperContext _dapperContext;
        public FundManagerRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<ResponseViewModel> addUploadFund(FundManagerViewModel fundManagerViewModel)
        {
            var procedureName = Constant.addFundRequest;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", fundManagerViewModel.URID, DbType.Guid);
            parameters.Add("@PaymentMode", fundManagerViewModel.PaymentMode, DbType.String);
            parameters.Add("@Amount", fundManagerViewModel.Amount, DbType.Decimal);
            parameters.Add("@DepositDetails", fundManagerViewModel.DepositDetails, DbType.String);
            parameters.Add("@RefrenceNo", fundManagerViewModel.RefrenceNo, DbType.String);
            parameters.Add("@Remark", fundManagerViewModel.Remark, DbType.String);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModel>(
                  procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (result != null)
                {
                    if (result.statusCode == 1)
                    {
                        result.statusCode = (int)HttpStatusCode.OK;
                        result.message = result.message;
                    }
                    else if (result.statusCode == 0 || result.statusCode == 2)
                    {
                        result.statusCode = (int)HttpStatusCode.ExpectationFailed;
                        result.message = result.message;
                    }
                    return result;
                }
                else
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.InternalServerError,
                        message = "No response from stored procedure"
                    };
                }

            }
        }

        //public async Task<ResponseViewModel> getUserWalletDetails(Guid URID)
        //{
        //    var procedureName1 = Constant.getFundRequestReport;
        //    var procedureName2 = Constant.spGetUser_WalletBalance;

        //    var parameters = new DynamicParameters();
        //    parameters.Add("@URID", URID, DbType.Guid);

        //    using (var connection = _dapperContext.createConnection())
        //    {
        //        var fundResult = await connection.QueryAsync<dynamic>(
        //            procedureName1, parameters, commandType: CommandType.StoredProcedure);

        //        var walletResult = await connection.QueryAsync<WalletBalanceModel>(
        //            procedureName2, parameters, commandType: CommandType.StoredProcedure);

        //        var fundList = fundResult?.ToList();
        //        var walletList = walletResult?.ToList();

        //        ResponseViewModel returnData;

        //        if (fundList != null && fundList.Any())
        //        {
        //            var validation = fundList.First();

        //            if (validation.statusCode == 1)
        //            {
        //                returnData = new ResponseViewModel
        //                {
        //                    statusCode = (int)HttpStatusCode.OK,
        //                    message = validation.message,
        //                    data = new
        //                    {
        //                        FundRequests = fundList,
        //                        WalletBalance = walletList
        //                    }
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
        //                message = "Something went wrong with server error."
        //            };
        //        }

        //        return returnData;
        //    }
        //}

        public async Task<ResponseViewModel> getUserWalletDetails(Guid URID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@URID", URID, DbType.Guid);

            using (var connection = _dapperContext.createConnection())
            {
                var fundResult = await connection.QueryAsync<dynamic>(
                    Constant.getFundRequestReport,
                    parameters,
                    commandType: CommandType.StoredProcedure);

                var walletResult = await connection.QueryAsync<WalletBalanceModel>(
                    Constant.spGetUser_WalletBalance,
                    parameters,
                    commandType: CommandType.StoredProcedure);

                var fundList = fundResult?.ToList();
                var walletList = walletResult?.ToList();

                bool hasFundData = fundList != null && fundList.Any();
                bool hasWalletData = walletList != null && walletList.Any();

                // ❌ dono empty
                if (!hasFundData && !hasWalletData)
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "No data found"
                    };
                }

                // ✅ statusCode/message fund se uthao agar available ho
                var validation = hasFundData ? fundList.First() : null;

                return new ResponseViewModel
                {
                    statusCode = (int)HttpStatusCode.OK,
                    message = validation?.message ?? "Data fetched successfully",
                    data = new
                    {
                        FundRequests = hasFundData ? fundList : new List<object>(),
                        WalletBalance = hasWalletData ? walletList : new List<WalletBalanceModel>()
                    }
                };
            }
        }



        public class WalletBalanceModel
        {
            public Guid URID { get; set; }
            public decimal IncomeWallet { get; set; }
            public decimal DepositWallet { get; set; }
            public decimal RentWallet { get; set; }
            public decimal ROIWallet { get; set; }

            public int statusCode { get; set; }
            public string message { get; set; }
        }

        public class UserWalletDetailsResponse
        {
            public List<FundManualRequestModel> ManualRequests { get; set; }
            public decimal IncomeWallet { get; set; }
            public decimal DepositWallet { get; set; }
        }

        public class FundManualRequestModel
        {
            public string? AuthLogin { get; set; }
            public string? Bank { get; set; }
            public string? PaymentMode { get; set; }
            public string? PaymentDate { get; set; }
            public decimal Amount { get; set; }
            public string? Rf_Status { get; set; }
            public string? RefrenceNo { get; set; }
            public string? AdminRemark { get; set; }
            public string? Docpath { get; set; }
        }

        public async Task<ResponseViewModel> getPayModeMaster()
        {
            var procedureName = Constant.spPayModeMaster;
            var parameters = new DynamicParameters();

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<PayModeMaster>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);

                var dataList = result.ToList();

                if (dataList.Any())
                {
                    return new ResponseViewModel
                    {
                        statusCode = 200,
                        message = "Data fetched successfully",
                        data = dataList
                    };
                }
                else
                {
                    return new ResponseViewModel
                    {
                        statusCode = 404,
                        message = "No data found",
                        data = null
                    };
                }
            }
        }


        public class PayModeMaster
        {
            public int Id { get; set; }
            public string? PaymentMode { get; set; }
            public string? PaymentDetails { get; set; }
            public int Status { get; set; }
        }
        public async Task<ResponseViewModel> getMandatoryWithdrawalDetails(WithDrawalRequestViewModel withDrawalRequestViewModel)
        {
            var procedureName = Constant.spGetMandatoryDetailsWithdrawl;
            var procedureNameWalletBalance = Constant.spGetIncomeWalletBalance;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", withDrawalRequestViewModel.URID, DbType.Guid);
            using (var connection = _dapperContext.createConnection())
            {
                var withdrawalResult = await connection.QueryAsync<dynamic>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);
                var walletResult = await connection.QueryFirstOrDefaultAsync<decimal>(
                    procedureNameWalletBalance, parameters, commandType: CommandType.StoredProcedure);
                var combinedData = new
                {
                    WithdrawalDetails = withdrawalResult,
                    WalletBalance = walletResult
                };

                if (withdrawalResult != null && withdrawalResult.Any())
                {
                    var validation = withdrawalResult.First();
                    int status = validation.statusCode == 1 ? (int)HttpStatusCode.OK : (int)HttpStatusCode.Conflict;

                    return new ResponseViewModel
                    {
                        statusCode = status,
                        message = validation.message,
                        data = combinedData
                    };
                }
                else
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "Something went wrong with the server.",
                        data = null
                    };
                }
            }
        }

        public async Task<ResponseViewModel> addRequestUserwithdrawalCoin(RequestUserwithdrawalCoin requestUserwithdrawalCoin)
        {
            var procedureName = Constant.addRequestUserwithdrawal;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", requestUserwithdrawalCoin.URID, DbType.Guid);
            parameters.Add("@SecureCode", requestUserwithdrawalCoin.SecureCode, DbType.String);
            parameters.Add("@IpAddress", requestUserwithdrawalCoin.IpAddress, DbType.String);
            parameters.Add("@Amount", requestUserwithdrawalCoin.Amount, DbType.Decimal);
            parameters.Add("@Emailid", requestUserwithdrawalCoin.Emailid, DbType.String);
            parameters.Add("@WalletAdress", requestUserwithdrawalCoin.WalletAdress, DbType.String);
            parameters.Add("@PayMode", requestUserwithdrawalCoin.PayMode, DbType.Int32);
            parameters.Add("@walletType", requestUserwithdrawalCoin.walletType, DbType.Int32);
            parameters.Add("@intResult", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using (var connection = _dapperContext.createConnection())
            {
                await connection.ExecuteAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);

                var intResult = parameters.Get<int>("@intResult");

                var response = new ResponseViewModel();

                if (intResult > 0)
                {
                    response.statusCode = (int)HttpStatusCode.OK;
                    response.message = "Withdrawal request submitted successfully.";
                    response.data = intResult;
                }
                else if (intResult == -2)
                {
                    response.statusCode = (int)HttpStatusCode.Conflict;
                    response.message = "Insufficient balance.";
                }
                else
                {
                    response.statusCode = (int)HttpStatusCode.ExpectationFailed;
                    response.message = "Minimum Amount Should be 10.";
                }

                return response;
            }
        }

        public async Task<ResponseViewModel> transferP2(P2PViewModel P2PViewModel)
        {
            var procedureName = Constant.fundTransferDepositToDeposit;
            var parameters = new DynamicParameters();

            parameters.Add("@URID", P2PViewModel.URID, DbType.Guid);
            parameters.Add("@AuthLoginReciver", P2PViewModel.AuthLoginReciver, DbType.String);
            // parameters.Add("@fundtye", P2PViewModel.fundtye, DbType.String);
            parameters.Add("@trnsamount", P2PViewModel.trnsamount, DbType.Int64);


            parameters.Add("@intResult", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using (var connection = _dapperContext.createConnection())
            {
                await connection.ExecuteAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);

                var intResult = parameters.Get<int>("@intResult");

                var response = new ResponseViewModel();

                if (intResult > 0)
                {
                    response.statusCode = (int)HttpStatusCode.OK;
                    response.message = "FUND TRANSFER successfully.";
                    response.data = intResult;
                }
                else if (intResult == -2)
                {
                    response.statusCode = (int)HttpStatusCode.Conflict;
                    response.message = "Insufficient balance.";
                }
                else
                {
                    response.statusCode = (int)HttpStatusCode.ExpectationFailed;
                    response.message = "Failed to process withdrawal request.";
                }

                return response;
            }
        }

        public async Task<ResponseViewModel> getUserWalletBalance(Guid URID)
        {
            var procedureName = Constant.spGetUser_WalletBalance;
            var depositWalletReportProc = Constant.spDepositWalletReport;

            var parameters = new DynamicParameters();
            parameters.Add("@URID", URID, DbType.Guid);

            using (var connection = _dapperContext.createConnection())
            {
                var walletResult = await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);

                var depositReportResult = await connection.QueryAsync(depositWalletReportProc, parameters, commandType: CommandType.StoredProcedure);

                ResponseViewModel returnData;

                if (walletResult != null && walletResult.Any())
                {
                    var validation = walletResult.First();
                    int statusCode = validation.statusCode;

                    // Combine both results into one object
                    var combinedResult = new
                    {
                        WalletBalance = walletResult,
                        DepositWalletReport = depositReportResult
                    };

                    returnData = new ResponseViewModel
                    {
                        statusCode = statusCode == 1 ? (int)HttpStatusCode.OK : (int)HttpStatusCode.Conflict,
                        message = validation.message,
                        data = statusCode == 1 ? combinedResult : null
                    };
                }
                else
                {
                    returnData = new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "Something went wrong with the server."
                    };
                }

                return returnData;
            }
        }
        public async Task<ResponseViewModel> addTransferIncomeToDepositWallet(TransferIncomeToDepositWalletViewModel model)
        {
            var procedureName = Constant.transferIncomeToDepositWallet;
            var parameters = new DynamicParameters();

            parameters.Add("@URID", model.URID, DbType.Guid);
            parameters.Add("@TrnsAmount", model.trnsamount, DbType.Decimal);
            parameters.Add("@walletType", model.walletType, DbType.Int32);
            parameters.Add("@IntResult", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using (var connection = _dapperContext.createConnection())
            {
                try
                {
                    await connection.ExecuteAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
                    var intResult = parameters.Get<int>("@IntResult");

                    return intResult switch
                    {
                        > 0 => new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = "Fund transferred to Deposit Wallet successfully.",
                            data = intResult
                        },
                        -2 => new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.Conflict,
                            message = "Insufficient balance.",
                            data = null
                        },
                        _ => new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.ExpectationFailed,
                            message = "Failed to process the transfer request.",
                            data = null
                        }
                    };
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.InternalServerError,
                        message = $"Internal server error: {ex.Message}",
                        data = null
                    };
                }
            }
        }
        
        public async Task<ResponseViewModel> getIncomeToDepositWalletReport(Guid URID)
        {
            var balanceProc = Constant.spGetUser_WalletBalance;
            var reportProc = Constant.getIncomeToDepositWalletReport;

            var parameters = new DynamicParameters();
            parameters.Add("@URID", URID, DbType.Guid);

            using (var connection = _dapperContext.createConnection())
            {
                try
                {
                    var reportResult = await connection.QueryAsync(reportProc, parameters, commandType: CommandType.StoredProcedure);
                    var depositReportList = reportResult.ToList();

                    var walletResult = await connection.QueryAsync<WalletBalanceModel>(
                        balanceProc, parameters, commandType: CommandType.StoredProcedure);
                    var walletList = walletResult.ToList();

                    bool hasReport = depositReportList != null && depositReportList.Any();
                    bool hasWallet = walletList != null && walletList.Any();

                    if (hasReport || hasWallet)
                    {
                        var combinedData = new
                        {
                            DepositWalletReport = depositReportList,
                            walletBalance = walletList
                        };

                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = "Data fetched successfully",
                            data = combinedData
                        };
                    }
                    else
                    {
                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.NotFound,
                            message = "No data found",
                            data = null
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.InternalServerError,
                        message = "An error occurred while fetching data: " + ex.Message,
                        data = null
                    };
                }
            }
        }

 
        public async Task<ResponseViewModel> getAllFundRequestReport_Admin(AppUnAppFundRequestModel appUnAppFundRequestModel)
        {
            var balanceProc = Constant.getAllApprovedFundRequestReport_Admin;
            var reportProc = Constant.getAllUnAppFundRequestReport_Admin;

            var parameters = new DynamicParameters();
            parameters.Add("@AuthLogin", appUnAppFundRequestModel.AuthLogin, DbType.String);
            parameters.Add("@FromDate", appUnAppFundRequestModel.FromDate, DbType.String);
            parameters.Add("@ToDate", appUnAppFundRequestModel.ToDate, DbType.String);


            using (var connection = _dapperContext.createConnection())
            {
                try
                {
                    // Fetch All Fund Requests
                    var reportResult = await connection.QueryAsync(reportProc, parameters, commandType: CommandType.StoredProcedure);
                    var depositReportList = reportResult.ToList();

                    // Fetch Approved Fund Requests
                    var walletResultData = await connection.QueryAsync(balanceProc, parameters, commandType: CommandType.StoredProcedure);
                    var walletList = walletResultData.ToList();

                    bool hasReport = depositReportList != null && depositReportList.Any();
                    bool hasWallet = walletList != null && walletList.Any();

                    if (hasReport || hasWallet)
                    {
                        var combinedData = new
                        {
                            UnApproveFundRequest = depositReportList,
                            ApprovedFundRequest = walletList
                        };

                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = "Data fetched successfully",
                            data = combinedData
                        };
                    }
                    else
                    {
                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.NotFound,
                            message = "No data found",
                            data = null
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.InternalServerError,
                        message = "An error occurred while fetching data: " + ex.Message,
                        data = null
                    };
                }
            }
        }
        public async Task<ResponseViewModel> upIncWithdReqStatus_Admin(AppRejFundViewModel appRejFundViewModel)
        {
            var reportProc = Constant.upIncWithdReqStatus_Admin;
            var parameters = new DynamicParameters();
            parameters.Add("@AuthLoginId", appRejFundViewModel.AuthLoginId, DbType.String);
            parameters.Add("@Rfstatus", appRejFundViewModel.Rfstatus, DbType.Int32);
            parameters.Add("@Remark", appRejFundViewModel.Remark, DbType.String);
            parameters.Add("@Id", appRejFundViewModel.Id, DbType.Int32);

            using (var connection = _dapperContext.createConnection())
            {
                try
                {
                    var reportResult = await connection.QueryAsync(reportProc, parameters, commandType: CommandType.StoredProcedure);
                    var depositReportList = reportResult.ToList();
                    bool hasReport = depositReportList != null && depositReportList.Any();
                    var combinedData = new
                    {
                        updateFundRequest = depositReportList,
                    };

                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "Data fetched successfully",
                        data = combinedData
                    };
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.InternalServerError,
                        message = "An error occurred while fetching data: " + ex.Message,
                        data = null
                    };
                }
            }
        }

        public async Task<ResponseViewModel> updateFundRequestStatus_Admin(AppRejFundViewModel appRejFundViewModel)
        {
            var reportProc = Constant.updateFundRequestStatus_Admin;
            var parameters = new DynamicParameters();
            parameters.Add("@AuthLoginId", appRejFundViewModel.AuthLoginId, DbType.String);
            parameters.Add("@Rfstatus", appRejFundViewModel.Rfstatus, DbType.Int32);
            parameters.Add("@Remark", appRejFundViewModel.Remark, DbType.String);
            parameters.Add("@Id", appRejFundViewModel.Id, DbType.Int32);

            using (var connection = _dapperContext.createConnection())
            {
                try
                {
                    var reportResult = await connection.QueryAsync(reportProc, parameters, commandType: CommandType.StoredProcedure);
                    var depositReportList = reportResult.ToList();
                    bool hasReport = depositReportList != null && depositReportList.Any();                
                        var combinedData = new
                        {
                            updateFundRequest = depositReportList,
                        };

                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = "Data fetched successfully",
                            data = combinedData
                        };                   
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.InternalServerError,
                        message = "An error occurred while fetching data: " + ex.Message,
                        data = null
                    };
                }
            }
        }

        public async Task<ResponseViewModel> getAllUserWithdrawalRequest_Admin(AppUnAppIncomeVideoModel appUnAppIncomeVideoModel)
        {
            var UnApWithIncome = Constant.allUnApprIncWithdrawalHistory_Admin;
            var AprWithIncome = Constant.allApprIncWithdrawalHistory_Admin;

            var parameters = new DynamicParameters();
            parameters.Add("@AuthLogin", appUnAppIncomeVideoModel.AuthLogin, DbType.String);
            parameters.Add("@FromDate", appUnAppIncomeVideoModel.FromDate, DbType.String);
            parameters.Add("@ToDate", appUnAppIncomeVideoModel.ToDate, DbType.String);


            using (var connection = _dapperContext.createConnection())
            {
                try
                {
                    // Fetch All Fund Requests
                    var reportResult = await connection.QueryAsync(UnApWithIncome, parameters, commandType: CommandType.StoredProcedure);
                    var depositReportList = reportResult.ToList();

                    // Fetch Approved Fund Requests
                    var walletResultData = await connection.QueryAsync(AprWithIncome, parameters, commandType: CommandType.StoredProcedure);
                    var walletList = walletResultData.ToList();

                    bool hasReport = depositReportList != null && depositReportList.Any();
                    bool hasWallet = walletList != null && walletList.Any();

                    if (hasReport || hasWallet)
                    {
                        var combinedData = new
                        {
                            UnApWithIncome = depositReportList,
                            AprWithIncome = walletList
                        };

                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = "Data fetched successfully",
                            data = combinedData
                        };
                    }
                    else
                    {
                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.NotFound,
                            message = "No data found",
                            data = null
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.InternalServerError,
                        message = "An error occurred while fetching data: " + ex.Message,
                        data = null
                    };
                }
            }
        }
        public async Task<ResponseViewModel> getUserAutoDeposit(Guid URID)
        {
            var reportProc = Constant.getTokenDepositsByURID;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", URID, DbType.Guid);

            using (var connection = _dapperContext.createConnection())
            {
                try
                {
                    var reportResult = await connection.QueryAsync(reportProc, parameters, commandType: CommandType.StoredProcedure);
                    var depositReportList = reportResult.ToList();
                    bool hasReport = depositReportList != null && depositReportList.Any();
                    var combinedData = new
                    {
                        UserAutoDeposit = depositReportList,
                    };

                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "Data fetched successfully",
                        data = combinedData
                    };
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.InternalServerError,
                        message = "An error occurred while fetching data: " + ex.Message,
                        data = null
                    };
                }
            }
        }

        public async Task<ResponseViewModel> addAutoDeposit(TokenDepositsViewModel tokenDepositsViewModel)
        {
            var procedureName = Constant.addToken_Deposits;
            var parameters = new DynamicParameters();

            parameters.Add("@URID", tokenDepositsViewModel.URID, DbType.Guid);
            parameters.Add("@WalletAddress", tokenDepositsViewModel.WalletAddress ?? string.Empty, DbType.String);
            parameters.Add("@USDAmount", tokenDepositsViewModel.trnsamount, DbType.Decimal);
            parameters.Add("@TransHash", tokenDepositsViewModel.TransHash ?? string.Empty, DbType.String);
            parameters.Add("@IsActive", tokenDepositsViewModel.IsActive, DbType.Boolean);
            parameters.Add("@KID", tokenDepositsViewModel.KID, DbType.Int32);
            parameters.Add("@TransType", tokenDepositsViewModel.TransType ?? string.Empty, DbType.String);
            parameters.Add("@TokenRate", tokenDepositsViewModel.TokenRate, DbType.Decimal);
            parameters.Add("@TokenType", tokenDepositsViewModel.TokenType ?? string.Empty, DbType.String);
            parameters.Add("@TokenAmount", tokenDepositsViewModel.TokenAmount, DbType.Decimal);
            parameters.Add("@TokenId", tokenDepositsViewModel.TokenId ?? string.Empty, DbType.String);

            // Output parameter
            parameters.Add("@IntResult", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using (var connection = _dapperContext.createConnection())
            {
                try
                {
                    await connection.ExecuteAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
                    var intResult = parameters.Get<int>("@IntResult");

                    return intResult switch
                    {
                        > 0 => new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = "Auto deposit added successfully.",
                            data = intResult
                        },
                        -2 => new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.Conflict,
                            message = "Insufficient balance.",
                            data = null
                        },
                        _ => new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.ExpectationFailed,
                            message = "Failed to process the transfer request.",
                            data = null
                        }
                    };
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.InternalServerError,
                        message = $"Internal server error: {ex.Message}",
                        data = null
                    };
                }
            }
        }


        public async Task<ResponseViewModel> upRentWithdReqStatus_Admin(AppRejFundViewModel appRejFundViewModel)
        {
            var reportProc = Constant.upRentWithdReqStatus_Admin;
            var parameters = new DynamicParameters();
            parameters.Add("@AuthLoginId", appRejFundViewModel.AuthLoginId, DbType.String);
            parameters.Add("@Rfstatus", appRejFundViewModel.Rfstatus, DbType.Int32);
            parameters.Add("@Remark", appRejFundViewModel.Remark, DbType.String);
            parameters.Add("@Id", appRejFundViewModel.Id, DbType.Int32);

            using (var connection = _dapperContext.createConnection())
            {
                try
                {
                    var reportResult = await connection.QueryAsync(reportProc, parameters, commandType: CommandType.StoredProcedure);
                    var depositReportList = reportResult.ToList();
                    bool hasReport = depositReportList != null && depositReportList.Any();
                    var combinedData = new
                    {
                        updateFundRequest = depositReportList,
                    };

                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "Data fetched successfully",
                        data = combinedData
                    };
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.InternalServerError,
                        message = "An error occurred while fetching data: " + ex.Message,
                        data = null
                    };
                }
            }
        }

        //public async Task<ResponseViewModel> addRechargeTransaction(addRechargeTransactionViewModel addRechargeTransactionViewModel)
        //{
        //    var procedureName = Constant.SpAddRechargeTransaction;
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@URID", addRechargeTransactionViewModel.URID, DbType.Guid);
        //    parameters.Add("@Rkprice", addRechargeTransactionViewModel.Rkprice, DbType.Decimal);
        //    parameters.Add("@USDTValue", addRechargeTransactionViewModel.USDTValue, DbType.String);
        //    parameters.Add("@createdBy", addRechargeTransactionViewModel.createdBy, DbType.Guid);
        //    parameters.Add("@ByURID", addRechargeTransactionViewModel.ByURID, DbType.Guid);

        //    using (var connection = _dapperContext.createConnection())
        //    {
        //        var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModel>(
        //          procedureName, parameters, commandType: CommandType.StoredProcedure);

        //        if (result != null)
        //        {
        //            if (result.statusCode == 1)
        //            {
        //                result.statusCode = (int)HttpStatusCode.OK;
        //                result.message = result.message;
        //            }
        //            else if (result.statusCode == 0 || result.statusCode == 2)
        //            {
        //                result.statusCode = (int)HttpStatusCode.ExpectationFailed;
        //                result.message = result.message;
        //            }
        //            return result;
        //        }
        //        else
        //        {
        //            return new ResponseViewModel
        //            {
        //                statusCode = (int)HttpStatusCode.InternalServerError,
        //                message = "No response from stored procedure"
        //            };
        //        }

        //    }
        //}
        public async Task<ResponseViewModel> addRechargeTransaction(
            addRechargeTransactionViewModel model)
        {
            var procedureName = Constant.SpAddRechargeTransaction;

            var parameters = new DynamicParameters();
            parameters.Add("@URID", model.URID, DbType.Guid);
            parameters.Add("@Rkprice", model.Rkprice, DbType.Decimal);
            parameters.Add("@USDTValue", model.USDTValue, DbType.Int32); // 🔥 FIXED
            parameters.Add("@createdBy", model.createdBy, DbType.Guid);
            parameters.Add("@ByURID", model.ByURID, DbType.Guid);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModel>(
                    procedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                if (result == null)
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.InternalServerError,
                        message = "No response from stored procedure"
                    };
                }

                // Status mapping (clean & correct)
                result.statusCode = result.statusCode == 1
                    ? (int)HttpStatusCode.OK
                    : (int)HttpStatusCode.ExpectationFailed;

                return result;
            }
        }
        public async Task<ResponseViewModel> getspBindPackageUserSide()
        {
            var reportProc = Constant.bindPackageUserSide;

            using (var connection = _dapperContext.createConnection())
            {
                try
                {
                    var result = (await connection.QueryAsync(
                        reportProc,
                        commandType: CommandType.StoredProcedure
                    )).ToList();

                    if (result == null || !result.Any())
                    {
                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.NotFound,
                            message = "No package found",
                            data = null
                        };
                    }

                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "Data fetched successfully",
                        data = result
                    };
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.InternalServerError,
                        message = ex.Message,
                        data = null
                    };
                }
            }
        }

        //public async Task<ResponseViewModel> getspBindPackageUserSide()
        //{
        //    var reportProc = Constant.bindPackageUserSide;
        //    var parameters = new DynamicParameters();

        //    using (var connection = _dapperContext.createConnection())
        //    {
        //        try
        //        {
        //            var reportResult = await connection.QueryAsync(reportProc, parameters, commandType: CommandType.StoredProcedure);
        //            var depositReportList = reportResult.ToList();
        //            bool hasReport = depositReportList != null && depositReportList.Any();
        //            var combinedData = new
        //            {
        //                UserAutoDeposit = depositReportList,
        //            };

        //            return new ResponseViewModel
        //            {
        //                statusCode = (int)HttpStatusCode.OK,
        //                message = "Data fetched successfully",
        //                data = combinedData
        //            };
        //        }
        //        catch (Exception ex)
        //        {
        //            return new ResponseViewModel
        //            {
        //                statusCode = (int)HttpStatusCode.InternalServerError,
        //                message = "An error occurred while fetching data: " + ex.Message,
        //                data = null
        //            };
        //        }
        //    }
        //}
        public async Task<ResponseViewModel> getUserDormantReportDetails(Guid URID)
        {
            var procedureName1 = Constant.getDormantReport;
            var procedureName2 = Constant.spGetUser_WalletBalance;

            var parameters = new DynamicParameters();
            parameters.Add("@URID", URID, DbType.Guid);

            using (var connection = _dapperContext.createConnection())
            {
                var fundResult = await connection.QueryAsync<dynamic>(
                    procedureName1, parameters, commandType: CommandType.StoredProcedure);

                var walletResult = await connection.QueryAsync<WalletBalanceModel>(
                    procedureName2, parameters, commandType: CommandType.StoredProcedure);

                var fundList = fundResult?.ToList();


                var walletList = walletResult?.ToList();

                ResponseViewModel returnData;

                if (fundList != null && fundList.Any())
                {
                    var validation = fundList.First();

                    if (validation.statusCode == 1)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = validation.message,
                            data = new
                            {
                                Dormant = fundList,
                                WalletBalance = walletList
                            }
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
                        message = "Something went wrong with server error."
                    };
                }

                return returnData;
            }
        }
        public async Task<ResponseViewModel> getRechargeTransaction(Guid URID)
        {
            var procedureName1 = Constant.getRechargeTransaction;

            var parameters = new DynamicParameters();
            parameters.Add("@URID", URID, DbType.Guid);

            using (var connection = _dapperContext.createConnection())
            {
                var fundResult = await connection.QueryAsync<dynamic>(
                    procedureName1, parameters, commandType: CommandType.StoredProcedure);


                var fundList = fundResult?.ToList();



                ResponseViewModel returnData;

                if (fundList != null && fundList.Any())
                {
                    var validation = fundList.First();

                    if (validation.statusCode == 1)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = validation.message,
                            data = new
                            {
                                recharge = fundList,
                            }
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
                        message = "Something went wrong with server error."
                    };
                }

                return returnData;
            }
        }


        public async Task<ResponseViewModel> getAllUserROIWithdrawalRequest_Admin(AppUnAppIncomeVideoModel appUnAppIncomeVideoModel)
        {
            var UnApWithIncome = Constant.unApprROIWithdrawalHistory_Admin;
            var AprWithIncome = Constant.allApprROIWithdrawalHistory_Admin;

            var parameters = new DynamicParameters();
            parameters.Add("@AuthLogin", appUnAppIncomeVideoModel.AuthLogin, DbType.String);
            parameters.Add("@FromDate", appUnAppIncomeVideoModel.FromDate, DbType.String);
            parameters.Add("@ToDate", appUnAppIncomeVideoModel.ToDate, DbType.String);


            using (var connection = _dapperContext.createConnection())
            {
                try
                {
                    // Fetch All Fund Requests
                    var reportResult = await connection.QueryAsync(UnApWithIncome, parameters, commandType: CommandType.StoredProcedure);
                    var depositReportList = reportResult.ToList();

                    // Fetch Approved Fund Requests
                    var walletResultData = await connection.QueryAsync(AprWithIncome, parameters, commandType: CommandType.StoredProcedure);
                    var walletList = walletResultData.ToList();

                    bool hasReport = depositReportList != null && depositReportList.Any();
                    bool hasWallet = walletList != null && walletList.Any();

                    if (hasReport || hasWallet)
                    {
                        var combinedData = new
                        {
                            UnApWithROI = depositReportList,
                            AprWithROI = walletList
                        };

                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = "Data fetched successfully",
                            data = combinedData
                        };
                    }
                    else
                    {
                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.NotFound,
                            message = "No data found",
                            data = null
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.InternalServerError,
                        message = "An error occurred while fetching data: " + ex.Message,
                        data = null
                    };
                }
            }
        }

        public async Task<ResponseViewModel> upROIWithdReqStatus_Admin(AppRejFundViewModel appRejFundViewModel)
        {
            var reportProc = Constant.upROIWithdReqStatus_Admin;
            var parameters = new DynamicParameters();
            parameters.Add("@AuthLoginId", appRejFundViewModel.AuthLoginId, DbType.String);
            parameters.Add("@Rfstatus", appRejFundViewModel.Rfstatus, DbType.Int32);
            parameters.Add("@Remark", appRejFundViewModel.Remark, DbType.String);
            parameters.Add("@Id", appRejFundViewModel.Id, DbType.Int32);

            using (var connection = _dapperContext.createConnection())
            {
                try
                {
                    var reportResult = await connection.QueryAsync(reportProc, parameters, commandType: CommandType.StoredProcedure);
                    var depositReportList = reportResult.ToList();
                    bool hasReport = depositReportList != null && depositReportList.Any();
                    var combinedData = new
                    {
                        updateFundRequest = depositReportList,
                    };

                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "Data fetched successfully",
                        data = combinedData
                    };
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.InternalServerError,
                        message = "An error occurred while fetching data: " + ex.Message,
                        data = null
                    };
                }
            }
        }
    }
}

