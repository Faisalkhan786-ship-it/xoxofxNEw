using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ViewModel;
using Common;
using EmailSystem;
using LoggerService;
using Microsoft.AspNetCore.Authorization;

using Microsoft.IdentityModel.Tokens;
using ServiceContract;
using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;
using System.Text;


namespace Rentelligence.AI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminManageUserController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILoggerManager _logger;
        private readonly IConfiguration _configuration;
        private readonly EmailService emailService;
        private readonly ExtractToken extractToken;

        public AdminManageUserController(IServiceManager serviceManager, ILoggerManager logger, IConfiguration configuration)
        {
            _serviceManager = serviceManager;
            _logger = logger;
            extractToken = new ExtractToken(configuration);
            emailService = new EmailService(configuration);
            _configuration = configuration;
        }

        [HttpPost("SearchAllUsers")]
        public async Task<IActionResult> SearchAllUsers(AdminManageViewModel adminManageViewModel)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} SearchAllUsers");
            var adminDetails = await _serviceManager.adminManageService.adminSearchAllUsers(adminManageViewModel);
            return Ok(adminDetails);
        }

        [HttpPost("getRentWithdrawalWallet")]
        public async Task<IActionResult> getRentWallet(AppUnApprentViewModel appUnApprentViewModel)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getRentWallet");
            var getRentWallet = await _serviceManager.adminManageService.getRentWallet(appUnApprentViewModel);
            return Ok(getRentWallet);
        }
    }
}
