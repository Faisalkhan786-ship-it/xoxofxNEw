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

namespace XoxoFX_Apis.AI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILoggerManager _logger;
        private readonly IConfiguration _configuration;
        private readonly EmailService emailService;
        private readonly ExtractToken extractToken;
        public AuthenticationController(IServiceManager serviceManager, ILoggerManager logger, IConfiguration configuration)
        {
            _serviceManager = serviceManager;
            _logger = logger;
            _configuration = configuration;
            emailService = new EmailService(configuration);
            extractToken = new ExtractToken(configuration);
        }


        public class UserDto
        {
            public Guid URID { get; set; }
            public string Username { get; set; }
            public string Role { get; set; }
        }

        public static class Roles
        {
            public const string Admin = "Admin";
            public const string User = "User";
        }

        [HttpPost("appLogin")]
        public async Task<IActionResult> appLogin(AppLoginViewModel appLogin)
        {
            _logger.logInfo($"Login attempt: {appLogin.username}");

            var loginDetails = await _serviceManager.authenticationContract.appLogin(appLogin);

            string token = null;
            object userData = null;

            if (loginDetails.statusCode == (int)HttpStatusCode.OK)
            {
                var userDynamic = (loginDetails.data as IEnumerable<dynamic>)?.FirstOrDefault();

                if (userDynamic == null)
                {
                    return BadRequest("User data invalid");
                }

                var user = new UserDto
                {
                    URID = userDynamic.URID,
                    Username = userDynamic.Email,
                    Role = "User"
                };

                token = GenerateToken(user, user.Role);
                userData = user;

            }
            else
            {
                _logger.logWarn("Invalid login attempt");
            }

            return Ok(new
            {
                token,
                loginDetails.statusCode,
                loginDetails.message,
                //data = userData,
                data = (loginDetails.data as IEnumerable<object>)?.FirstOrDefault()
            });
        }
        private string GenerateToken(UserDto user, string role)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, role),
                new Claim("URID", user.URID.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("adminUserLogin")]
        public async Task<IActionResult> adminUserLogin(AppUserAdminLoginViewModel appUserAdminLoginViewModel)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} appLogin");
            var loginDetails = await _serviceManager.authenticationContract.adminUserLogin(appUserAdminLoginViewModel);
            string token = null;

            if (loginDetails.statusCode == (int)HttpStatusCode.OK)
            {
                token = GenerateTokenForUserNameAdminlogin(appUserAdminLoginViewModel);
            }
            else
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No User Found");
            }
            var response = new
            {
                token,
                loginDetails.statusCode,
                loginDetails.message,
                data = (loginDetails.data as IEnumerable<object>)?.FirstOrDefault()
            };

            return Ok(response);
        }

        private string GenerateTokenForUserNameAdminlogin(AppUserAdminLoginViewModel appUserAdminLoginViewModel)
        {
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.Name, appUserAdminLoginViewModel.username),
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [HttpPost("userRegistration")]
        public async Task<IActionResult> userRegistration(AddAppUserViewModel addAppUser)
        {
            _logger.logInfo($" {LoggingEvents.updateItem} addAppUser");
            var returnData = await _serviceManager.authenticationContract.addAppUser(addAppUser);
            return Ok(returnData);
        }

        [HttpPost("forgotPassword")]
        public async Task<IActionResult> forgotPassword(ForgotPasswordViewModel updatePassword)
        {
            _logger.logInfo($" {LoggingEvents.updateItem} updatePassword");
            var returnData = await _serviceManager.authenticationContract.forgotPassword(updatePassword);
            return Ok(returnData);
        }

        [Authorize(Roles = "User")]
        [HttpPost("changePassword")]

        public async Task<IActionResult> changePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            _logger.logInfo($" {LoggingEvents.updateItem} changePassword");
            var returnData = await _serviceManager.authenticationContract.changePassword(changePasswordViewModel);
            return Ok(returnData);
        }


        [HttpPost("sendOtp")]
        public async Task<IActionResult> sendOtp(SendOtpViewModel sendOtp)
        {
            try
            {
                _logger.logInfo($"{LoggingEvents.getByIdItem} sendOtp");

                var loginDetails = await _serviceManager.authenticationContract.sendOtp(sendOtp);
                if (loginDetails.statusCode == 200)
                {
                    loginDetails.message = "OTP generated successfully.";
                }

                return Ok(loginDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Something went wrong.", error = ex.Message });
            }
        }

        [HttpGet("getByReferralId")]
        public async Task<IActionResult> getByReferralId(string loginId)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getByIdProduct loginId ${loginId}");
            var getByRefreralId = await _serviceManager.authenticationContract.getByReferralId(loginId);
            if (getByRefreralId.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Product Found");
            }
            return Ok(getByRefreralId);
        }

        //[HttpGet("getAllUserRegitration")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> getAllUserRegitration()
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} appLogin");
        //    var loginDetails = await _serviceManager.authenticationContract.GetAllUserRegitration();
        //    return Ok(loginDetails);
        //}



        [HttpGet("userDashboardDetails")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> userDashboardDetails(Guid URID)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} UserDashboardDetails");
            var UserDashboardDetails = await _serviceManager.authenticationContract.UserDashboardDetails(URID);
            return Ok(UserDashboardDetails);
        }
       
 

        [HttpPost("validateOtp")]
        public async Task<IActionResult> validateOtp(ValidateOtpViewModel validateOtpViewModel)
        {
            _logger.logInfo($"{LoggingEvents.getByIdItem} sendOtp");
            var loginDetails = await _serviceManager.authenticationContract.validateOtp(validateOtpViewModel);
            return Ok(loginDetails);

        }


        //[HttpGet("userAffiliateDashboard")]
        //[Authorize]
        //public async Task<IActionResult> userAffiliateDashboard(Guid URID)
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} UserDashboardDetails");
        //    var UserDashboardDetails = await _serviceManager.authenticationContract.UserUserRentelligenceDashboard(URID);
        //    return Ok(UserDashboardDetails);
        //}

        //[HttpGet("getLBRank")]
        //public async Task<IActionResult> getLBRank()
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} UserDashboardDetails");
        //    var UserDashboardDetails = await _serviceManager.authenticationContract.getLBRank();
        //    return Ok(UserDashboardDetails);
        //}

        [HttpPost("sendOtpFundRequest")]
        public async Task<IActionResult> sendOtpFundTransfer(SendOtpFundRequestViewModel sendOtp)
        {
            try
            {
                _logger.logInfo($"{LoggingEvents.getByIdItem} sendOtp");

                var loginDetails = await _serviceManager.authenticationContract.sendOtpRequest(sendOtp);
                if (loginDetails.statusCode == 200)
                {
                    loginDetails.message = "OTP Send Your Regsiterd Email successfully.";
                }

                return Ok(loginDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Something went wrong.", error = ex.Message });
            }
        }

        [HttpPost("sendOtpWithdrawalRequest")]
        public async Task<IActionResult> sendOtpWithdrawalRequest(SendOtpWithdrawalViewModel sendOtp)
        {
            try
            {
                _logger.logInfo($"{LoggingEvents.getByIdItem} sendOtp");

                var loginDetails = await _serviceManager.authenticationContract.sendOtpWithdrawal(sendOtp);
                if (loginDetails.statusCode == 200)
                {
                    loginDetails.message = "OTP Send Your Regsiterd Email successfully.";
                }

                return Ok(loginDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Something went wrong.", error = ex.Message });
            }
        }

        [HttpPost("updateUserProfile")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> updateUserProfile(UpdateUserProfileViewModel updateUserProfile)
        {
            _logger.logInfo($" {LoggingEvents.updateItem} updateUserProfile");
            var updatepro = await _serviceManager.authenticationContract.updateUserProfile(updateUserProfile);
            return Ok(updatepro);
        }

        [HttpPost("updateUserProfileImage")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> updateUserProfileImage(UpdateUserImageViewModel updateUserImageViewModel)
        {
            _logger.logInfo($" {LoggingEvents.updateItem} updateUserProfileImage");
            var updatepro = await _serviceManager.authenticationContract.updateUserProfileImage(updateUserImageViewModel);
            return Ok(updatepro);
        }



        [HttpGet("userSummaryDetails")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> userSummaryDetails(Guid URID)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} UserSummaryDetails");
            var UserSummaryDetails = await _serviceManager.authenticationContract.UserSummaryDetails(URID);
            return Ok(UserSummaryDetails);
        }

        //    [HttpGet("getAgentAnalyticsUser")]
        //    [Authorize]
        //    public async Task<IActionResult> getAgentAnalyticsUser(Guid URID)
        //    {
        //        _logger.logInfo($" {LoggingEvents.getByIdItem} getAgentAnalyticsUser");
        //        var getAgentAnalyticsUser = await _serviceManager.authenticationContract.getAgentAnalyticsUser(URID);
        //        return Ok(getAgentAnalyticsUser);
        //    }

        //   [Authorize]
        //    [HttpPost("sendOtpEvent")]
        //    public async Task<IActionResult> sendOtpEvent(SendOtpFundRequestViewModel sendOtp)
        //    {
        //        try
        //        {
        //            _logger.logInfo($"{LoggingEvents.getByIdItem} sendOtpEvent");

        //            var loginDetails = await _serviceManager.authenticationContract.sendOtpEvent(sendOtp);
        //            if (loginDetails.statusCode == 200)
        //            {
        //                loginDetails.message = "OTP Send Your Regsiterd Email successfully.";
        //            }

        //            return Ok(loginDetails);
        //        }
        //        catch (Exception ex)
        //        {
        //            return StatusCode(500, new { message = "Something went wrong.", error = ex.Message });
        //        }
        //    }
    }
}

