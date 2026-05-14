//using Common;
//using EmailSystem;
//using LoggerService;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using Nethereum.ABI.CompilationMetadata;
//using ServiceContract;
//using System.Data;
//using System.IdentityModel.Tokens.Jwt;
//using System.Net;
//using System.Security.Claims;
//using System.Text;
//using ViewModel;
//using static System.Runtime.InteropServices.JavaScript.JSType;

//namespace Rentelligence.AI.MarketPlace.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AuthenticationController : ControllerBase
//    {
//        private readonly IServiceManager _serviceManager;
//        private readonly ILoggerManager _logger;
//        private readonly IConfiguration _configuration;

//        public AuthenticationController(IServiceManager serviceManager, ILoggerManager logger, IConfiguration configuration)
//        {
//            _serviceManager = serviceManager;
//            _logger = logger;
//            _configuration = configuration;
//        }
//        public class UserDto
//        {
//            public Guid UserId { get; set; }
//            public string Username { get; set; }
//            public string Role { get; set; }
//        }

//        public static class Roles
//        {
//            public const string Admin = "Admin";
//            public const string User = "User";
//        }

//        [HttpPost("appLogin")]
//        public async Task<IActionResult> appLogin(AppLoginViewModel appLogin)
//        {
//            _logger.logInfo($"Login attempt: {appLogin.username}");

//            var loginDetails = await _serviceManager.authenticationContract.appLogin(appLogin);

//            string token = null;
//            object userData = null;

//            if (loginDetails.statusCode == (int)HttpStatusCode.OK)
//            {
//                var userDynamic = (loginDetails.data as IEnumerable<dynamic>)?.FirstOrDefault();

//                if (userDynamic == null)
//                {
//                    return BadRequest("User data invalid");
//                }

//                var user = new UserDto
//                {
//                    UserId = userDynamic.UserId,
//                    Username = userDynamic.Email, 
//                    Role = "User" 
//                };

//                token = GenerateToken(user, user.Role);
//                userData = user;
               
//            }
//            else
//            {
//                _logger.logWarn("Invalid login attempt");
//            }

//            return Ok(new
//            {
//                token,
//                loginDetails.statusCode,
//                loginDetails.message,
//                //data = userData,
//                data = (loginDetails.data as IEnumerable<object>)?.FirstOrDefault()
//        });
//        }
//        private string GenerateToken(UserDto user, string role)
//        {
//            var claims = new[]
//            {
//                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
//                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//                new Claim(ClaimTypes.Name, user.Username),
//                new Claim(ClaimTypes.Role, role),
//                new Claim("UserId", user.UserId.ToString())
//            };

//            var key = new SymmetricSecurityKey(
//                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
//            );

//            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//            var token = new JwtSecurityToken(
//                issuer: _configuration["Jwt:Issuer"],
//                audience: _configuration["Jwt:Audience"],
//                claims: claims,
//                expires: DateTime.Now.AddDays(30),
//                signingCredentials: creds
//            );

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }
//        [HttpPost("userRegistration")]
//        public async Task<IActionResult> userRegistration(AddAppUserViewModel addAppUser)
//        {
//            var result = await _serviceManager.authenticationContract.addAppUser(addAppUser);
//            return Ok(result);
//        }

//        [HttpPost("forgotPassword")]
//        public async Task<IActionResult> forgotPassword(ForgotPasswordViewModel updatePassword)
//        {
//            var result = await _serviceManager.authenticationContract.forgotPassword(updatePassword);
//            return Ok(result);
//        }
//        [HttpPost("VerifyLoginid")]
//        public async Task<IActionResult> VerifyLoginid(verifyloginidViewModel verifyloginid)
//        {
//            var result = await _serviceManager.authenticationContract.VerifyLoginid(verifyloginid);
//            return Ok(result);
//        }


//        [HttpPost("sendOtp")]
//        public async Task<IActionResult> sendOtp(SendOtpViewModel sendOtp)
//        {
//            try
//            {
//                _logger.logInfo($"{LoggingEvents.getByIdItem} sendOtp");

//                var loginDetails = await _serviceManager.authenticationContract.sendOtp(sendOtp);
//                if (loginDetails.statusCode == 200)
//                {
//                    loginDetails.message = "OTP generated successfully.";
//                }

//                return Ok(loginDetails);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { message = "Something went wrong.", error = ex.Message });
//            }
//        }

