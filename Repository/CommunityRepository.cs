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
using static Model.ModelType;

namespace Repository
{
    public class CommunityRepository: ICommunityRepository
    {
        private readonly DapperContext _dapperContext;
        public CommunityRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<ResponseViewModel> GetDirectMemberDetails(DirectMemberViewModel directMemberViewModel)
        {
            var procedureName = Constant.directMemberSearch;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", directMemberViewModel.URID, DbType.Guid);
            parameters.Add("@Statusid", directMemberViewModel.StatusId, DbType.String);
            parameters.Add("@Loginid", directMemberViewModel.Loginid, DbType.String);         
            try
            {
                using (var connection = _dapperContext.createConnection())
                {
                    var result = await connection.QueryAsync<DirectMember>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                    var response = new ResponseViewModel
                    {
                        statusCode = result.Any() ? (int)HttpStatusCode.OK : (int)HttpStatusCode.NotFound,
                        message = result.Any() ? "Data Found" : "Data Not Found",
                        data = result
                    };
                    return response;
                }
            }
            catch (Exception ex)
            {
                return new ResponseViewModel
                {
                    statusCode = (int)HttpStatusCode.InternalServerError,
                    message = "An error occurred while fetching data.",
                    data = null,
                };
            }
        }

        public async Task<ResponseViewModel> GetPersonalTeam(PersonalTeamViewModel PersonalTeamViewModel)
        {
            var procedureName = Constant.spGetPersonalTeamList_Search;
            var parameters = new DynamicParameters();
            parameters.Add("@AuthLogin", PersonalTeamViewModel.AuthLogin, DbType.String);
            parameters.Add("@lvl", PersonalTeamViewModel.lvl, DbType.String);
            parameters.Add("@statusid", PersonalTeamViewModel.statusId, DbType.String);
            try
            {
                using (var connection = _dapperContext.createConnection())
                {
                    var result = await connection.QueryAsync<PersonalTeam>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                    var response = new ResponseViewModel
                    {
                        statusCode = result.Any() ? (int)HttpStatusCode.OK : (int)HttpStatusCode.NotFound,
                        message = result.Any() ? "Data Found" : "Data Not Found",
                        data = result
                    };
                    return response;
                }
            }
            catch (Exception ex)
            {
                return new ResponseViewModel
                {
                    statusCode = (int)HttpStatusCode.InternalServerError,
                    message = "An error occurred while fetching data.",
                    data = null,
                };
            }
        }
        public async Task<ResponseViewModel> getPersonalTeamList(PersonalTeamReportViewModel model)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@AuthLogin", model.AuthLogin);
                parameters.Add("@lvl", model.lvl ?? "");
                parameters.Add("@statusid", model.statusid ?? "");

