using Common;
using EmailSystem;
using LoggerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ServiceContract;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using ViewModel;

namespace Rentelligence.AI.MarketPlace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILoggerManager _logger;
        private readonly IConfiguration _configuration;
        private readonly EmailService emailService;
        private readonly ExtractToken extractToken;
        public SellerController(IServiceManager serviceManager, ILoggerManager logger, IConfiguration configuration)
        {
            _serviceManager = serviceManager;
            _logger = logger;
            _configuration = configuration;
            emailService = new EmailService(configuration);
            extractToken = new ExtractToken(configuration);
        }

        [HttpGet("getByIdSeller/{sellerId}")]
        public async Task<IActionResult> getByIdSeller(Guid sellerId)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getByIdSeller sellerId ${sellerId}");
            var getByIdSeller = await _serviceManager.sellerContract.getByIdSeller(sellerId);
            if (getByIdSeller.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Seller Found");
            }
            return Ok(getByIdSeller);
        }

        [HttpGet("getAllSeller")]
        public async Task<IActionResult> getAllSeller()
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} getAllSeller");
            var getAllSeller = await _serviceManager.sellerContract.getAllSeller();
            if (getAllSeller.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Seller Found");
            }
            return Ok(getAllSeller);
        }


        [HttpGet("getAllActiveSeller")]
        public async Task<IActionResult> getAllActiveSeller()
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} getAllSellerForUser");
            var getAllSellerForUser = await _serviceManager.sellerContract.getAllSellerForUser();
            if (getAllSellerForUser.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Seller Found");
            }
            return Ok(getAllSellerForUser);
        }

        [HttpPost("addSeller")]
        public async Task<IActionResult> addSeller(AddSellerViewModel addSeller)
        {
            _logger.logInfo($" {LoggingEvents.addItem} addSeller");
            var add = await _serviceManager.sellerContract.addSeller(addSeller);
            return Ok(add);
        }

        [HttpPost("updateSeller")]
        public async Task<IActionResult> updateSeller(UpdateSellerViewModel updateSeller)
        {
            _logger.logInfo($" {LoggingEvents.updateItem} updateSeller");
            var update = await _serviceManager.sellerContract.updateSeller(updateSeller);
            return Ok(update);
        }

        [HttpPost("deleteSeller")]
        public async Task<IActionResult> deleteSeller(DeleteSellerViewModel deleteSeller)
        {
            _logger.logInfo($" {LoggingEvents.deleteItem} deleteSeller");
            var delete = await _serviceManager.sellerContract.deleteSeller(deleteSeller);
            return Ok(delete);
        }    
    }
}

