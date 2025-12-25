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
    public class SubCategoryTypeController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILoggerManager _logger;
        public SubCategoryTypeController(IServiceManager serviceManager, ILoggerManager logger, IConfiguration configuration)
        {
            _serviceManager = serviceManager;
            _logger = logger;
        }
        [HttpGet("getByIdSubCategoryType/{subCategoryTypeId}")]
        public async Task<IActionResult> getByIdSubCategory(Guid subCategoryTypeId)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getByIdSubCategoryType subCategoryTypeId ${subCategoryTypeId}");
            var getByIdSubCategoryType = await _serviceManager.subCategoryTypeContract.getByIdSubCategoryType(subCategoryTypeId);
            if (getByIdSubCategoryType.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Sub Category Type Found");
            }
            return Ok(getByIdSubCategoryType);
        }

        [HttpGet("getAllSubCategoryType")]
        public async Task<IActionResult> getAllSubCategoryType()
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} getAllSubCategoryType");
            var getAllSubCategoryType = await _serviceManager.subCategoryTypeContract.getAllSubCategoryType();
            if (getAllSubCategoryType.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Sub Category Type Found");
            }
            return Ok(getAllSubCategoryType);
        }

        [HttpGet("getAllActiveSubCategoryType")]
        public async Task<IActionResult> getAllActiveSubCategoryType()
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} getAllSubCategoryTypeForUser");
            var getAllSubCategoryType = await _serviceManager.subCategoryTypeContract.getAllSubCategoryTypeForUser();
            if (getAllSubCategoryType.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Sub Category Type Found");
            }
            return Ok(getAllSubCategoryType);
        }

        [HttpPost("addSubCategoryType")]
        public async Task<IActionResult> addSubCategoryType(AddSubCategoryTypeViewModel addSubCategoryType)
        {
            _logger.logInfo($" {LoggingEvents.addItem} addSubCategoryType");
            var add = await _serviceManager.subCategoryTypeContract.addSubCategoryType(addSubCategoryType);
            return Ok(add);
        }

        [HttpPost("updateSubCategoryType")]
        public async Task<IActionResult> updateSubCategory(UpdateSubCategoryTypeViewModel updateSubCategoryType)
        {
            _logger.logInfo($" {LoggingEvents.updateItem} updateSubCategoryType");
            var update = await _serviceManager.subCategoryTypeContract.updateSubCategoryType(updateSubCategoryType);
            return Ok(update);
        }

        [HttpPost("deleteSubCategoryType")]
        public async Task<IActionResult> deleteSubCategoryType(DeleteSubCategoryTypeViewModel deleteSubCategoryType)
        {
            _logger.logInfo($" {LoggingEvents.deleteItem} deleteSubCategoryType");
            var delete = await _serviceManager.subCategoryTypeContract.deleteSubCategoryType(deleteSubCategoryType);
            return Ok(delete);
        }
    }
}
