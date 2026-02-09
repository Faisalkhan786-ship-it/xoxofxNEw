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
    
        public async Task<ResponseViewModel> adminSearchAllUsers(AdminManageViewModel adminManageViewModel)
        {
            var procedureName = Constant.searchAllUsers;
            var parameters = new DynamicParameters();
            parameters.Add("@Fullname", adminManageViewModel.Fullname ?? "", DbType.String);
            parameters.Add("@Active", adminManageViewModel.Active ?? "", DbType.String);
            parameters.Add("@PhoneNo", adminManageViewModel.PhoneNo ?? "", DbType.String);
            parameters.Add("@Email", adminManageViewModel.Email ?? "", DbType.String);
            parameters.Add("@FromDate", string.IsNullOrEmpty(adminManageViewModel.FromDate) ? null : adminManageViewModel.FromDate);
            parameters.Add("@ToDate", string.IsNullOrEmpty(adminManageViewModel.ToDate) ? null : adminManageViewModel.ToDate);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<dynamic>(procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (result == null || !result.Any())
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "No data found."
                    };
                }

                // Take first row for statusCode & message
                var firstRow = result.First();

                int status = firstRow.statusCode;

                if (status == 1)
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = firstRow.message,
                        data = result
                    };
                }
                else
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.BadRequest,
                        message = firstRow.message
                    };
                }
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

    }
}
