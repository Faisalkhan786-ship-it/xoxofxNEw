using Dapper;
using Common;
using RepositoryContract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace Repository
{
    public class AdminManageRepository: IAdminManageRepository
    {
        private readonly DapperContext _dapperContext;
        public AdminManageRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }
        //public async Task<ResponseViewModel> adminSearchAllUsers(AdminManageViewModel adminManageViewModel)
        //{
        //    var procedureName = Constant.searchAllUsers;
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@AuthLogin", adminManageViewModel.AuthLogin, DbType.String);
        //    parameters.Add("@Fname", adminManageViewModel.Fname, DbType.String);
        //    parameters.Add("@Active", adminManageViewModel.Active, DbType.String);
        //    parameters.Add("@Mobile", adminManageViewModel.Mobile, DbType.String);
        //    parameters.Add("@Email", adminManageViewModel.Email, DbType.String);
        //    parameters.Add("@Kid", adminManageViewModel.Kid, DbType.String);
        //    parameters.Add("@Walletid", adminManageViewModel.Walletid, DbType.String);
        //    parameters.Add("@FromDate", adminManageViewModel.FromDate, DbType.String);
        //    parameters.Add("@ToDate", adminManageViewModel.ToDate, DbType.String);

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

        public async Task<ResponseViewModel> adminSearchAllUsers(AdminManageViewModel adminManageViewModel)
        {
            var procedureName = Constant.searchAllUsers;
            var parameters = new DynamicParameters();

            parameters.Add("@AuthLogin", adminManageViewModel.AuthLogin ?? "", DbType.String);
            parameters.Add("@Fname", adminManageViewModel.Fname ?? "", DbType.String);
            parameters.Add("@Active", adminManageViewModel.Active ?? "", DbType.String);
            parameters.Add("@Mobile", adminManageViewModel.Mobile ?? "", DbType.String);
            parameters.Add("@Email", adminManageViewModel.Email ?? "", DbType.String);

            // Kid int hai, isliye string ki jagah int bhejna
            parameters.Add("@Kid", string.IsNullOrEmpty(adminManageViewModel.Kid) ? 0 : Convert.ToInt32(adminManageViewModel.Kid), DbType.Int32);

            parameters.Add("@Walletid", adminManageViewModel.Walletid ?? "", DbType.String);

            // Date agar empty hai to NULL bhejna
            parameters.Add("@FromDate", string.IsNullOrEmpty(adminManageViewModel.FromDate) ? null : adminManageViewModel.FromDate, DbType.String);
            parameters.Add("@ToDate", string.IsNullOrEmpty(adminManageViewModel.ToDate) ? null : adminManageViewModel.ToDate, DbType.String);

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
                    else if (validation.statusCode == 0 || validation.statusCode == -1)
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

        public async Task<ResponseViewModel> getRentWallet(AppUnApprentViewModel appUnApprentViewModel)
        {
            var UnApWithIncome = Constant.allUnApprRentWalletWithdrawal;
            var AprWithIncome = Constant.allApprRentWallet;

            var parameters = new DynamicParameters();
            parameters.Add("@AuthLogin", appUnApprentViewModel.AuthLogin, DbType.String);
            parameters.Add("@FromDate", appUnApprentViewModel.FromDate, DbType.String);
            parameters.Add("@ToDate", appUnApprentViewModel.ToDate, DbType.String);


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
                            UnApWithrentwallet = depositReportList,
                            AprWithrentwallet = walletList
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

        //public async Task<ResponseViewModel> getRentWallet()
        //{
        //    var incomeProc = Constant.getAllApprRentWallet;

        //    var parameters = new DynamicParameters();

        //    using (var connection = _dapperContext.createConnection())
        //    {
        //        try
        //        {
        //            var incomeResult = await connection.QueryAsync(incomeProc, commandType: CommandType.StoredProcedure);
        //            var incomeList = incomeResult.ToList();


        //            if ((incomeList != null && incomeList.Any()))
        //            {
        //                var message = "Data fetched successfully";
        //                var combinedData = new
        //                {
        //                    RentWallet = incomeList,
        //                };

        //                return new ResponseViewModel
        //                {
        //                    statusCode = (int)HttpStatusCode.OK,
        //                    message = message,
        //                    data = combinedData
        //                };
        //            }
        //            else
        //            {
        //                return new ResponseViewModel
        //                {
        //                    statusCode = (int)HttpStatusCode.NotFound,
        //                    message = "No transaction types found.",
        //                    data = null
        //                };
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return new ResponseViewModel
        //            {
        //                statusCode = (int)HttpStatusCode.InternalServerError,
        //                message = $"Error occurred: {ex.Message}",
        //                data = null
        //            };
        //        }
        //    }
        //}
    }
}
