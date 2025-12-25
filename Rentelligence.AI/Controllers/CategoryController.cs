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
    public class CategoryController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILoggerManager _logger;
        public CategoryController(IServiceManager serviceManager, ILoggerManager logger, IConfiguration configuration)
        {
            _serviceManager = serviceManager;
            _logger = logger;
        }
        [HttpGet("getByIdCategory/{categoryId}")]
        public async Task<IActionResult> getByIdCategory(Guid categoryId)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getByIdCategory categoryId ${categoryId}");
            var getByIdCategory = await _serviceManager.categoryContract.getByIdCategory(categoryId);
            if (getByIdCategory.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Category Found");
            }
            return Ok(getByIdCategory);
        }

        [HttpGet("getAllCategory")]
        public async Task<IActionResult> getAllCategory()
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} getAllCategory");
            var getAllCategory = await _serviceManager.categoryContract.getAllCategory();
            if (getAllCategory.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Category Found");
            }
            return Ok(getAllCategory);
        }
       

        [HttpGet("getAllActiveCategory")]       
        public async Task<IActionResult> getAllActiveCategory()
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} getAllCategoryForUser");
            var getAllCategory = await _serviceManager.categoryContract.getAllCategoryForUser();
            if (getAllCategory.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Category Found");
            }
            return Ok(getAllCategory);
        }


        [HttpPost("addCategory")]    
        public async Task<IActionResult> addCategory(AddCategoryViewModel addCategory)
        {
            _logger.logInfo($" {LoggingEvents.addItem} addCategory");
            var add = await _serviceManager.categoryContract.addCategory(addCategory);
            return Ok(add);
        }
     
        [HttpPost("updateCategory")] 
        public async Task<IActionResult> updateCategory(UpdateCategoryViewModel updateCategory)
        {
            _logger.logInfo($" {LoggingEvents.updateItem} updateCategory");
            var update = await _serviceManager.categoryContract.updateCategory(updateCategory);
            return Ok(update);
        }

        [HttpPost("deleteCategory")]
        public async Task<IActionResult> deleteCategory(DeleteCategoryViewModel deleteCategory)
        {
            _logger.logInfo($" {LoggingEvents.deleteItem} deleteCategory");
            var delete = await _serviceManager.categoryContract.deleteCategory(deleteCategory);
            return Ok(delete);
        }
    }
}
