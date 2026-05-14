using Common;
using LoggerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceContract;
using System.Net;
using ViewModel;

namespace Rentelligence.AI.MarketPlace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class MenuController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILoggerManager _logger;
        private readonly ExtractToken extractToken;
        public MenuController(IServiceManager serviceManager, ILoggerManager logger, IConfiguration configuration)
        {
            _serviceManager = serviceManager;
            _logger = logger;
            extractToken = new ExtractToken(configuration);
        }
        [HttpGet("getByIdMenu/{menuId}")]
        public async Task<IActionResult> getByIdMenu(Guid menuId)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getByIdMenu menuId ${menuId}");
            var getByIdMenu = await _serviceManager.menuContract.getByIdMenu(menuId);
            if (getByIdMenu.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Menu Found");
            }
            return Ok(getByIdMenu);
        }

        [HttpGet("getMenuByUserRole")]
        public async Task<IActionResult> getMenuByUserRole()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            TokenDetailsViewModel loginViewModel = extractToken.ExtractUserDetailsFromToken(token);
            _logger.logInfo($" {LoggingEvents.getByIdItem} getMenuByUserRole");
            var getMenuByUserRole = await _serviceManager.menuContract.getMenuByUserRole(loginViewModel.username);
            if (getMenuByUserRole.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Menu Found");
            }
            return Ok(getMenuByUserRole);
        }

        //ye main h jo use ho rahi h menu and submenu me 
        [HttpGet("getAllMenu")]
        public async Task<IActionResult> getAllMenu(Guid adminUserId)
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} getAllMenu");
            var getAllMenu = await _serviceManager.menuContract.getAllMenu(adminUserId);
            if (getAllMenu.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Menu Found");
            }
            return Ok(getAllMenu);
        }

        [HttpPost("addMenu")]
        public async Task<IActionResult> addMenu(AddMenuViewModel addMenuViewModel)
        {
            _logger.logInfo($" {LoggingEvents.addItem} addMenu");
            var addMenu = await _serviceManager.menuContract.addMenu(addMenuViewModel);
            return Ok(addMenu);
        }

        [HttpPost("updateMenu")]
        public async Task<IActionResult> updateMenu(UpdateMenuViewModel updateMenuViewModel)
        {
            _logger.logInfo($" {LoggingEvents.updateItem} updateMenu");
            var updateMenu = await _serviceManager.menuContract.updateMenu(updateMenuViewModel);
            return Ok(updateMenu);
        }

        [HttpDelete("deleteMenu")]
        public async Task<IActionResult> deleteMenu(DeleteMenuViewModel deleteMenuViewModel)
        {
            _logger.logInfo($" {LoggingEvents.deleteItem} deleteItem");
            var deleteMenu = await _serviceManager.menuContract.deleteMenu(deleteMenuViewModel);
            return Ok(deleteMenu);
        }

        [HttpGet("getAllMenuOfSubMenu")]
        public async Task<IActionResult> getAllMenuOfSubMenu(Guid menuId)
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} getAllMenuOfSubMenu");
            var getAllMenu = await _serviceManager.menuContract.getAllMenuOfSubMenu(menuId);
            if (getAllMenu.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Menu Found");
            }
            return Ok(getAllMenu);
        }

        [HttpGet("menuAndSubMenuPermisiom")]
        public async Task<IActionResult> menuAndSubMenuPermisiom(Guid appRoleId)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getByIdMenu menuId ${appRoleId}");
            var getByIdMenu = await _serviceManager.menuContract.menuAndSubMenuPermisiom(appRoleId);
            if (getByIdMenu.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Menu Found");
            }
            return Ok(getByIdMenu);
        }

        [HttpPost("addMenuWithSubMenuBatch")]
        public async Task<IActionResult> addMenuWithSubMenuBatch([FromBody] AddMenuWithSubMenu menuItem)
        {

            var menuList = new List<AddMenuWithSubMenu> { menuItem };

            var result = await _serviceManager.menuContract.addMenuWithSubMenuBatch(menuList);
            return Ok(result);
        }


        [HttpGet("getAllAdminListbyPermission")]
        public async Task<IActionResult> getAllAdminListbyPermission()
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getByIdMenu menuId ${getAllAdminListbyPermission}");
            var getByIdMenu = await _serviceManager.menuContract.getAllAdminListbyPermission();
            if (getByIdMenu.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Menu Found");
            }
            return Ok(getByIdMenu);
        }


    }
}