//        [HttpPost("validateOtp")]
//        public async Task<IActionResult> validateOtp(ValidateOtpViewModel validateOtpViewModel)
//        {

//            _logger.logInfo($"{LoggingEvents.getByIdItem} sendOtp");
//            var loginDetails = await _serviceManager.authenticationContract.validateOtp(validateOtpViewModel);
//            return Ok(loginDetails);

//        }
//        [HttpPost("validateOtpbyEmail")]
//        public async Task<IActionResult> validateOtpbyEmail(ValidateOtpViewModelbyemail validateOtpViewModelbyemail)
//        {

//            _logger.logInfo($"{LoggingEvents.getByIdItem} sendOtp");
//            var loginDetails = await _serviceManager.authenticationContract.validateOtpbyEmail(validateOtpViewModelbyemail);
//            return Ok(loginDetails);

//        }
//        [HttpGet("userDashboardDetails")]
//        public async Task<IActionResult> userDashboardDetails(Guid URID)
//        {
//            _logger.logInfo($" {LoggingEvents.getByIdItem} UserDashboardDetails");
//            var UserDashboardDetails = await _serviceManager.authenticationContract.UserDashboardDetails(URID);
//            return Ok(UserDashboardDetails);
//        }
//        [HttpGet("getTransactionLog")]
//        public async Task<IActionResult> getTransactionLog(Guid URID)
//        {
//            _logger.logInfo($" {LoggingEvents.getByIdItem} getTransactionLog");
//            var getTransactionLog = await _serviceManager.authenticationContract.getTransactionLog(URID);
//            return Ok(getTransactionLog);
//        }
//        [HttpGet("getABREngine")]
//        public async Task<IActionResult> getABREngine(Guid URID)
//        {
//            _logger.logInfo($" {LoggingEvents.getByIdItem} getABREngine");
//            var getABREngine = await _serviceManager.authenticationContract.getABREngine(URID);
//            return Ok(getABREngine);
//        }
//        [HttpGet("getUserAnalytics")]
//        public async Task<IActionResult> getUserAnalytics(Guid URID)
//        {
//            _logger.logInfo($" {LoggingEvents.getByIdItem} getUserAnalytics");
//            var getUserAnalytics = await _serviceManager.authenticationContract.getUserAnalytics(URID);
//            return Ok(getUserAnalytics);
//        }
//        [HttpGet("getUserLinkedIds")]
//        public async Task<IActionResult> getUserLinkedIds(Guid URID)
//        {
//            _logger.logInfo($" {LoggingEvents.getByIdItem} getUserLinkedIds");
//            var getUserLinkedIds = await _serviceManager.authenticationContract.getUserLinkedIds(URID);
//            return Ok(getUserLinkedIds);
//        }
//    }
//}


////using Common;
////using EmailSystem;
////using LoggerService;
////using Microsoft.AspNetCore.Authorization;
////using Microsoft.AspNetCore.Mvc;
////using Microsoft.IdentityModel.Tokens;
////using ServiceContract;
////using System.IdentityModel.Tokens.Jwt;
////using System.Net;
////using System.Security.Claims;
////using System.Text;
////using ViewModel;

////namespace Rentelligence.AI.MarketPlace.Controllers
////{
////    [Route("api/[controller]")]
////    [ApiController]
////    public class AuthenticationController : ControllerBase
////    {
////        private readonly IServiceManager _serviceManager;
////        private readonly ILoggerManager _logger;
////        private readonly IConfiguration _configuration;
////        private readonly EmailService emailService;
////        private readonly ExtractToken extractToken;
////        public AuthenticationController(IServiceManager serviceManager, ILoggerManager logger, IConfiguration configuration)
////        {
////            _serviceManager = serviceManager;
////            _logger = logger;
////            _configuration = configuration;
////            emailService = new EmailService(configuration);
////            extractToken = new ExtractToken(configuration);
////        }

////        [HttpPost("appLogin")]
////        public async Task<IActionResult> appLogin(AppLoginViewModel appLogin)
////        {
////            _logger.logInfo($" {LoggingEvents.getByIdItem} appLogin");
////            var loginDetails = await _serviceManager.authenticationContract.appLogin(appLogin);
////            string token = null;

////            if (loginDetails.statusCode == (int)HttpStatusCode.OK)
////            {
////                token = GenerateTokenForUserName(appLogin);
////            }
////            else
////            {
////                _logger.logWarn($"{LoggingEvents.getItemNotFound},No User Found");
////            }
////            var response = new
////            {
////                token,
////                loginDetails.statusCode,
////                loginDetails.message,
////                data = (loginDetails.data as IEnumerable<object>)?.FirstOrDefault()
////            };

