using Common;
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
using static Model.ModelType;

namespace Repository
{
    public class AdminManageFundRepository: IAdminManageFundRepository
    {
        private readonly DapperContext _dapperContext;
        public AdminManageFundRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }
        public class CombinedWalletResponseViewModel
        {
            public List<WalletType> FundTypes { get; set; }

            public List<FundTypeWiseCrDr> FundTypeWiseCrDrList { get; set; }
            public WalletDetails? WalletDetails { get; set; }
        }

        public async Task<ResponseViewModel> getUserWalletDetailsF(string loginId)
        {
            var fundTypeProcedure = Constant.fundType;
            var fundTypeWiseCrDrProcedure = Constant.fundTypeWiseCrDr;
            //var walletDetailsProcedure = Constant.spGetUserWalletDetails;
            var walletDetailsProcedure = Constant.getUser_WalletDetails;

            var combinedData = new CombinedWalletResponseViewModel();

            using (var connection = _dapperContext.createConnection())
            {
                // Get FundTypes
                using (var multi1 = await connection.QueryMultipleAsync(fundTypeProcedure, commandType: CommandType.StoredProcedure))
                {
                    combinedData.FundTypes = (await multi1.ReadAsync<WalletType>()).ToList();
                    var status1 = await multi1.ReadFirstOrDefaultAsync<dynamic>();
                    if ((status1?.statusCode ?? -1) != 1)
                    {
                        return new ResponseViewModel { statusCode = 401, message = "Failed to load Fund Types", data = null };
                    }
                }


                //Get FundTypeWiseCrDr
                var parameters2 = new DynamicParameters();
                using (var multi2 = await connection.QueryMultipleAsync(fundTypeWiseCrDrProcedure, parameters2, commandType: CommandType.StoredProcedure))
                {
                    combinedData.FundTypeWiseCrDrList = (await multi2.ReadAsync<FundTypeWiseCrDr>()).ToList();
                    var status2 = await multi2.ReadFirstOrDefaultAsync<dynamic>();
                    if ((status2?.statusCode ?? -1) != 1)
                    {
                        return new ResponseViewModel { statusCode = 401, message = "Failed to load Fund Type Wise CrDr", data = null };
                    }
                }


                //// Get WalletDetails
                //var parameters3 = new DynamicParameters();
                //parameters3.Add("@AuthLogin", loginId, DbType.String);
                //using (var multi3 = await connection.QueryMultipleAsync(walletDetailsProcedure, parameters3, commandType: CommandType.StoredProcedure))
                //{
                //    combinedData.WalletDetails = (await multi3.ReadAsync<WalletDetails>()).FirstOrDefault();
                //    var status3 = await multi3.ReadFirstOrDefaultAsync<dynamic>();
                //    if ((status3?.statusCode ?? -1) != 1)
                //    {
                //        return new ResponseViewModel { statusCode = 401, message = "Failed to load Wallet Details", data = null };
                //    }
                //}
                // Get WalletDetails
                var parameters3 = new DynamicParameters();
                parameters3.Add("@AuthLogin", loginId, DbType.String);

                var result = await connection.QueryFirstOrDefaultAsync<WalletDetails>(
                    walletDetailsProcedure,
                    parameters3,
                    commandType: CommandType.StoredProcedure);

                if (result == null || result.statusCode != 1)
                {
                    return new ResponseViewModel { statusCode = 401, message = "Failed to load Wallet Details", data = null };
                }

                combinedData.WalletDetails = result;


            }

            return new ResponseViewModel
            {
                statusCode = 200,
                message = "Success",
                data = combinedData
            };
        }
        public class WalletDetails
        {
            public Guid URID { get; set; }
            public string Name { get; set; }
            public decimal IncomeWallet { get; set; }
            public decimal DepositWallet { get; set; }
            public decimal RentWallet { get; set; }
            public int statusCode { get; set; }
            public string message { get; set; }
        }

