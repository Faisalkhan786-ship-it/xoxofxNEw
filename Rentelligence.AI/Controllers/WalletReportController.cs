using Common;
using Dapper;
using EmailSystem;

using LoggerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceContract;
using System;
using System;
using System.IO;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Threading.Tasks;
using ViewModel;



namespace Rentelligence.AI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class WalletReportController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILoggerManager _logger;
        private readonly IConfiguration _configuration;
        private readonly EmailService emailService;
        private readonly ExtractToken extractToken;
        public WalletReportController(IServiceManager serviceManager, ILoggerManager logger, IConfiguration configuration)
        {
            _serviceManager = serviceManager;
            _logger = logger;
            _configuration = configuration;
            emailService = new EmailService(configuration);
            extractToken = new ExtractToken(configuration);
        }
        [HttpPost("getIncomeAndDepositTransType")]
        [Authorize]
        public async Task<IActionResult> getIncomeAndDepositTransType(Guid URID)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} appLogin");
            var getIncomeAndDepositTransType = await _serviceManager.walletReportService.getIncomeAndDepositTransType(URID);
            return Ok(getIncomeAndDepositTransType);
        }

        [HttpPost("getIncomeWalletReport")]
        //[Authorize]
        public async Task<IActionResult> getIncomeWalletReport(WalletReportViewModel walletReportViewModel)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} appLogin");
            var getIncomeWalletWallerReport = await _serviceManager.walletReportService.getIncomeWalletWallerReport(walletReportViewModel);
            return Ok(getIncomeWalletWallerReport);
        }

        [HttpPost("getDepositWalletReport")]
        //[Authorize]
        public async Task<IActionResult> getDepositWalletReport(DepositReportViewModel depositReportViewModel)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} appLogin");
            var getDepositWalletReport = await _serviceManager.walletReportService.getDepositWalletReport(depositReportViewModel);
            return Ok(getDepositWalletReport);
        }

        [HttpPost("getIncomeWithdrawalHistory")]
        //[Authorize]
        public async Task<IActionResult> getIncomeWithdrawalHistory(IncomeWithdrawalHistoryViewModel incomeWithdrawalHistoryViewModel)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} appLogin");
            var getIncomeWithdrawalHistory = await _serviceManager.walletReportService.getIncomeWithdrawalHistory(incomeWithdrawalHistoryViewModel);
            return Ok(getIncomeWithdrawalHistory);
        }

        [HttpPost("getRechargeTransactByTId")]
        //[Authorize]
        public async Task<IActionResult> getRechargeTransactByTId(Guid URID)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} appLogin");
            var getRechargeTransact = await _serviceManager.walletReportService.getRechargeTransact(URID);
            return Ok(getRechargeTransact);
        }

        [HttpPost("addRechargeTransact")]
        // [Authorize]
        public async Task<IActionResult> addRechargeTransact(AddRechargeTransactionViewModel addRechargeTransactionViewModel)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} appLogin");
            var addRechargeTransact = await _serviceManager.walletReportService.addRechargeTransact(addRechargeTransactionViewModel);
            return Ok(addRechargeTransact);
        }

        [HttpPost("getRentWalletByURID")]
        // [Authorize]
        public async Task<IActionResult> getRentWalletByURID(Guid URID)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} appLogin");
            var getRentWalletByURID = await _serviceManager.walletReportService.getRentWalletByURID(URID);
            return Ok(getRentWalletByURID);
        }

        [HttpPost("getNetworkTree")]
        public async Task<IActionResult> getNetworkTree(string authlogin)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} appLogin");
            var getRentWalletByURID = await _serviceManager.walletReportService.getNetworkTree(authlogin);
            return Ok(getRentWalletByURID);
        }

        [HttpPost("getRentWalletWallerReport")]
        // [Authorize]
        public async Task<IActionResult> getRentWalletWallerReport(RentWalletReportViewModel rentWalletReportViewModel)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getRentWalletWallerReport");
            var getRentWalletWallerReport = await _serviceManager.walletReportService.getRentWalletWallerReport(rentWalletReportViewModel);
            return Ok(getRentWalletWallerReport);
        }

        [HttpPost("getLeaderShipbyURID")]
        [Authorize]
        public async Task<IActionResult> getLeaderShipbyURID(Guid URID)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getLeaderShipbyURID");
            var getLeaderShipbyURID = await _serviceManager.walletReportService.getleaderShipURID(URID);
            return Ok(getLeaderShipbyURID);
        }

        [HttpPost("getPerformanceRewardListByURID")]
        //  [Authorize]
        public async Task<IActionResult> getPerformanceRewardListByURID(Guid URID)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getLeaderShipbyURID");
            var getLeaderShipbyURID = await _serviceManager.walletReportService.getPerformanceRewardList(URID);
            return Ok(getLeaderShipbyURID);
        }


        [HttpPost("getTransactionHistory")]
        // [Authorize]
        public async Task<IActionResult> getTransactionHistory(IncomeWithdrawalHistoryViewModel incomeWithdrawalHistoryViewModel)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} appLogin");
            var getIncomeWithdrawalHistory = await _serviceManager.walletReportService.getTransactionHistory(incomeWithdrawalHistoryViewModel);
            return Ok(getIncomeWithdrawalHistory);
        }

        [HttpPost("updateRentWalletAdress")]
        // [Authorize]
        public async Task<IActionResult> updateRentWalletAdress(UpdateRentWalletAdressViewModel updateRentWalletAdressViewModel)
        {
            _logger.logInfo($" {LoggingEvents.updateItem} updateRentWalletAdress");
            var update = await _serviceManager.walletReportService.updateRentWalletAdress(updateRentWalletAdressViewModel);
            return Ok(update);
        }

        [HttpPost("updateIncomeWalletAdress")]
        // [Authorize]
        public async Task<IActionResult> updateIncomeWalletAdress(UpdateIncometWalletAdressViewModel updateIncometWalletAdressViewModel)
        {
            _logger.logInfo($" {LoggingEvents.updateItem} updateIncomeWalletAdress");
            var update = await _serviceManager.walletReportService.updateIncomeWalletAdress(updateIncometWalletAdressViewModel);
            return Ok(update);
        }

        [HttpPost("getAccStatemtnt")]
        // [Authorize]
        public async Task<IActionResult> getAccStatemtnt(accStateMent accStateMent)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} appLogin");
            var getIncomeWithdrawalHistory = await _serviceManager.walletReportService.getAccStatemtnt(accStateMent);
            return Ok(getIncomeWithdrawalHistory);
        }

        [HttpPost("getAllWalletHistory")]
        // [Authorize]
        public async Task<IActionResult> getAllWalletHistory(AllWalletHistory allWalletHistory)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getAllWalletHistory");
            var getAllWalletHistory = await _serviceManager.walletReportService.getAllWalletHistory(allWalletHistory);
            return Ok(getAllWalletHistory);
        }

        [HttpPost("getRechargeTransactionAdmin")]
        //  [Authorize]
        public async Task<IActionResult> getRechargeTransactionAdmin(RechargeTransactionAdminViewModel rechargeTransactionAdminViewModel)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} appLogin");
            var getRechargeTransactionAdmin = await _serviceManager.walletReportService.getRechargeTransactionAdmin(rechargeTransactionAdminViewModel);
            return Ok(getRechargeTransactionAdmin);
        }
        [HttpPost("addRechargeTransactionAdmin")]
        //  [Authorize]
        public async Task<IActionResult> addRechargeTransactionAdmin(AddRechargeTransactionAdminViewModel addRechargeTransactionAdminViewModel)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} appLogin");
            var addRechargeTransactionAdmin = await _serviceManager.walletReportService.addRechargeTransactionAdmin(addRechargeTransactionAdminViewModel);
            return Ok(addRechargeTransactionAdmin);
        }

        [HttpPost("addRechargeTransactionUser")]
         [Authorize]
        public async Task<IActionResult> addRechargeTransactionUser(AddRechargeTransactionUserViewModel addRechargeTransactionUserViewModel)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} appLogin");
            var addRechargeTransactionUser = await _serviceManager.walletReportService.addRechargeTransactionUser(addRechargeTransactionUserViewModel);
            return Ok(addRechargeTransactionUser);
        }
        [HttpPost("getBindBuyPackageList")]
        [Authorize]
        public async Task<IActionResult> getBindBuyPackageList(Guid URID)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getBindBuyPackageList");
            var getBindBuyPackageList = await _serviceManager.walletReportService.getBindBuyPackageList(URID);
            return Ok(getBindBuyPackageList);
        }
    }
}
