using Dapper;
using RepositoryContract;
using System.Data;
using System.Net;
using ViewModel;
using Common;
using static Model.ModelType;

namespace Repository
{
    public class SubCategoryTypeRepository : ISubCategoryTypeRepository
    {
        private readonly DapperContext _dapperContext;
        public SubCategoryTypeRepository(DapperContext dapperContext) =>
            _dapperContext = dapperContext;
        public async Task<ResponseViewModel> getByIdSubCategoryType(Guid subCategoryTypeId)
        {
            var procedureName = Constant.spGetByIdSubCategoryType;
            var parameters = new DynamicParameters();
            parameters.Add("@subCategoryTypeId", subCategoryTypeId, DbType.Guid);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<SubCategoryType>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                var getbyIdSubCategoryType = new ResponseViewModel
                {
                    statusCode = result.Count() == 0 ? (int)HttpStatusCode.NotFound : (int)HttpStatusCode.OK,
                    message = result.Count() == 0 ? "Data Not Found" : "Data Found",
                    data = result
                };
                return getbyIdSubCategoryType;
            }
        }

        public async Task<ResponseViewModel> getAllSubCategoryType()
        {
            var procedureName = Constant.spGetAllSubCategoryType;

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<SubCategoryType>(procedureName, null, commandType: CommandType.StoredProcedure);
                var getAllSubCategoryType = new ResponseViewModel
                {
                    statusCode = result.Count() == 0 ? (int)HttpStatusCode.NotFound : (int)HttpStatusCode.OK,
                    message = result.Count() == 0 ? "Data Not Found" : "Data Found",
                    data = result
                };
                return getAllSubCategoryType;
            }
        }

        public async Task<ResponseViewModel> getAllSubCategoryTypeForUser()
        {
            var procedureName = Constant.spGetAllSubCategoryTypeForUser;

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<SubCategoryTypeForUser>(procedureName, null, commandType: CommandType.StoredProcedure);
                var getAllSubCategoryType = new ResponseViewModel
                {
                    statusCode = result.Count() == 0 ? (int)HttpStatusCode.NotFound : (int)HttpStatusCode.OK,
                    message = result.Count() == 0 ? "Data Not Found" : "Data Found",
                    data = result
                };
                return getAllSubCategoryType;
            }
        }

        public async Task<ResponseViewModel> addSubCategoryType(AddSubCategoryTypeViewModel addSubCategoryType)
        {
            var procedureName = Constant.spAddSubCategoryType;
            var parameters = new DynamicParameters();
            parameters.Add("@categoryId", addSubCategoryType.categoryId, DbType.Guid);
            parameters.Add("@subCategoryId", addSubCategoryType.subCategoryId, DbType.Guid);
            parameters.Add("@name", addSubCategoryType.name, DbType.String);
            parameters.Add("@createdBy", addSubCategoryType.createdBy, DbType.Guid);

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
        public async Task<ResponseViewModel> updateSubCategoryType(UpdateSubCategoryTypeViewModel updateSubCategoryType)
        {
            var procedureName = Constant.spUpdateSubCategoryType;

            var parameters = new DynamicParameters();
            parameters.Add("@subCategoryTypeId", updateSubCategoryType.subCategoryTypeId, DbType.Guid);
            parameters.Add("@name", updateSubCategoryType.name, DbType.String);
            parameters.Add("@active", updateSubCategoryType.active ? 1 : 0, DbType.Boolean);
            parameters.Add("@updatedBy", updateSubCategoryType.updatedBy, DbType.Guid);
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
                    result.statusCode = (int)HttpStatusCode.OK;
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
        public async Task<ResponseViewModel> deleteSubCategoryType(DeleteSubCategoryTypeViewModel deleteSubCategoryType)
        {
            var procedureName = Constant.spDeleteSubCategoryType;
            var parameters = new DynamicParameters();
            parameters.Add("@subCategoryTypeId", deleteSubCategoryType.subCategoryTypeId, DbType.Guid);
            parameters.Add("@updatedBy", deleteSubCategoryType.updatedBy, DbType.Guid);
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
                    result.statusCode = (int)HttpStatusCode.OK;
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
    }
}
