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

    public class SubCategoryController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILoggerManager _logger;
        public SubCategoryController(IServiceManager serviceManager, ILoggerManager logger, IConfiguration configuration)
        {
            _serviceManager = serviceManager;
            _logger = logger;
        }
        [HttpGet("getByIdSubCategory/{subCategoryId}")]
        public async Task<IActionResult> getByIdSubCategory(Guid subCategoryId)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getByIdSubCategory subCategoryId ${subCategoryId}");
            var getByIdSubCategory = await _serviceManager.subCategoryContract.getByIdSubCategory(subCategoryId);
            if (getByIdSubCategory.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Sub Category Found");
            }
            return Ok(getByIdSubCategory);
        }
        
        [HttpGet("getAllSubCategory")]
        public async Task<IActionResult> getAllSubCategory()
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} getAllSubCategory");
            var getAllSubCategory = await _serviceManager.subCategoryContract.getAllSubCategory();
            if (getAllSubCategory.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Sub Category Found");
            }
            return Ok(getAllSubCategory);
        }

        [HttpGet("getAllActiveSubCategory")]
        public async Task<IActionResult> getAllActiveSubCategory()
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} getAllSubCategoryForUser");
            var getAllSubCategory = await _serviceManager.subCategoryContract.getAllSubCategoryForUser();
            if (getAllSubCategory.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Sub Category Found");
            }
            return Ok(getAllSubCategory);
        }

        [HttpPost("addSubCategory")]
        public async Task<IActionResult> addSubCategory(AddSubCategoryViewModel addSubCategory)
        {
            _logger.logInfo($" {LoggingEvents.addItem} addSubCategory");
            var add = await _serviceManager.subCategoryContract.addSubCategory(addSubCategory);
            return Ok(add);
        }

        [HttpPost("updateSubCategory")]
        public async Task<IActionResult> updateSubCategory(UpdateSubCategoryViewModel updateSubCategory)
        {
            _logger.logInfo($" {LoggingEvents.updateItem} updateSubCategory");
            var update = await _serviceManager.subCategoryContract.updateSubCategory(updateSubCategory);
            return Ok(update);
        }

        [HttpPost("deleteSubCategory")]
        public async Task<IActionResult> deleteSubCategory(DeleteSubCategoryViewModel deleteSubCategory)
        {
            _logger.logInfo($" {LoggingEvents.deleteItem} deleteSubCategory");
            var delete = await _serviceManager.subCategoryContract.deleteSubCategory(deleteSubCategory);
            return Ok(delete);
        }
        //--New 
        //[HttpGet("getAllRoboticsAgentsSubCat")]
        //public async Task<IActionResult> getAllRoboticsAgentsSubCat(Guid? SubcategoryId)
        //{
        //    _logger.logInfo($" {LoggingEvents.getAllItem} getAllRoboticsAgentsSubCat");
        //    var getAllRoboticsAgents = await _serviceManager.subCategoryContract.getAllRoboticsAgentsSubCat(SubcategoryId);
        //    if (getAllRoboticsAgents.statusCode == (int)HttpStatusCode.NotFound)
        //    {
        //        _logger.logWarn($"{LoggingEvents.getItemNotFound},No Robotics Agent");
        //    }
        //    return Ok(getAllRoboticsAgents);
        //}


 
        //[HttpGet("getAllAIAgentsSubCat")]
        //public async Task<IActionResult> getAllAIAgentsSubCat(Guid? SubcategoryId)
        //{
        //    _logger.logInfo($" {LoggingEvents.getAllItem} getAllAIAgentsSubCat");
        //    var getAllAIAgentsSubCat = await _serviceManager.subCategoryContract.getAllAIAgentsSubCat(SubcategoryId);
        //    if (getAllAIAgentsSubCat.statusCode == (int)HttpStatusCode.NotFound)
        //    {
        //        _logger.logWarn($"{LoggingEvents.getItemNotFound},No AI Agents");
        //    }
        //    return Ok(getAllAIAgentsSubCat);
        //}

        //[HttpPost("getAllProjectAgentsSubCat")]
        //public async Task<IActionResult> getAllProjectAgentsSubCat(Guid? SubcategoryId)
        //{
        //    _logger.logInfo($" {LoggingEvents.getAllItem} getAllProjectAgentsSubCat");
        //    var getAllProjectAgentsSubCat = await _serviceManager.subCategoryContract.getAllProjectAgentsSubCat(SubcategoryId);
        //    if (getAllProjectAgentsSubCat.statusCode == (int)HttpStatusCode.NotFound)
        //    {
        //        _logger.logWarn($"{LoggingEvents.getItemNotFound},No Projects Agents");
        //    }
        //    return Ok(getAllProjectAgentsSubCat);
        //}
    }
}
