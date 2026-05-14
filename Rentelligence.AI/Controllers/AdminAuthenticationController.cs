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
    public class AdminAuthenticationController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILoggerManager _logger;
        private readonly IConfiguration _configuration;
        private readonly EmailService emailService;
        private readonly ExtractToken extractToken;
        public AdminAuthenticationController(IServiceManager serviceManager, ILoggerManager logger, IConfiguration configuration)
        {
            _serviceManager = serviceManager;
            _logger = logger;
            extractToken = new ExtractToken(configuration);
            emailService = new EmailService(configuration);
            _configuration = configuration;
        }

        
        [HttpPost("adminLogin")]
        public async Task<IActionResult> adminLogin(AdminUserLoginViewModel adminLogin)
        {
            _logger.logInfo($"{LoggingEvents.getByIdItem} adminLogin");

            var adminDetails = await _serviceManager.adminAuthenticationContract.adminUserLogin(adminLogin);

            string token = null;
            var userData = (adminDetails.data as IEnumerable<dynamic>)?.FirstOrDefault();

            if (adminDetails.statusCode == (int)HttpStatusCode.OK && userData != null)
            {
                token = AdminGenerateTokenForUserName(userData);
            }
            else
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound}, No User Found");
            }

            var response = new
            {
                token,
                statusCode = adminDetails.statusCode,
                message = adminDetails.message,
                data = userData
            };

            return Ok(response);
        }
        private string AdminGenerateTokenForUserName(dynamic adminData)
        {
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

        new Claim("username", adminData.username.ToString()),
        new Claim(ClaimTypes.Role, adminData.Role.ToString()),

        new Claim("adminUserId", adminData.adminUserId.ToString()),
        new Claim("appRoleId", adminData.appRoleId.ToString())
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("addAdminUser")]
        public async Task<IActionResult> addAdminUser(AddAdminUserViewModel addAdminUser)
        {
            _logger.logInfo($" {LoggingEvents.updateItem} addAdminUser");
            var returnData = await _serviceManager.adminAuthenticationContract.addAdminUser(addAdminUser);
            return Ok(returnData);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("getAdminUserDetails")]
        public async Task<IActionResult> getAdminUserDetails(AdminUserGuidViewModel adminUserGuid)
        {
  
            var returnData = await _serviceManager.adminAuthenticationContract.getAdminUserDetails(adminUserGuid);          
            return Ok(returnData);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getAdminDashboardDetails")]
        public async Task<IActionResult> getAdminDashboardDetails(Guid adminUserId)
        {
            _logger.logInfo($" {LoggingEvents.updateItem} getAdminDashboardDetails");
            var returnData = await _serviceManager.adminAuthenticationContract.getAdminDashboardDetails(adminUserId);
            return Ok(returnData);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getAllAdminList")]
        public async Task<IActionResult> getAllAdminList()
        {
            _logger.logInfo($" {LoggingEvents.updateItem} getAllAdminList");
            var returnData = await _serviceManager.adminAuthenticationContract.getAllAdminList();
            return Ok(returnData);
        }

        
    }
}



//[HttpPost("adminDeActivate")]
//public async Task<IActionResult> adminDeActivate(Guid adminuserId)
//{
//    _logger.logInfo($" {LoggingEvents.updateItem} getAdminUserDetails");
//    var returnData = await _serviceManager.adminAuthenticationContract.updateAdminStatusDeActivate(adminuserId);
//    return Ok(returnData);
//}

//[HttpPost("adminActivate")]
//public async Task<IActionResult> adminActivate(Guid adminuserId)
//{

//    _logger.logInfo($" {LoggingEvents.updateItem} getAdminUserDetails");
//    var returnData = await _serviceManager.adminAuthenticationContract.updateAdminStatusActivate(adminuserId);
//    return Ok(returnData);
//}

//[HttpPost("addBulkRegsitration")]
//public async Task<IActionResult> addBulkRegsitration(BulkRegsitrationViewModel bulkRegsitrationViewModel)
//{
//    _logger.logInfo($" {LoggingEvents.updateItem} addBulkRegsitration");
//    var addBulkRegsitration = await _serviceManager.adminAuthenticationContract.addBulkRegsitration(bulkRegsitrationViewModel);
//    return Ok(addBulkRegsitration);
//}


//[HttpPost("adminForgotPassword")]
//public async Task<IActionResult> adminForgotPassword(string username)
//{

//    _logger.logInfo($" {LoggingEvents.updateItem} adminForgotPassword");
//    var adminForgotPassword = await _serviceManager.adminAuthenticationContract.adminForgotPassword(username);
//    return Ok(adminForgotPassword);
//}

//[HttpPost("updateAdminProfile")]
//public async Task<IActionResult> updateAdminProfile(UpdateAdminProfileViewModel updateAdminProfileViewModel)
//{
//    _logger.logInfo($" {LoggingEvents.updateItem} addAdminUser");
//    var returnData = await _serviceManager.adminAuthenticationContract.updateAdminProfile(updateAdminProfileViewModel);
//    return Ok(returnData);
//}