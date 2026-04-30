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
using static ViewModel.GeographyViewModel;

namespace Repository
{
    public class GeographyRepository : IGeographyRepository
    {
        private readonly DapperContext _dapperContext;
        public GeographyRepository(DapperContext dapperContext) =>
            _dapperContext = dapperContext;
        public async Task<ResponseViewModel> getAllCountryMethod()
        {
            var procedureName = Constant.spGetAllCountry;
            var parameters = new DynamicParameters();

            try
            {
                using (var connection = _dapperContext.createConnection())
                {
                    var result = await connection.QueryAsync<CountryMethod>(procedureName, null, commandType: CommandType.StoredProcedure);

                    return new ResponseViewModel
                    {
                        statusCode = result.Any() ? (int)HttpStatusCode.OK : (int)HttpStatusCode.NotFound,
                        message = result.Any() ? "Country List Data Found" : "Data Not Found",
                        data = result
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseViewModel
                {
                    statusCode = (int)HttpStatusCode.InternalServerError,
                    message = "An error occurred while fetching country data.",
                };
            }
        }



        public async Task<ResponseViewModel> getAllStateMethod(int Fk_CountryId)
        {
            var procedureName = Constant.spGetAllState;
            var parameters = new DynamicParameters();
            parameters.Add("@Fk_CountryId", Fk_CountryId, DbType.Int32);

            try
            {
                using (var connection = _dapperContext.createConnection())
                {
                    var result = await connection.QueryAsync<StateMethod>(procedureName, parameters, commandType: CommandType.StoredProcedure);

                    return new ResponseViewModel
                    {
                        statusCode = result.Any() ? (int)HttpStatusCode.OK : (int)HttpStatusCode.NotFound,
                        message = result.Any() ? "Data Found" : "Data Not Found",
                        data = result
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseViewModel
                {
                    statusCode = (int)HttpStatusCode.InternalServerError,
                    message = "An error occurred while fetching state data.",
                    //errorDetails = ex.Message // Optional for debugging/logging
                };
            }
        }
        public async Task<ResponseViewModel> getAllCityMethod(int Fk_StateId)
        {
            var procedureName = Constant.spGetAllCity;
            var parameters = new DynamicParameters();
            parameters.Add("@Fk_StateId", Fk_StateId, DbType.Int32);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<City>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                var getbyCity = new ResponseViewModel
                {
                    statusCode = result.Count() == 0 ? (int)HttpStatusCode.NotFound : (int)HttpStatusCode.OK,
                    message = result.Count() == 0 ? "Data Not Found" : "Data Found",
                    data = result
                };
                return getbyCity;
            }
        }

        public async Task<ResponseViewModel> getAllContacUs()
        {
            var procedureName = Constant.getAllContacUs;
            var parameters = new DynamicParameters();

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<dynamic>(
                    procedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    var validation = result.FirstOrDefault();

                    int statusCode = validation?.statusCode ?? -1;
                    string message = validation?.message ?? "Unexpected error.";

                    return new ResponseViewModel
                    {
                        statusCode = statusCode switch
                        {
                            1 => (int)HttpStatusCode.OK,
                            0 => (int)HttpStatusCode.Conflict,
                            -1 => (int)HttpStatusCode.Conflict,
                            _ => (int)HttpStatusCode.BadRequest
                        },
                        message = message,
                        data = statusCode == 1 ? result : null
                    };
                }

                return new ResponseViewModel
                {
                    statusCode = (int)HttpStatusCode.NotFound,
                    message = "Something went wrong with the server."
                };
            }
        }

        public async Task<ResponseViewModel> getAllCareerType()
        {
            var procedureName = Constant.getCareerType;
            var parameters = new DynamicParameters();

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<dynamic>(
                    procedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    var validation = result.FirstOrDefault();

                    int statusCode = validation?.statusCode ?? -1;
                    string message = validation?.message ?? "Unexpected error.";

                    return new ResponseViewModel
                    {
                        statusCode = statusCode switch
                        {
                            1 => (int)HttpStatusCode.OK,
                            0 => (int)HttpStatusCode.Conflict,
                            -1 => (int)HttpStatusCode.Conflict,
                            _ => (int)HttpStatusCode.BadRequest
                        },
                        message = message,
                        data = statusCode == 1 ? result : null
                    };
                }

                return new ResponseViewModel
                {
                    statusCode = (int)HttpStatusCode.NotFound,
                    message = "Something went wrong with the server."
                };
            }
        }


        public async Task<ResponseViewModel> addContactUs(ContactUsViewModel contactUsViewModel)
        {
            var procedureName = Constant.addContactUs;

            var parameters = new DynamicParameters();
            parameters.Add("@Name", contactUsViewModel.Name, DbType.String);
            parameters.Add("@Email", contactUsViewModel.Email, DbType.String);
            parameters.Add("@Mobile", contactUsViewModel.Mobile, DbType.String);
            parameters.Add("@Subject", contactUsViewModel.Subject, DbType.String);
            parameters.Add("@Message", contactUsViewModel.Message, DbType.String); 
            parameters.Add("@CareerName", contactUsViewModel.CareerName, DbType.String); 

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<dynamic>(
                    procedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return new ResponseViewModel
                {
                    statusCode = result?.statuscode ?? 0,
                    message = result?.message ?? "Something went wrong."
                };
            }
        }



    }
}
