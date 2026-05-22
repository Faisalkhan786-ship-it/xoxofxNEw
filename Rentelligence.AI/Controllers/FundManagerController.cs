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
using static Model.ModelType;

namespace XoxoFX_Apis.AI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "User")]
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
            _logger.logInfo($" {LoggingEvents.updateItem} addFundRequest");
            var returnData = await _serviceManager.fundManagerService.addUploadFund(fundManagerViewModel);
            return Ok(returnData);
        }

        [HttpGet("getFundRequestReport")]
        public async Task<IActionResult> getFundRequestReport(Guid URID)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getFundRequestReport");
            var adminDetails = await _serviceManager.fundManagerService.getUserWalletDetails(URID);
            return Ok(adminDetails);
        }

        [HttpPost("addUserWithdrawalRequest")]
        public async Task<IActionResult> addUserWithdrawalRequest(RequestUserwithdrawalCoin requestUserwithdrawalCoin)
        {
            _logger.logInfo($" {LoggingEvents.updateItem} addUserWithdrawalRequest");
            var add = await _serviceManager.fundManagerService.addRequestUserwithdrawalCoin(requestUserwithdrawalCoin);
            return Ok(add);
        }

        [HttpPost("addTransferIncomeToDepositWallet")]
        public async Task<IActionResult> addTransferIncomeToDepositWallet(TransferIncomeToDepositWalletViewModel TransferIncomeToDepositWalletViewModel)
        {
            _logger.logInfo($" {LoggingEvents.updateItem} addAdminUser");
            var add = await _serviceManager.fundManagerService.addTransferIncomeToDepositWallet(TransferIncomeToDepositWalletViewModel);
            return Ok(add);
        }

        [HttpPost("fundTransferDepositToDeposit")]
        public async Task<IActionResult> fundTransferDepositToDeposit(P2PViewModel P2PViewModel)
        {
            _logger.logInfo($" {LoggingEvents.updateItem} transferP2");
            var add = await _serviceManager.fundManagerService.transferP2(P2PViewModel);
            return Ok(add);
        }

        [HttpGet("getTransferIncomeToDepositWalletReport")]
        public async Task<IActionResult> getTransferIncomeToDepositWalletReport(Guid URID)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getIncomeToDepositWalletReport");
            var getIncomeToDepositWalletReport = await _serviceManager.fundManagerService.getIncomeToDepositWalletReport(URID);
            return Ok(getIncomeToDepositWalletReport);
        }

        [HttpGet("getfundTransferDepositToDepositReport")]
        public async Task<IActionResult> getfundTransferDepositToDepositReport(Guid URID)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} adminLogin");
            var getUserWalletBalance = await _serviceManager.fundManagerService.getUserWalletBalance(URID);
            return Ok(getUserWalletBalance);
        }
        [HttpPost("addRechargeTransactionUser")]
        public async Task<IActionResult> addRechargeTransactionUser(AddRechargeTransactionUserViewModel addRechargeTransactionUserViewModel)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} addRechargeTransactionUser");
            var addRechargeTransactionUser = await _serviceManager.fundManagerService.addRechargeTransactionUser(addRechargeTransactionUserViewModel);
            return Ok(addRechargeTransactionUser);
        }

        [HttpGet("getRechargeTransactionURID")]
        public async Task<IActionResult> getRechargeTransactionURID(Guid URID)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getRechargeTransaction URID ${URID}");
            var getRechargeTransaction = await _serviceManager.fundManagerService.getRechargeTransaction(URID);
            if (getRechargeTransaction.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Recharge Found");
            }
            return Ok(getRechargeTransaction);
        }
        //[HttpGet("getUserAutoDeposit")]
        //public async Task<IActionResult> getUserAutoDeposit(Guid URID)
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} getUserAutoDeposit");
        //    var getUserAutoDeposit = await _serviceManager.fundManagerService.getUserAutoDeposit(URID);
        //    return Ok(getUserAutoDeposit);
        //}


        //[HttpPost("addAutoDeposit")]
        //public async Task<IActionResult> addAutoDeposit(TokenDepositsViewModel tokenDepositsViewModel)
        //{
        //    _logger.logInfo($" {LoggingEvents.updateItem} addAdminUser");
        //    var add = await _serviceManager.fundManagerService.addAutoDeposit(tokenDepositsViewModel);
        //    return Ok(add);
        //}



        //[HttpPost("addRechargeTransaction")]
        //public async Task<IActionResult> addRechargeTransaction(addRechargeTransactionViewModel addRechargeTransactionViewModel)
        //{
        //    _logger.logInfo($" {LoggingEvents.updateItem} addRechargeTransaction");
        //    var returnData = await _serviceManager.fundManagerService.addRechargeTransaction(addRechargeTransactionViewModel);
        //    return Ok(returnData);
        //}
        //   public async Task<IActionResult> getUserAutoDeposit(Guid URID)
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} getUserAutoDeposit");
        //    var getUserAutoDeposit = await _serviceManager.fundManagerService.getUserAutoDeposit(URID);
        //    return Ok(getUserAutoDeposit);
        //}
        //[HttpPost("getUserPackage")]

        //public async Task<IActionResult> getUserPackage()
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} getspBindPackageUserSide");
        //    var getspBindPackageUserSide = await _serviceManager.fundManagerService.getspBindPackageUserSide();
        //    return Ok(getspBindPackageUserSide);
        //}



    }
}