        public async Task<ResponseViewModel> getUserWalletDetails(string LoginId)
        {
            var procedureName = Constant.spGetUserWalletDetails;
            var parameters = new DynamicParameters();
            parameters.Add("@LoginId", LoginId, DbType.String);

            using (var connection = _dapperContext.createConnection())
            {
                using (var multi = await connection.QueryMultipleAsync(procedureName, parameters, commandType: CommandType.StoredProcedure))
                {
                    var data = (await multi.ReadAsync<WalletDetails>()).FirstOrDefault();
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

        public async Task<ResponseViewModel> addCreditAndDebitFund(AdminManageFundViewModel adminManageFundViewModel)
        {
            var procedureName = Constant.spFundFromAdmin;
            var parameters = new DynamicParameters();
            parameters.Add("@Wallettype", adminManageFundViewModel.Wallettype, DbType.Int32);
            parameters.Add("@CrDr", adminManageFundViewModel.CrDr, DbType.Int32);
            parameters.Add("@URID", adminManageFundViewModel.URID, DbType.Guid);
            parameters.Add("@Amt", adminManageFundViewModel.Amt, DbType.Decimal);
            parameters.Add("@Remark", adminManageFundViewModel.Remark, DbType.String);
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

        public async Task<ResponseViewModel> getFundType()
        {
            var procedureName = Constant.fundType;
            var parameters = new DynamicParameters();

            using (var connection = _dapperContext.createConnection())
            {
                using (var multi = await connection.QueryMultipleAsync(procedureName, parameters, commandType: CommandType.StoredProcedure))
                {
                    var data = (await multi.ReadAsync<WalletType>()).ToList();  // ✅ Fix here
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

        public async Task<ResponseViewModel> getFundTypeWiseCrDr(int WalletId)
        {
            var procedureName = Constant.fundTypeWiseCrDr;
            var parameters = new DynamicParameters();
            parameters.Add("@WalletId", WalletId, DbType.Int32);

            using (var connection = _dapperContext.createConnection())
            {
                using (var multi = await connection.QueryMultipleAsync(procedureName, parameters, commandType: CommandType.StoredProcedure))
                {
                    var data = (await multi.ReadAsync<FundTypeWiseCrDr>()).ToList(); 
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
        public class WalletHistoryDto
        {
            public int statusCode { get; set; }
            public string message { get; set; }
            public string AuthLogin { get; set; }
            public decimal credit { get; set; }
            public string ApprovedDate { get; set; }
            public string Remark { get; set; }
            public int trStatus { get; set; }
            public string Status { get; set; }
        }

        public async Task<ResponseViewModel> allWalletHistory(AllWalletHistoryViewModel model)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@URID", model.URID, DbType.Guid);
            parameters.Add("@WalletType", model.WalletType, DbType.String);

            using var connection = _dapperContext.createConnection();

            var result = (await connection.QueryAsync<WalletHistoryDto>(
                Constant.SpGetAllWalletHistory,
                parameters,
                commandType: CommandType.StoredProcedure)).ToList();

            // ❌ No data
            if (!result.Any())
            {
                return new ResponseViewModel
                {
                    statusCode = (int)HttpStatusCode.NotFound,
                    message = "No wallet history found"
                };
            }

            var firstRow = result.First();

            // ❌ SP validation failed
            if (firstRow.statusCode != 1)
            {
                return new ResponseViewModel
                {
                    statusCode = (int)HttpStatusCode.Conflict,
                    message = firstRow.message
                };
            }

            // ✅ Success
            return new ResponseViewModel
            {
                statusCode = (int)HttpStatusCode.OK,
                message = firstRow.message,
                data = result
            };
        }

        //public async Task<ResponseViewModel> allWalletHistory(AllWalletHistoryViewModel allWalletHistoryViewModel)
        //{
        //    var procedureName = Constant.SpGetAllWalletHistory;
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@URID", allWalletHistoryViewModel.URID, DbType.Guid);
        //    parameters.Add("@WalletType", allWalletHistoryViewModel.WalletType, DbType.String);
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

    }
}
