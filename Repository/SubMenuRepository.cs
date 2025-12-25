using Common;
using Dapper;
using RepositoryContract;
using System.Data;
using System.Net;
using ViewModel;
using static Model.ModelType;

namespace Repository
{
    public class SubMenuRepository : ISubMenuRepository
    {
        private readonly DapperContext _dapperContext;
        public SubMenuRepository(DapperContext dapperContext) =>
            _dapperContext = dapperContext;
        public async Task<ResponseViewModel> getByIdSubMenu(Guid id)
        {
            var procedureName = Constant.spGetByIdSubMenu;
            var parameters = new DynamicParameters();
            parameters.Add("@subMenuId", id, DbType.Guid);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<SubMenu>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                var getbyIdSubMenu = new ResponseViewModel
                {
                    statusCode = result.Count() == 0 ? (int)HttpStatusCode.NotFound : (int)HttpStatusCode.OK,
                    message = result.Count() == 0 ? "Data Not Found" : "Data Found",
                    data = result
                };
                return getbyIdSubMenu;
            }
        }
        public async Task<ResponseViewModel> getAllSubMenu()
        {
            var procedureName = Constant.spGetAllSubMenu;

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<SubMenu>(procedureName, null, commandType: CommandType.StoredProcedure);
                var getAllSubMenu = new ResponseViewModel
                {
                    statusCode = result.Count() == 0 ? (int)HttpStatusCode.NotFound : (int)HttpStatusCode.OK,
                    message = result.Count() == 0 ? "Data Not Found" : "Data Found",
                    data = result
                };
                return getAllSubMenu;
            }
        }
        public async Task<ResponseViewModel> addSubMenu(AddSubMenuViewModel addSubMenuViewModel)
        {
            var procedureName = Constant.spAddSubMenu;
            var parameters = new DynamicParameters();
            parameters.Add("@menuId", addSubMenuViewModel.menuId, DbType.Guid);
            parameters.Add("@subMenuName", addSubMenuViewModel.subMenuName, DbType.String);
            parameters.Add("@subMenuPageName", addSubMenuViewModel.subMenuPageName, DbType.String);
            parameters.Add("@displayOrder", addSubMenuViewModel.displayOrder, DbType.Int32);
            parameters.Add("@createdBy", addSubMenuViewModel.createdBy, DbType.Guid);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModel>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                if (result.statusCode == 1)
                {
                    result.statusCode = (int)HttpStatusCode.OK;
                    result.message = result.message;
                    var createdSubMenu = new
                    {
                        id = (Guid)result.data,
                        subMenuName = addSubMenuViewModel.subMenuName
                    };
                    result.data = createdSubMenu;
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
        public async Task<ResponseViewModel> updateSubMenu(UpdateSubMenuViewModel updateSubMenuViewModel)
        {
            var procedureName = Constant.spUpdateSubMenu;

            var parameters = new DynamicParameters();
            parameters.Add("@subMenuId", updateSubMenuViewModel.subMenuId, DbType.Guid);
            parameters.Add("@menuId", updateSubMenuViewModel.menuId, DbType.Guid);
            parameters.Add("@subMenuName", updateSubMenuViewModel.subMenuName, DbType.String);
            parameters.Add("@subMenuPageName", updateSubMenuViewModel.subMenuPageName, DbType.String);
            parameters.Add("@displayOrder", updateSubMenuViewModel.displayOrder, DbType.Int32);
            parameters.Add("@updatedBy", updateSubMenuViewModel.updatedBy, DbType.Guid);
            parameters.Add("@active", updateSubMenuViewModel.active, DbType.Boolean);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModel>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                if (result.statusCode == 1)
                {
                    result.statusCode = (int)HttpStatusCode.OK; // 200
                }
                else
                {
                    result.statusCode = (int)HttpStatusCode.ExpectationFailed; // 417
                }

                return result;
            }
        }
        public async Task<ResponseViewModel> deleteSubMenu(DeleteSubMenuViewModel deleteSubMenuViewModel)
        {
            var procedureName = Constant.spDeleteSubMenu;
            var parameters = new DynamicParameters();
            parameters.Add("@subMenuId", deleteSubMenuViewModel.subMenuId, DbType.Guid);
            parameters.Add("@updatedBy", deleteSubMenuViewModel.updatedBy, DbType.Guid);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModel>(procedureName, parameters, commandType: CommandType.StoredProcedure);     
                if (result.statusCode == 1)
                {
                    result.statusCode = (int)HttpStatusCode.OK; // 200
                }
                else
                {
                    result.statusCode = (int)HttpStatusCode.ExpectationFailed; // 417
                }
                return result;
            }
        }
        public async Task<ResponseViewModel> getSubMenubyMenuId(Guid menuId)
        {
            var procedureName = Constant.spGetSubMenubyMenuId;
            var parameters = new DynamicParameters();
            parameters.Add("@menuId", menuId, DbType.Guid);

            try
            {
                using (var connection = _dapperContext.createConnection())
                {
                    var result = await connection.QueryAsync<SubMenubyid>(procedureName, parameters, commandType: CommandType.StoredProcedure);

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
                    message = "An error occurred while fetching the data.",
                    data = null
                };
            }
        }
    }
}
