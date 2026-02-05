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

    public class SubMenuController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILoggerManager _logger;
        private readonly ExtractToken extractToken;
        public SubMenuController(IServiceManager serviceManager, ILoggerManager logger, IConfiguration configuration)
        {
            _serviceManager = serviceManager;
            _logger = logger;
            extractToken = new ExtractToken(configuration);
        }
        [HttpGet("getByIdSubMenu/{SubMenuId}")]
        public async Task<IActionResult> getByIdSubMenu(Guid SubMenuId)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getByIdSubMenu SubMenuId ${SubMenuId}");
            var getByIdSubMenu = await _serviceManager.subMenuContract.getByIdSubMenu(SubMenuId);
            if (getByIdSubMenu.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No SubMenu Found");
            }
            return Ok(getByIdSubMenu);
        }


        [HttpGet("getAllSubMenu")]
        public async Task<IActionResult> getAllSubMenu()
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} getAllSubMenu");
            var getAllSubMenu = await _serviceManager.subMenuContract.getAllSubMenu();
            if (getAllSubMenu.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No SubMenu Found");
            }
            return Ok(getAllSubMenu);
        }

        [HttpPost("addSubMenu")]
        public async Task<IActionResult> addSubMenu(AddSubMenuViewModel addSubMenuViewModel)
        {
            _logger.logInfo($" {LoggingEvents.addItem} addSubMenu");
            var addSubMenu = await _serviceManager.subMenuContract.addSubMenu(addSubMenuViewModel);
            return Ok(addSubMenu);
        }

        [HttpPost("updateSubMenu")]
        public async Task<IActionResult> updateSubMenu(UpdateSubMenuViewModel updateSubMenuViewModel)
        {
            _logger.logInfo($" {LoggingEvents.updateItem} updateSubMenu");
            var updateSubMenu = await _serviceManager.subMenuContract.updateSubMenu(updateSubMenuViewModel);
            return Ok(updateSubMenu);
        }

        [HttpPost("deleteSubMenu")]
        public async Task<IActionResult> deleteSubMenu(DeleteSubMenuViewModel deleteSubMenuViewModel)
        {
            _logger.logInfo($" {LoggingEvents.deleteItem} deleteItem");
            var deleteSubMenu = await _serviceManager.subMenuContract.deleteSubMenu(deleteSubMenuViewModel);
            return Ok(deleteSubMenu);
        }

        [HttpGet("getSubMenubyMenuId")]
        public async Task<IActionResult> getSubMenubyMenuId(Guid menuId)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getSubMenubyMenuId MenuId ${menuId}");
            var getByIdSubMenu = await _serviceManager.subMenuContract.getSubMenubyMenuId(menuId);
            if (getByIdSubMenu.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No SubMenu Found");
            }
            return Ok(getByIdSubMenu);
        }
    }
}