////            return Ok(response);
////        }


////        private string GenerateTokenForUserName(AppLoginViewModel login)
////        {
////            var claims = new[]
////            {
////        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
////        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
////        new Claim(ClaimTypes.Name, login.username),  
////    };

////            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
////            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

////            var token = new JwtSecurityToken(
////                _configuration["Jwt:Issuer"],
////                _configuration["Jwt:Audience"],
////                claims: claims,
////                expires: DateTime.Now.AddDays(30),
////                signingCredentials: signIn);

////            return new JwtSecurityTokenHandler().WriteToken(token);
////        }


////        [HttpPost("userRegistration")]
////        public async Task<IActionResult> userRegistration(AddAppUserViewModel addAppUser)
////        {
////            _logger.logInfo($" {LoggingEvents.updateItem} addAppUser");
////            var returnData = await _serviceManager.authenticationContract.addAppUser(addAppUser);
////            return Ok(returnData);
////        }

////        [HttpPost("forgotPassword")]
////        public async Task<IActionResult> forgotPassword(ForgotPasswordViewModel updatePassword)
////        {
////            _logger.logInfo($" {LoggingEvents.updateItem} updatePassword");
////            var returnData = await _serviceManager.authenticationContract.forgotPassword(updatePassword);
////            return Ok(returnData);
////        }

////        //[HttpPost("changePassword")]

////        //public async Task<IActionResult> changePassword(ChangePasswordViewModel changePasswordViewModel)
////        //{
////        //    _logger.logInfo($" {LoggingEvents.updateItem} changePassword");
////        //    var returnData = await _serviceManager.authenticationContract.changePassword(changePasswordViewModel);
////        //    return Ok(returnData);
////        //}




////        //[HttpPost("sendOtp")]
////        //public async Task<IActionResult> sendOtp(SendOtpViewModel sendOtp)
////        //{
////        //    try
////        //    {
////        //        _logger.logInfo($"{LoggingEvents.getByIdItem} sendOtp");

////        //        var loginDetails = await _serviceManager.authenticationContract.sendOtp(sendOtp);
////        //        if (loginDetails.statusCode == 200)
////        //        {
////        //            loginDetails.message = "OTP generated successfully.";
////        //        }

////        //        return Ok(loginDetails);
////        //    }
////        //    catch (Exception ex)
////        //    {
////        //        return StatusCode(500, new { message = "Something went wrong.", error = ex.Message });
////        //    }
////        //}



////        //[HttpGet("userDashboardDetails")]
////        //public async Task<IActionResult> userDashboardDetails(Guid URID)
////        //{
////        //    _logger.logInfo($" {LoggingEvents.getByIdItem} UserDashboardDetails");
////        //    var UserDashboardDetails = await _serviceManager.authenticationContract.UserDashboardDetails(URID);
////        //    return Ok(UserDashboardDetails);
////        //}



////        //[HttpPost("validateOtp")]
////        //public async Task<IActionResult> validateOtp(ValidateOtpViewModel validateOtpViewModel)
////        //{

////        //    _logger.logInfo($"{LoggingEvents.getByIdItem} sendOtp");
////        //    var loginDetails = await _serviceManager.authenticationContract.validateOtp(validateOtpViewModel);
////        //    return Ok(loginDetails);

////        //}


////        //[HttpPost("updateUserProfile")]
////        //[Authorize]

////        //public async Task<IActionResult> updateUserProfile(UpdateUserProfileViewModel updateUserProfile)
////        //{
////        //    _logger.logInfo($" {LoggingEvents.updateItem} changePassword");
////        //    var updatepro = await _serviceManager.authenticationContract.updateUserProfile(updateUserProfile);
////        //    return Ok(updatepro);
////        //}

////        //[HttpPost("updateUserProfileImage")]
////        //[Authorize]

////        //public async Task<IActionResult> updateUserProfileImage(UpdateUserImageViewModel updateUserImageViewModel)
////        //{
////        //    _logger.logInfo($" {LoggingEvents.updateItem} changePassword");
////        //    var updatepro = await _serviceManager.authenticationContract.updateUserProfileImage(updateUserImageViewModel);
////        //    return Ok(updatepro);
////        //}       
////    }
////}

