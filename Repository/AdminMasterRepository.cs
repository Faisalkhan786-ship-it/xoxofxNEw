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
using Common;
using Azure;

namespace Repository
{
    public class AdminMasterRepository : IAdminMasterRepository
    {
        private readonly DapperContext _dapperContext;
        public AdminMasterRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }
       
        public async Task<ResponseViewModel> chanegAdminlvl(AdminChangelvlViewModel adminChangelvlViewModel)
        {
            var procedureName = Constant.updateLvl;
            var parameters = new DynamicParameters();
            parameters.Add("@authLogin", adminChangelvlViewModel.AuthLogin, DbType.String);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<SponsorUpdateResponse>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    var validation = result.First();
                    return new ResponseViewModel
                    {
                        statusCode = validation.statusCode == 1 ? (int)HttpStatusCode.OK : (int)HttpStatusCode.Conflict,
                        message = validation.message,
                        data = validation.statusCode == 1 ? result : null
                    };
                }
                else
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "Something went wrong with the server."
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
        public async Task<ResponseViewModel> chanegAdminPassword(AdminMasterViewModel adminMasterViewModel)
        {
            var procedureName = Constant.spUpdateAdminPassWord;
            var parameters = new DynamicParameters();
            parameters.Add("@username", adminMasterViewModel.username, DbType.String);
            parameters.Add("@OldPassword", adminMasterViewModel.OldPassword, DbType.String);
            parameters.Add("@NewPassword", adminMasterViewModel.NewPassword, DbType.String);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<PasswordUpdateResponse>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    var validation = result.First();
                    return new ResponseViewModel
                    {
                        statusCode = validation.statusCode == 1 ? (int)HttpStatusCode.OK : (int)HttpStatusCode.Conflict,
                        message = validation.message,
                        data = validation.statusCode == 1 ? result : null
                    };
                }
                else
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "Something went wrong with the server."
                    };
                }
            }
        }
        public class PasswordUpdateResponse
        {
            public int statusCode { get; set; }
            public string? message { get; set; }
        }
        public async Task<ResponseViewModel> chanegAdminSponsorID(AdminChangeSponsorIdViewModel AdminChangeSponsorIdViewModel)
        {
            var procedureName = Constant.spChangeSponsorID;
            var parameters = new DynamicParameters();
            parameters.Add("@authLogin", AdminChangeSponsorIdViewModel.AuthLogin, DbType.String);
            parameters.Add("@sponsorAuthLogin", AdminChangeSponsorIdViewModel.SponsorAuthLogin, DbType.String);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<SponsorUpdateResponse>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    var validation = result.First();
                    return new ResponseViewModel
                    {
                        statusCode = validation.statusCode == 1 ? (int)HttpStatusCode.OK : (int)HttpStatusCode.Conflict,
                        message = validation.message,
                        data = validation.statusCode == 1 ? result : null
                    };
                }
                else
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "Something went wrong with the server."
                    };
                }
            }
        }
        public class SponsorUpdateResponse
        {
            public int statusCode { get; set; }
            public string? message { get; set; }
        }


        public async Task<ResponseViewModel> blockUserByAdmin(string authLogin)
        {
            var procedureName = Constant.spBlockUserByAdmin;
            var parameters = new DynamicParameters();
            parameters.Add("@authLogin", authLogin, DbType.String);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<SponsorUpdateResponse>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    var validation = result.First();
                    return new ResponseViewModel
                    {
                        statusCode = validation.statusCode == 1 ? (int)HttpStatusCode.OK : (int)HttpStatusCode.Conflict,
                        message = validation.message,
                        data = validation.statusCode == 1 ? result : null
                    };
                }
                else
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "Something went wrong with the server."
                    };
                }
            }
        }

        public async Task<ResponseViewModel> userNameByLoginId(string authLogin)
        {
            var procedureName = Constant.spGetUserNameByLoginId;
            var parameters = new DynamicParameters();
            parameters.Add("@authLogin", authLogin, DbType.String);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<UserNameResponse>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (result != null)
                {
                    return new ResponseViewModel
                    {
                        statusCode = result.statusCode == 1 ? (int)HttpStatusCode.OK : (int)HttpStatusCode.Conflict,
                        message = result.message,
                        data = result.statusCode == 1 ? result : null
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

        public class UserNameResponse
        {
            public int statusCode { get; set; }
            public string? message { get; set; }
            public string? Name { get; set; }
            public string? FName { get; set; }
            public string? LName { get; set; }
            public string? Email { get; set; }
            public string? WalletBep20 { get; set; }
            public string? Address { get; set; }
            public string? Mobile { get; set; }
            public string? Country_Name { get; set; }
            public string? phonecode { get; set; }
            public Guid URID { get; set; }
            public int CountryId { get; set; }
            public string? ProfileImage { get; set; }
            public string? AuthPass { get; set; }
        }

        public async Task<ResponseViewModel> downloadExcel(AdminDownloadExcelViewModel adminDownloadExcelViewModel)
        {
            var procedureName = Constant.sPDownloadExcel;
            var parameters = new DynamicParameters();
            parameters.Add("@transtype", adminDownloadExcelViewModel.TransType, DbType.String);
            using (var connection = _dapperContext.createConnection())
            {
                if (adminDownloadExcelViewModel.TransType == "AllMember")
                {
                    var result = await connection.QueryAsync<AllMemberModel>(
                        procedureName, parameters, commandType: CommandType.StoredProcedure);

                    if (result != null && result.Any())
                    {
                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = "AllMember data fetched successfully.",
                            data = result
                        };
                    }
                }
                else
                {
                    // fallback to dynamic for other transtype
                    var result = await connection.QueryAsync<dynamic>(
                        procedureName, parameters, commandType: CommandType.StoredProcedure);

                    if (result != null && result.Any())
                    {
                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = "Data fetched successfully.",
                            data = result
                        };
                    }
                }

                return new ResponseViewModel
                {
                    statusCode = (int)HttpStatusCode.NotFound,
                    message = "No data found."
                };
            }
        }
        public class AllMemberModel
        {
            public string? AuthLogin { get; set; }
            public string? Name { get; set; }
            public string? Mobile { get; set; }
            public string? Email { get; set; }
            public string? Sponsor { get; set; }
            public string? RegDate { get; set; }
            public string? WalletAddress { get; set; }
            public string? Status { get; set; }
        }

        public async Task<ResponseViewModel> getEditNews(NewsViewModel model)
        {
            var procedureName = Constant.getEditNews;
            using (var connection = _dapperContext.createConnection())
            {
                var parameters = new DynamicParameters();

                // Convert NewsId to int if not null/empty
                if (!string.IsNullOrWhiteSpace(model.NewsId) && int.TryParse(model.NewsId, out int parsedId))
                    parameters.Add("@NewsId", parsedId, DbType.Int32);
                else
                    parameters.Add("@NewsId", DBNull.Value, DbType.Int32);

                using (var multi = await connection.QueryMultipleAsync(procedureName, parameters, commandType: CommandType.StoredProcedure))
                {
                    var status = await multi.ReadFirstOrDefaultAsync<ResponseViewModel>();
                    var dataList = multi.IsConsumed ? new List<FullNewsViewModel>() : multi.Read<FullNewsViewModel>().ToList();

                    status.data = dataList;
                    status.statusCode = status.statusCode == 1 ? (int)HttpStatusCode.OK : (int)HttpStatusCode.NotFound;
                    return status;
                }
            }
        }

        public class SettingViewModel
        {
            public int sID { get; set; }
            public string? caption { get; set; }
            public string? ClubDesc { get; set; }
            public decimal LIMITS { get; set; }
            public bool Active { get; set; }
            public int Editable { get; set; }
        }
        public class FullNewsViewModel
        {
            public int NewsId { get; set; }
            public string News { get; set; }
            public DateTime NewsDate { get; set; }
            public bool ActiveFlag { get; set; }
            public string Display { get; set; }
        }

        public async Task<ResponseViewModel> updateNews(UpdateViewModel updateViewModel)
        {
            var procedureName = Constant.updateNews;
            var parameters = new DynamicParameters();
            parameters.Add("@NewsId", updateViewModel.NewsId, DbType.String);
            parameters.Add("@News", updateViewModel.News, DbType.String);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<SponsorUpdateResponse>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    var validation = result.First();
                    return new ResponseViewModel
                    {
                        statusCode = validation.statusCode == 1 ? (int)HttpStatusCode.OK : (int)HttpStatusCode.Conflict,
                        message = validation.message,
                        data = validation.statusCode == 1 ? result : null
                    };
                }
                else
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "Something went wrong with the server."
                    };
                }
            }
        }

        public async Task<ResponseViewModel> getSettinDetails(SettinViewModel settinViewModel)
        {
            var procedureName = Constant.getSettings;
            using (var connection = _dapperContext.createConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@sID", settinViewModel.sID ?? 0, DbType.Int32); // default 0

                using (var multi = await connection.QueryMultipleAsync(procedureName, parameters, commandType: CommandType.StoredProcedure))
                {
                    var status = await multi.ReadFirstOrDefaultAsync<ResponseViewModel>();
                    var dataList = multi.IsConsumed ? new List<SettingViewModel>() : multi.Read<SettingViewModel>().ToList();

                    status.data = dataList;
                    status.statusCode = status.statusCode == 1 ? (int)HttpStatusCode.OK : (int)HttpStatusCode.NotFound;
                    return status;
                }
            }
        }

        public async Task<ResponseViewModel> updateSetting(UpdateSettingViewModel updateSettingViewModel)
        {
            var procedureName = Constant.updateSettings;
            var parameters = new DynamicParameters();
            parameters.Add("@SId", updateSettingViewModel.SId, DbType.Int32);
            parameters.Add("@Limits", updateSettingViewModel.Limits, DbType.Decimal);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<SponsorUpdateResponse>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    var validation = result.First();
                    return new ResponseViewModel
                    {
                        statusCode = validation.statusCode == 1 ? (int)HttpStatusCode.OK : (int)HttpStatusCode.Conflict,
                        message = validation.message,
                        data = null
                    };
                }
                else
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "Something went wrong with the server."
                    };
                }
            }
        }
        public async Task<ResponseViewModel> getLeaseAgent()
        {
            var procedureName = Constant.leaseAgent;
            var parameters = new DynamicParameters();         
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, commandType: CommandType.StoredProcedure);
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


        public async Task<ResponseViewModel> getGetLeaseStatement(LeaseStatementViewModel leaseStatementViewModel)
        {
            var procedureName = Constant.getLeaseStatement;
            var parameters = new DynamicParameters();
            parameters.Add("@AuthLogin", leaseStatementViewModel.AuthLogin ?? "", DbType.String);
            //parameters.Add("@productName", leaseStatementViewModel.productName ?? "", DbType.String);

            // Agar empty hai to NULL bhej do, SP default le legi
            parameters.Add("@FromDate", string.IsNullOrEmpty(leaseStatementViewModel.FromDate) ? null : leaseStatementViewModel.FromDate, DbType.String);
            parameters.Add("@ToDate", string.IsNullOrEmpty(leaseStatementViewModel.ToDate) ? null : leaseStatementViewModel.ToDate, DbType.String);

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
                        message = "No records found."
                    };
                }
                return returnData;
            }
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
            public decimal ROIWallet { get; set; }
            public int statusCode { get; set; }
            public string message { get; set; }
        }
        public class CombinedWalletResponseViewModel
        {
            public List<WalletType> FundTypes { get; set; }

            public List<FundTypeWiseCrDr> FundTypeWiseCrDrList { get; set; }
            public WalletDetails? WalletDetails { get; set; }
        }
    }
}


