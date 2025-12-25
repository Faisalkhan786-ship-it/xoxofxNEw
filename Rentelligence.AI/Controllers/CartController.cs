using LoggerService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceContract;
using System.Net;
using ViewModel;
using static ViewModel.CartViewModel;

namespace Rentelligence.AI.MarketPlace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILoggerManager _logger;
        public CartController(IServiceManager serviceManager, ILoggerManager logger, IConfiguration configuration)
        {
            _serviceManager = serviceManager;
            _logger = logger;
        }
        [HttpPost("addCart")]
        public async Task<IActionResult> addCart(AddCartViewModel addCartViewModel)
        {
            _logger.logInfo($" {LoggingEvents.addItem} addCart");
            var add = await _serviceManager.cartContract.addCart(addCartViewModel);
            return Ok(add);
        }
        [HttpPost("getCartlist")]
        public async Task<IActionResult> getCartlist(Guid userId)
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} getCartList");
            var getAllProjectAgents = await _serviceManager.cartContract.getCartlist(userId);
            if (getAllProjectAgents.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Cart Details");
            }
            return Ok(getAllProjectAgents);
        }
        [HttpPost("removeCart")]
        public async Task<IActionResult> removeCart(DeleteCartViewModel deleteCartViewModel)
        {
            _logger.logInfo($" {LoggingEvents.addItem} removeCart");
            var removeCart = await _serviceManager.cartContract.removeCart(deleteCartViewModel);
            return Ok(removeCart);
        }
    }
}
