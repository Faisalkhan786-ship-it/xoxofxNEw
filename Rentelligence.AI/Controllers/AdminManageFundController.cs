using Common;
using EmailSystem;
using LoggerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceContract;
using System.Net;
using ViewModel;

namespace Rentelligence.AI.MarketPlace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminManageFundController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILoggerManager _logger;
        private readonly IConfiguration _configuration;
        private readonly EmailService emailService;
        private readonly ExtractToken extractToken;
        public AdminManageFundController(IServiceManager serviceManager, ILoggerManager logger, IConfiguration configuration)
        {
            _serviceManager = serviceManager;
            _logger = logger;
            extractToken = new ExtractToken(configuration);
            emailService = new EmailService(configuration);
            _configuration = configuration;
        }

        [HttpPost("getAllFundRequestReport_Admin")]
        public async Task<IActionResult> getAllFundRequestReport_Admin(UnAppIncomeViewModel appUnAppFundRequestModel)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getAllFundRequestReport_Admin");
            var getAllFundRequestReport_Admin = await _serviceManager.adminManageFundService.getAllFundRequestReport_Admin(appUnAppFundRequestModel);
            return Ok(getAllFundRequestReport_Admin);
        }

        [HttpPost("getAllIncomeRequestReport_Admin")]
        public async Task<IActionResult> getAllIncomeRequestReport_Admin(UnAppIncomeViewModel appUnAppFundRequestModel)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getAllIncomeRequestReport_Admin");
            var getAllIncomeRequestReport_Admin = await _serviceManager.adminManageFundService.getAllUserWithdrawalRequest_Admin(appUnAppFundRequestModel);
            return Ok(getAllIncomeRequestReport_Admin);
        }

        [HttpPost("getAllROIWithdrawalReport_Admin")]
        public async Task<IActionResult> getAllROIWithdrawalReport_Admin(UnAppIncomeViewModel appUnAppFundRequestModel)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getAllROIWithdrawalReport_Admin");
            var getAllROIWithdrawalReport_Admin = await _serviceManager.adminManageFundService.getAllUserROIWithdrawalRequest_Admin(appUnAppFundRequestModel);
            return Ok(getAllROIWithdrawalReport_Admin);
        }

        [HttpPost("updateIncomeWalletAdress")]
        public async Task<IActionResult> updateIncomeWalletAdress(UpdateIncometWalletAdressViewModel updateIncometWalletAdressViewModel)
        {
            _logger.logInfo($" {LoggingEvents.updateItem} updateIncomeWalletAdress");
            var update = await _serviceManager.adminManageFundService.updateIncomeWalletAdress(updateIncometWalletAdressViewModel);
            return Ok(update);
        }
      
        [HttpPost("UpIncomeWithdReqStatus_Admin")]
        public async Task<IActionResult> UpIncomeWithdReqStatus_Admin(AppRejFundViewModel appRejFundViewModel)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} adminLogin");
            var upIncWithdReqStatus_Admin = await _serviceManager.adminManageFundService.upIncWithdReqStatus_Admin(appRejFundViewModel);
            return Ok(upIncWithdReqStatus_Admin);
        }
       

        [HttpPost("updateRoiWalletAdress")]
        public async Task<IActionResult> updateRoiWalletAdress(UpdateIncometWalletAdressViewModel updateIncometWalletAdressViewModel)
        {
            _logger.logInfo($" {LoggingEvents.updateItem} updateRoiWalletAdress");
            var update = await _serviceManager.adminManageFundService.updateRoiWalletAdress(updateIncometWalletAdressViewModel);
            return Ok(update);
        }

        [HttpPost("upROIWithdReqStatus_Admin")]
        public async Task<IActionResult> upROIWithdReqStatus_Admin(AppRejFundViewModel appRejFundViewModel)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} adminLogin");
            var getUserWalletBalance = await _serviceManager.adminManageFundService.upROIWithdReqStatus_Admin(appRejFundViewModel);
            return Ok(getUserWalletBalance);
        }


        [HttpPost("updateFundRequestStatus_Admin")]
        public async Task<IActionResult> updateFundRequestStatus_Admin(AppRejFundViewModel appRejFundViewModel)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} adminLogin");
            var getUserWalletBalance = await _serviceManager.adminManageFundService.updateFundRequestStatus_Admin(appRejFundViewModel);
            return Ok(getUserWalletBalance);
        }

        //[HttpGet("getUserWalletDetails")]
        //public async Task<IActionResult> getUserWalletDetails(string loginId)
        //{
        //    _logger.logInfo($" {LoggingEvents.updateItem} getAdminUserDetails");
        //    var returnData = await _serviceManager.adminManageFundService.getUserWalletDetailsF(loginId);
        //    return Ok(returnData);
        //}


        //[HttpPost("allWalletHistory")]
        //public async Task<IActionResult> allWalletHistory(AllWalletHistoryViewModel allWalletHistoryViewModel)
        //{
        //    _logger.logInfo($" {LoggingEvents.updateItem} getAdminUserDetails");
        //    var returnData = await _serviceManager.adminManageFundService.allWalletHistory(allWalletHistoryViewModel);
        //    return Ok(returnData);
        //}
    }
}
