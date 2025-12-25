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
    public class ProductController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILoggerManager _logger;
        public ProductController(IServiceManager serviceManager, ILoggerManager logger, IConfiguration configuration)
        {
            _serviceManager = serviceManager;
            _logger = logger;
        }

        [HttpGet("getAllProduct")]
        public async Task<IActionResult> getAllProduct()
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} getAllProduct");
            var getAllProduct = await _serviceManager.productContract.getAllProduct();
            if (getAllProduct.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Product Found");
            }
            return Ok(getAllProduct);
        }


        [HttpPost("addProduct")]
        public async Task<IActionResult> addProduct(AddProductViewModel addProduct)
        {
            _logger.logInfo($" {LoggingEvents.addItem} addProduct");
            var add = await _serviceManager.productContract.addProduct(addProduct);
            return Ok(add);
        }
    
        [HttpPost("updateProduct")]
        public async Task<IActionResult> updateProduct(UpdateProductViewModel updateProduct)
        {
            _logger.logInfo($" {LoggingEvents.updateItem} updateProductImage");
            var update = await _serviceManager.productContract.updateProduct(updateProduct);
            return Ok(update);
        }


        [HttpPost("deleteProduct")]
        public async Task<IActionResult> deleteCategory(DeleteProductViewModel deleteProduct)
        {
            _logger.logInfo($" {LoggingEvents.deleteItem} deleteProduct");
            var delete = await _serviceManager.productContract.deleteProduct(deleteProduct);
            return Ok(delete);
        }



        [HttpPost("addProductImage")]
        public async Task<IActionResult> addProductImage(AddProductImageViewModel addProductImage)
        {
            _logger.logInfo($" {LoggingEvents.addItem} addProductImage");
            var add = await _serviceManager.productContract.addProductImage(addProductImage);
            return Ok(add);
        }
       
  
        [HttpPost("deleteProductImage")]
        public async Task<IActionResult> deleteProductImage(DeleteProductImageViewModel deleteProductImage)
        {
            _logger.logInfo($" {LoggingEvents.deleteItem} deleteProductImage");
            var delete = await _serviceManager.productContract.deleteProductImage(deleteProductImage);
            return Ok(delete);
        }

        [HttpGet("getByIdImage")]
        public async Task<IActionResult> getByIdImage(Guid productId)
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} getByIdImage");
            var getByIdImage = await _serviceManager.productContract.getByIdImage(productId);
            if (getByIdImage.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Product Image Found");
            }
            return Ok(getByIdImage);
        }

        [HttpGet("getAllRoboticsAgentsforUser")]
        public async Task<IActionResult> getAllRoboticsAgentsforUser(Guid? ProductId)
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} getAllRoboticsAgents");
            var getAllRoboticsAgents = await _serviceManager.productContract.getAllRoboticsAgents(ProductId);
            if (getAllRoboticsAgents.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Robotics Agent");
            }
            return Ok(getAllRoboticsAgents);
        }

        [HttpGet("getAllAIAgentsforUser")]
        public async Task<IActionResult> getAllAIAgentsforUser(Guid? ProductId)
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} getAllAIAgents");
            var getAllAIAgents = await _serviceManager.productContract.getAllAIAgents(ProductId);
            if (getAllAIAgents.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No AI Agents");
            }
            return Ok(getAllAIAgents);
        }

        [HttpPost("getAllProjectAgentsforUser")]
        public async Task<IActionResult> getAllProjectAgentsforUser(Guid? ProductId)
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} getAllProjectAgents");
            var getAllProjectAgents = await _serviceManager.productContract.getAllProjectAgents(ProductId);
            if (getAllProjectAgents.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Projects Agents");
            }
            return Ok(getAllProjectAgents);
        }
    }
}
