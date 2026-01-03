using Common;
using EmailSystem;
using LoggerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [Authorize]
    public class FundManagerController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILoggerManager _logger;
        private readonly IConfiguration _configuration;
        private readonly EmailService emailService;
        private readonly ExtractToken extractToken;
        public FundManagerController(IServiceManager serviceManager, ILoggerManager logger, IConfiguration configuration)
        {
            _serviceManager = serviceManager;
            _logger = logger;
            extractToken = new ExtractToken(configuration);
            emailService = new EmailService(configuration);
            _configuration = configuration;
        }


        [HttpPost("addFundRequest")]
        public async Task<IActionResult> addFundRequest(FundManagerViewModel fundManagerViewModel)
        {
            _logger.logInfo($" {LoggingEvents.updateItem} addAdminUser");
            var returnData = await _serviceManager.fundManagerService.addUploadFund(fundManagerViewModel);
            return Ok(returnData);
        }

        [HttpGet("getFundRequestReport")]
        public async Task<IActionResult> getFundRequestReport(Guid URID)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} adminLogin");
            var adminDetails = await _serviceManager.fundManagerService.getUserWalletDetails(URID);
            return Ok(adminDetails);
        }

        [HttpPost("addTransferIncomeToDepositWallet")]
        public async Task<IActionResult> addTransferIncomeToDepositWallet(TransferIncomeToDepositWalletViewModel TransferIncomeToDepositWalletViewModel)
        {
            _logger.logInfo($" {LoggingEvents.updateItem} addAdminUser");
            var add = await _serviceManager.fundManagerService.addTransferIncomeToDepositWallet(TransferIncomeToDepositWalletViewModel);
            return Ok(add);
        }

        [HttpGet("getTransferIncomeToDepositWalletReport")]
        public async Task<IActionResult> getTransferIncomeToDepositWalletReport(Guid URID)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getIncomeToDepositWalletReport");
            var getIncomeToDepositWalletReport = await _serviceManager.fundManagerService.getIncomeToDepositWalletReport(URID);
            return Ok(getIncomeToDepositWalletReport);
        }

        [HttpPost("fundTransferDepositToDeposit")]
        public async Task<IActionResult> fundTransferDepositToDeposit(P2PViewModel P2PViewModel)
        {
            _logger.logInfo($" {LoggingEvents.updateItem} transferP2");
            var add = await _serviceManager.fundManagerService.transferP2(P2PViewModel);
            return Ok(add);
        }

        [HttpGet("getfundTransferDepositToDepositReport")]
        public async Task<IActionResult> getUserWalletBalAndWalletReport(Guid URID)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} adminLogin");
            var getUserWalletBalance = await _serviceManager.fundManagerService.getUserWalletBalance(URID);
            return Ok(getUserWalletBalance);
        }

        [HttpPost("addUserWithdrawalRequest")]
        public async Task<IActionResult> addUserWithdrawalRequest(RequestUserwithdrawalCoin requestUserwithdrawalCoin)
        {
            _logger.logInfo($" {LoggingEvents.updateItem} addAdminUser");
            var add = await _serviceManager.fundManagerService.addRequestUserwithdrawalCoin(requestUserwithdrawalCoin);
            return Ok(add);
        }


        [HttpPost("getAllIncomeRequest_Admin")]
        public async Task<IActionResult> getAllIncomeRequest_Admin(AppUnAppIncomeVideoModel appUnAppIncomeVideoModel)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} adminLogin");
            var getAllUserWithdrawalRequest_Admin = await _serviceManager.fundManagerService.getAllUserWithdrawalRequest_Admin(appUnAppIncomeVideoModel);
            return Ok(getAllUserWithdrawalRequest_Admin);
        }

        [HttpPost("UpIncomeWithdReqStatus_Admin")]
        public async Task<IActionResult> UpIncomeWithdReqStatus_Admin(AppRejFundViewModel appRejFundViewModel)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} adminLogin");
            var upIncWithdReqStatus_Admin = await _serviceManager.fundManagerService.upIncWithdReqStatus_Admin(appRejFundViewModel);
            return Ok(upIncWithdReqStatus_Admin);
        }

        [HttpPost("getAllFundRequestReport_Admin")]
        public async Task<IActionResult> getAllFundRequestReport_Admin(AppUnAppFundRequestModel appUnAppFundRequestModel)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} adminLogin");
            var getAllFundRequestReport_Admin = await _serviceManager.fundManagerService.getAllFundRequestReport_Admin(appUnAppFundRequestModel);
            return Ok(getAllFundRequestReport_Admin);
        }


        [HttpPost("updateFundRequestStatus_Admin")]
        public async Task<IActionResult> updateFundRequestStatus_Admin(AppRejFundViewModel appRejFundViewModel)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} adminLogin");
            var getUserWalletBalance = await _serviceManager.fundManagerService.updateFundRequestStatus_Admin(appRejFundViewModel);
            return Ok(getUserWalletBalance);
        }

        [HttpGet("getUserAutoDeposit")]
        public async Task<IActionResult> getUserAutoDeposit(Guid URID)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getUserAutoDeposit");
            var getUserAutoDeposit = await _serviceManager.fundManagerService.getUserAutoDeposit(URID);
            return Ok(getUserAutoDeposit);
        }


        [HttpPost("addAutoDeposit")]
        public async Task<IActionResult> addAutoDeposit(TokenDepositsViewModel tokenDepositsViewModel)
        {
            _logger.logInfo($" {LoggingEvents.updateItem} addAdminUser");
            var add = await _serviceManager.fundManagerService.addAutoDeposit(tokenDepositsViewModel);
            return Ok(add);
        }

        [HttpPost("upRentWithdReqStatus_Admin")]
        public async Task<IActionResult> upRentWithdReqStatus_Admin(AppRejFundViewModel appRejFundViewModel)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} adminLogin");
            var getUserWalletBalance = await _serviceManager.fundManagerService.upRentWithdReqStatus_Admin(appRejFundViewModel);
            return Ok(getUserWalletBalance);
        }

        [HttpPost("addRechargeTransaction")]
        public async Task<IActionResult> addRechargeTransaction(addRechargeTransactionViewModel addRechargeTransactionViewModel)
        {
            _logger.logInfo($" {LoggingEvents.updateItem} addRechargeTransaction");
            var returnData = await _serviceManager.fundManagerService.addRechargeTransaction(addRechargeTransactionViewModel);
            return Ok(returnData);
        }
        //   public async Task<IActionResult> getUserAutoDeposit(Guid URID)
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} getUserAutoDeposit");
        //    var getUserAutoDeposit = await _serviceManager.fundManagerService.getUserAutoDeposit(URID);
        //    return Ok(getUserAutoDeposit);
        //}
        [HttpPost("getUserPackage")]

        public async Task<IActionResult> getUserPackage()
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getspBindPackageUserSide");
            var getspBindPackageUserSide = await _serviceManager.fundManagerService.getspBindPackageUserSide();
            return Ok(getspBindPackageUserSide);
        }
        [HttpGet("getUserDormantReportDetails")]
        public async Task<IActionResult> getUserDormantReportDetails(Guid URID)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} adminLogin");
            var getUserDormantReportDetails = await _serviceManager.fundManagerService.getUserDormantReportDetails(URID);
            return Ok(getUserDormantReportDetails);
        }
    }
}