                using (var connection = _dapperContext.createConnection())
                {
                    var result = (await connection.QueryAsync<PersonalTeamResultModel>(
                        Constant.getPersonalTeamList_Search,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    )).ToList();

                    if (!result.Any())
                    {
                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.NotFound,
                            message = "Data not found"
                        };
                    }

                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "Data found",
                        data = result
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseViewModel
                {
                    statusCode = (int)HttpStatusCode.InternalServerError,
                    message = ex.Message
                };
            }
        }

        //public async Task<ResponseViewModel> getPersonalTeamList(PersonalTeamReportViewModel personalTeamReportViewModel)
        //{
        //    var procedureName = Constant.getPersonalTeamList_Search;
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@AuthLogin", personalTeamReportViewModel.AuthLogin, DbType.String);
        //    parameters.Add("@uRank", personalTeamReportViewModel.uRank, DbType.String);
        //    parameters.Add("@lvl", personalTeamReportViewModel.lvl, DbType.String);
        //    parameters.Add("@statusid", personalTeamReportViewModel.statusid, DbType.String);

        //    using (var connection = _dapperContext.createConnection())
        //    {
        //        var result = await connection.QueryAsync<PersonalTeamResultModel>(
        //            procedureName,
        //            parameters,
        //            commandType: CommandType.StoredProcedure
        //        );

        //        ResponseViewModel returnData;

        //        if (result != null && result.Any())
        //        {
        //            var validation = result.FirstOrDefault();

        //            if (validation != null)
        //            {
        //                if (validation.statusCode == 1)
        //                {
        //                    returnData = new ResponseViewModel
        //                    {
        //                        statusCode = (int)HttpStatusCode.OK,
        //                        message = "Data found",
        //                        data = result
        //                    };
        //                }
        //                else if (validation.statusCode == 0)
        //                {
        //                    returnData = new ResponseViewModel
        //                    {
        //                        statusCode = (int)HttpStatusCode.NotFound,
        //                        message = "Wrong ID"
        //                    };
        //                }
        //                else
        //                {
        //                    returnData = new ResponseViewModel
        //                    {
        //                        statusCode = (int)HttpStatusCode.BadRequest,
        //                        message = "Unexpected error"
        //                    };
        //                }
        //            }
        //            else
        //            {
        //                returnData = new ResponseViewModel
        //                {
        //                    statusCode = (int)HttpStatusCode.NotFound,
        //                    message = "Data not found"
        //                };
        //            }
        //        }
        //        else
        //        {
        //            returnData = new ResponseViewModel
        //            {
        //                statusCode = (int)HttpStatusCode.NotFound,
        //                message = "Data not found"
        //            };
        //        }

        //        return returnData;
        //    }
        //}
        public class PersonalTeamResultModel
        {
            public int Id { get; set; }
            public string? Loginid { get; set; }
            public string? SponsorId { get; set; }
            public string? Name { get; set; }
            public string? RegDate { get; set; }
            public string? Email { get; set; }
            public string? Mobile { get; set; }
            public string? TopupDate { get; set; }
            public int TopupValue { get; set; }
            public decimal TeamBusiness { get; set; }
            public int uLvl { get; set; }
            public string? Urank { get; set; }
            public int kid { get; set; }
            public string? status { get; set; }
            public int statusCode { get; set; }
            public string? message { get; set; }
            public string? LeaseAmount { get; set; }
            public decimal TotTeam { get; set; }
            public int ActiveTeam { get; set; }
            public decimal MonthlySelf { get; set; }
            public decimal MonthlyTeam { get; set; }
        }
        public async Task<ResponseViewModel> getAgentLeaseCredit(Guid urid)
        {
            var procedureName = Constant.agentLeaseCredit;

            using (var connection = _dapperContext.createConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@URID", urid);   // <<-- Required Parameter

                var result = await connection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    var combinedData = new
                    {
                        AgentLeaseCredit = result.ToList(),
                    };

                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "Get Agent Lease Credit.",
                        data = combinedData
                    };
                }
                else
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "No Credit data found."
                    };
                }
            }
        }

        public async Task<ResponseViewModel> getdownLineTreeDetails(Guid URID)
        {
            var incomeProc = Constant.downLineTree_Details_fourlvl;

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
                            downLineTreeDetails = incomeList,
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
                            message = "No down Line Tree Details types found.",
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
        public async Task<ResponseViewModel> getDownlineLeftRightCount(DownlineLeftRightCountViewModel downlineLeftRightCountViewModel)
        {
            var incomeProc = Constant.downlineLeftRightCount;
            var parameters = new DynamicParameters();

            parameters.Add("@mURID", downlineLeftRightCountViewModel.URID, DbType.Guid);
            parameters.Add("@side", downlineLeftRightCountViewModel.side, DbType.String);
            parameters.Add("@totcount", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using (var connection = _dapperContext.createConnection())
            {
                try
                {
                    await connection.ExecuteAsync(
                        incomeProc,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    var totalCount = parameters.Get<int>("@totcount");

                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "Data fetched successfully",
                        data = new
                        {
                            totalCount = totalCount
                        }
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
        public async Task<ResponseViewModel> getLeftRightdownline(LeftRightdownlineTeamViewModel leftRightdownlineTeamViewModel)
        {
            var incomeProc = Constant.leftRightdownlineTeam;
            var parameters = new DynamicParameters();
            parameters.Add("@mUrid", leftRightdownlineTeamViewModel.Urid, DbType.Guid);
            parameters.Add("@side", leftRightdownlineTeamViewModel.side, DbType.String);
            parameters.Add("@kid", leftRightdownlineTeamViewModel.kid, DbType.Int32);
            parameters.Add("@dt_from", leftRightdownlineTeamViewModel.fromdate, DbType.String);
            parameters.Add("@dt_to", leftRightdownlineTeamViewModel.toDate, DbType.String);

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
                            leftRightdownline = incomeList,
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
                            message = "No left Right down line.",
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
    }
}
