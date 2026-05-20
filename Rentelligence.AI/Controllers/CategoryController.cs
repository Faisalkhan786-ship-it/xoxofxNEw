using LoggerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceContract;
using System.Net;
using ViewModel;

namespace XoxoFX_Apis.AI.Controllers
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
        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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
        //[HttpGet("getAllCategorytest")]
        //public async Task<IActionResult> getAllCategorytest()
        //{
        //    _logger.logInfo($" {LoggingEvents.getAllItem} getAllCategory");
        //    var getAllCategory = await _serviceManager.categoryContract.getAllCategorytest();
        //    if (getAllCategory.statusCode == (int)HttpStatusCode.NotFound)
        //    {
        //        _logger.logWarn($"{LoggingEvents.getItemNotFound},No Category Found");
        //    }
        //    return Ok(getAllCategory);
        //}      
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

        [Authorize(Roles = "Admin")]
        [HttpPost("addCategory")]    
        public async Task<IActionResult> addCategory(AddCategoryViewModel addCategory)
        {
            _logger.logInfo($" {LoggingEvents.addItem} addCategory");
            var add = await _serviceManager.categoryContract.addCategory(addCategory);
            return Ok(add);
        }


        //[HttpPost("addCategorytest")]
        //public async Task<IActionResult> addCategorytest(AddCategoryViewModel addCategory)
        //{
        //    _logger.logInfo($" {LoggingEvents.addItem} addCategory");
        //    var add = await _serviceManager.categoryContract.addCategorytest(addCategory);
        //    return Ok(add);
        //}

        [HttpPost("updateCategory")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> updateCategory(UpdateCategoryViewModel updateCategory)
        {
            _logger.logInfo($" {LoggingEvents.updateItem} updateCategory");
            var update = await _serviceManager.categoryContract.updateCategory(updateCategory);
            return Ok(update);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("deleteCategory")]
        public async Task<IActionResult> deleteCategory(DeleteCategoryViewModel deleteCategory)
        {
            _logger.logInfo($" {LoggingEvents.deleteItem} deleteCategory");
            var delete = await _serviceManager.categoryContract.deleteCategory(deleteCategory);
            return Ok(delete);
        }


        //[HttpPost("addCloudImages")]
        //public async Task<IActionResult> addCloudImages(AddCloudImages addCloudImages)
        //{
        //    _logger.logInfo($" {LoggingEvents.addItem} addCloudImages");
        //    var add = await _serviceManager.categoryContract.addCloudImages(addCloudImages);
        //    return Ok(add);
        //}

        //[HttpGet("getCloudImages")]
        //public async Task<IActionResult> getCloudImages()
        //{
        //    _logger.logInfo($" {LoggingEvents.getAllItem} getCloudImages");
        //    var getCloudImages = await _serviceManager.categoryContract.getCloudImages();
        //    if (getCloudImages.statusCode == (int)HttpStatusCode.NotFound)
        //    {
        //        _logger.logWarn($"{LoggingEvents.getItemNotFound},No Category Found");
        //    }
        //    return Ok(getCloudImages);
        //}
        //[HttpPost("deleteCloudImage")]
        //public async Task<IActionResult> deleteCloudImage(int? Id)
        //{
        //    _logger.logInfo($" {LoggingEvents.deleteItem} deleteCloudImage");
        //    var delete = await _serviceManager.categoryContract.deleteCloudImage(Id);
        //    return Ok(delete);
        //}
    }
}
