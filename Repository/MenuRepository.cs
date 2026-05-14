using Common;
using Dapper;
using RepositoryContract;
using System.Data;
using System.Net;
using ViewModel;
using static Model.ModelType;

namespace Repository
{
    public class MenuRepository : IMenuRepository
    {
        private readonly DapperContext _dapperContext;
        public MenuRepository(DapperContext dapperContext) =>
            _dapperContext = dapperContext;
        public async Task<ResponseViewModel> getByIdMenu(Guid id)
        {
            var procedureName = Constant.spGetByIdMenu;
            var parameters = new DynamicParameters();
            parameters.Add("@menuId", id, DbType.Guid);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<Menu>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                var getbyIdMenu = new ResponseViewModel
                {
                    statusCode = result.Count() == 0 ? (int)HttpStatusCode.NotFound : (int)HttpStatusCode.OK,
                    message = result.Count() == 0 ? "Data Not Found" : "Data Found",
                    data = result
                };
                return getbyIdMenu;
            }
        }

        //ye main h jo use ho rahi h menu and submenu me 
        public async Task<ResponseViewModel> getAllMenu(Guid adminUserId)
        {
            var procedureName = Constant.spGetAllMenu;
            var procedureSubMenu = Constant.getAllSubMenu;

            try
            {
                using (var connection = _dapperContext.createConnection())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@adminUserId", adminUserId);
                    var menus = (await connection.QueryAsync<Menu>(procedureName, param, commandType: CommandType.StoredProcedure)).ToList();
                    foreach (var menu in menus)
                    {
                        DynamicParameters subParam = new DynamicParameters();
                        subParam.Add("@adminUserId", adminUserId);  
                        subParam.Add("@menuId", menu.menuId);  

                        var subMenus = await connection.QueryAsync<SubMenu>(procedureSubMenu, subParam, commandType: CommandType.StoredProcedure);
                        menu.SubMenus = subMenus.ToList(); 
                    }

                    var response = new ResponseViewModel
                    {
                        statusCode = menus.Any() ? (int)HttpStatusCode.OK : (int)HttpStatusCode.NotFound,
                        message = menus.Any() ? "Data Found" : "Data Not Found",
                        data = menus
                    };

                    return response;
                }
            }
            catch (Exception ex)
            {
                return new ResponseViewModel
                {
                    statusCode = (int)HttpStatusCode.InternalServerError,
                    message = $"Error: {ex.Message}",
                    data = null
                };
            }
        }
        public class MenuPermission
        {
            public Guid menuId { get; set; }
            public string? menuName { get; set; }
            public string? menuPageName { get; set; }
            public string? menuIcon { get; set; }
            public int MenuDisplayOrder { get; set; }
            public bool HasMenuPermission { get; set; }

            public List<SubMenu> SubMenus { get; set; } = new List<SubMenu>();
        }
        public class Menu
        {
            public Guid menuId { get; set; }
            public string? menuName { get; set; }
            public string? pageName { get; set; }
            public string? menuIcon { get; set; }
            public int displayOrder { get; set; }

            public List<SubMenu> SubMenus { get; set; } = new List<SubMenu>();
        }

        public class SubMenu
        {
            public Guid subMenuId { get; set; }
            public string? subMenuName { get; set; }
            public string? subMenuPageName { get; set; }

            public int displayOrder { get; set; }
            public bool HasSubMenuPermission { get; set; }

        }

        public async Task<ResponseViewModel> getMenuByUserRole(string userName)
        {
            var procedureName = Constant.spGetMenuByUserRole;
            var parameters = new DynamicParameters();
            parameters.Add("@userName", userName, DbType.String);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<MenuByUserRole>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                var getbyIdMenu = new ResponseViewModel
                {
                    statusCode = result.Count() == 0 ? (int)HttpStatusCode.NotFound : (int)HttpStatusCode.OK,
                    message = result.Count() == 0 ? "Data Not Found" : "Data Found",
                    data = result
                };
                return getbyIdMenu;
            }
        }
        public async Task<ResponseViewModel> addMenu(AddMenuViewModel addMenuViewModel)
        {
            var procedureName = Constant.addMenu;
            var parameters = new DynamicParameters();
            parameters.Add("@menuName", addMenuViewModel.menuName, DbType.String);
            parameters.Add("@displayOrder", addMenuViewModel.displayOrder, DbType.Int32);
            parameters.Add("@createdBy", addMenuViewModel.createdBy, DbType.Guid);
            parameters.Add("@menuIcon", addMenuViewModel.menuIcon, DbType.String);
            parameters.Add("@pageName", addMenuViewModel.pageName, DbType.String);

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
        public async Task<ResponseViewModel> updateMenu(UpdateMenuViewModel updateMenuViewModel)
        {
            var procedureName = Constant.updateMenu;

            var parameters = new DynamicParameters();
            parameters.Add("@menuId", updateMenuViewModel.menuId, DbType.Guid);
            parameters.Add("@menuName", updateMenuViewModel.menuName, DbType.String);
            parameters.Add("@pageName", updateMenuViewModel.pageName, DbType.String);
            parameters.Add("@displayOrder", updateMenuViewModel.displayOrder, DbType.Int32);
            parameters.Add("@updatedBy", updateMenuViewModel.updatedBy, DbType.Guid);
            parameters.Add("@menuIcon", updateMenuViewModel.menuIcon, DbType.String);
            parameters.Add("@active", updateMenuViewModel.active, DbType.Boolean);
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
        public async Task<ResponseViewModel> deleteMenu(DeleteMenuViewModel deleteMenuViewModel)
        {
            var procedureName = Constant.spDeleteMenu;
            var parameters = new DynamicParameters();
            parameters.Add("@menuId", deleteMenuViewModel.menuId, DbType.Guid);
            parameters.Add("@updatedBy", deleteMenuViewModel.updatedBy, DbType.Guid);
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
        public async Task<ResponseViewModel> getAllMenuOfSubMenu(Guid menuId)
        {
            var procedureMenu = Constant.spGetAllMenu;
            var procedureSubMenu = Constant.spGetAllSubMenu;

            try
            {
                using (var connection = _dapperContext.createConnection())
                {
                    var resultMenu = await connection.QueryAsync<Menu>(procedureMenu, commandType: CommandType.StoredProcedure);
                    var resultSubMenu = await connection.QueryAsync<GetAllSubMenu>(procedureSubMenu, commandType: CommandType.StoredProcedure);

                    var subMenuLookup = resultSubMenu.ToLookup(sub => sub.menuId);

                    var menuList = resultMenu.Select(menu => new
                    {
                        menuId = menu.menuId,
                        menuName = menu.menuName,
                        subMenu = subMenuLookup[menu.menuId].Select(sub => new
                        {
                            subMenuId = sub.subMenuId,
                            menuId = sub.menuId,
                            subMenuName = sub.subMenuName,
                            subMenuPageName = sub.subMenuPageName,
                            menuName = sub.menuName,
                            pageName = sub.pageName
                        }).ToList()
                    }).ToList();

                    return new ResponseViewModel
                    {
                        statusCode = menuList.Any() ? (int)HttpStatusCode.OK : (int)HttpStatusCode.NotFound,
                        message = menuList.Any() ? "Data Found" : "Data Not Found",
                        data = menuList
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

        public async Task<ResponseViewModel> menuAndSubMenuPermisiom(Guid appRoleId)
        {
            var procedureName = Constant.getMenuWithSubMenu;
            var procedureSubMenu = Constant.getMenubyMenuId;

            try
            {
                using (var connection = _dapperContext.createConnection())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@appRoleId", appRoleId);

                    // 1. Get all menus (Distinct by menuId)
                    var menus = (await connection.QueryAsync<MenuPermission>(procedureName, param, commandType: CommandType.StoredProcedure))
                                  .GroupBy(m => m.menuId)
                                  .Select(g => g.First())
                                  .ToList();

                    // 2. For each menu, get its submenus
                    foreach (var menu in menus)
                    {
                        DynamicParameters subParam = new DynamicParameters();
                        subParam.Add("@menuId", menu.menuId);
                        subParam.Add("@appRoleId", appRoleId);

                        var subMenus = await connection.QueryAsync<SubMenu>(procedureSubMenu, subParam, commandType: CommandType.StoredProcedure);

                        // Remove duplicates from submenus also
                        menu.SubMenus = subMenus
                                        .GroupBy(s => s.subMenuId)
                                        .Select(g => g.First())
                                        .ToList();
                    }

                    var response = new ResponseViewModel
                    {
                        statusCode = menus.Any() ? (int)HttpStatusCode.OK : (int)HttpStatusCode.NotFound,
                        message = menus.Any() ? "Data Found" : "Data Not Found",
                        data = menus
                    };

                    return response;
                }
            }
            catch (Exception ex)
            {
                return new ResponseViewModel
                {
                    statusCode = (int)HttpStatusCode.InternalServerError,
                    message = $"Error: {ex.Message}",
                    data = null
                };
            }
        }

        public async Task<ResponseViewModel> getAllAdminListbyPermission()
        {
            var procedureName = Constant.getAllAdminName;
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

       
        public async Task<ResponseViewModel> addMenuWithSubMenuBatch(List<AddMenuWithSubMenu> menuList)
        {
            var procedureName = Constant.addMenuWithSubMenuBatch;

            // TVP Table Structure
            DataTable dt = new DataTable();
            dt.Columns.Add("appRoleId", typeof(Guid));
            dt.Columns.Add("menuId", typeof(Guid));
            dt.Columns.Add("menuName", typeof(string));
            dt.Columns.Add("pageName", typeof(string));
            dt.Columns.Add("displayOrder", typeof(int));
            dt.Columns.Add("createdBy", typeof(Guid));
            dt.Columns.Add("menuIcon", typeof(string));
            dt.Columns.Add("subMenuId", typeof(Guid));
            dt.Columns.Add("subMenuName", typeof(string));
            dt.Columns.Add("subMenuPageName", typeof(string));
            dt.Columns.Add("displayOrderSubMenu", typeof(int));
            dt.Columns.Add("ActiveSubmenu", typeof(bool));
            dt.Columns.Add("Activemenu", typeof(bool));

            // Fill DataTable
            foreach (var menu in menuList)
            {
                if (menu.subMenuList != null && menu.subMenuList.Any())
                {
                    foreach (var sub in menu.subMenuList)
                    {
                        dt.Rows.Add(
                            menu.appRoleId,
                            menu.menuId,
                            menu.menuName ?? "",
                            menu.pageName ?? "",
                            menu.displayOrder,
                            menu.createdBy == Guid.Empty ? Guid.NewGuid() : menu.createdBy,
                            menu.menuIcon ?? "",
                            sub.subMenuId == Guid.Empty ? Guid.NewGuid() : sub.subMenuId,
                            sub.subMenuName ?? "",
                            sub.subMenuPageName ?? "",
                            sub.displayOrderSubMenu,
                            sub.ActiveSubmenu,
                            menu.Activemenu
                        );
                    }
                }
                else
                {
                    dt.Rows.Add(
                        menu.appRoleId,
                        menu.menuId,
                        menu.menuName ?? "",
                        menu.pageName ?? "",
                        menu.displayOrder,
                        menu.createdBy == Guid.Empty ? Guid.NewGuid() : menu.createdBy,
                        menu.menuIcon ?? "",
                        DBNull.Value,
                        DBNull.Value,
                        DBNull.Value,
                        DBNull.Value,
                        false,
                        menu.Activemenu
                    );
                }
            }

            using (var connection = _dapperContext.createConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@MenuSubMenuTable", dt.AsTableValuedParameter("dbo.MenuSubMenuType"));

                try
                {
                    var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModel>(
                        procedureName, parameters, commandType: CommandType.StoredProcedure
                    );

                    if (result == null)
                    {
                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.ExpectationFailed,
                            message = "⚠️ No response from database."
                        };
                    }

                    result.statusCode = result.statusCode == 1
                        ? (int)HttpStatusCode.OK
                        : (int)HttpStatusCode.ExpectationFailed;

                    return result;
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.InternalServerError,
                        message = $"❌ Exception: {ex.Message}"
                    };
                }
            }
        }
    }
}
