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



namespace XoxoFX_Apis.AI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
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
        [HttpGet("getAllWalletTransType")]
        public async Task<IActionResult> getAllWalletTransType(Guid URID)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} appLogin");
            var getIncomeAndDepositTransType = await _serviceManager.walletReportService.getIncomeAndDepositTransType(URID);
            return Ok(getIncomeAndDepositTransType);
        }

        [HttpPost("getDepositWalletReport")]
        public async Task<IActionResult> getDepositWalletReport(DepositReportViewModel depositReportViewModel)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getDepositWalletReport");
            var getDepositWalletReport = await _serviceManager.walletReportService.getDepositWalletReport(depositReportViewModel);
            return Ok(getDepositWalletReport);
        }

        [HttpPost("getIncomeWalletReport")]
        public async Task<IActionResult> getIncomeWalletReport(WalletReportViewModel walletReportViewModel)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getIncomeWalletReport");
            var getIncomeWalletWallerReport = await _serviceManager.walletReportService.getIncomeWalletWallerReport(walletReportViewModel);
            return Ok(getIncomeWalletWallerReport);
        }

        [HttpPost("getROIWalletWallerReport")]
        public async Task<IActionResult> getROIWalletWallerReport(ROIWalletReportViewModel rOIWalletReportViewModel)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getROIWalletWallerReport");
            var getROIWalletWallerReport = await _serviceManager.walletReportService.getROIWalletWallerReport(rOIWalletReportViewModel);
            return Ok(getROIWalletWallerReport);
        }
        
        [HttpPost("getWithdrawalHistory")]
        public async Task<IActionResult> getWithdrawalHistory(IncomeWithdrawalHistoryViewModel1 incomeWithdrawalHistoryViewModel)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} appLogin");
            var getIncomeWithdrawalHistory = await _serviceManager.walletReportService.getIncomeWithdrawalHistory(incomeWithdrawalHistoryViewModel);
            return Ok(getIncomeWithdrawalHistory);
        }

        [HttpGet("getdownLineTreeDetails")]
        public async Task<IActionResult> getdownLineTreeDetails(Guid URID)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getdownLineTreeDetails");
            var getdownLineTreeDetails = await _serviceManager.walletReportService.getdownLineTreeDetails(URID);
            return Ok(getdownLineTreeDetails);
        }
       
        [HttpPost("getDownlineLeftRightCount")]
        public async Task<IActionResult> getDownlineLeftRightCount(DownlineLeftRightCountViewModel downlineLeftRightCountViewModel)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getDownlineLeftRightCount");
            var getDownlineLeftRightCount = await _serviceManager.walletReportService.getDownlineLeftRightCount(downlineLeftRightCountViewModel);
            return Ok(getDownlineLeftRightCount);
        }

        [HttpPost("getLeftRightdownline")]
        public async Task<IActionResult> getLeftRightdownline(LeftRightdownlineTeamViewModel leftRightdownlineTeamViewModel)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getLeftRightdownline");
            var getLeftRightdownline = await _serviceManager.walletReportService.getLeftRightdownline(leftRightdownlineTeamViewModel);
            return Ok(getLeftRightdownline);
        }

        //[HttpPost("getTransactionHistory")]
        //public async Task<IActionResult> getTransactionHistory(IncomeWithdrawalHistoryViewModel incomeWithdrawalHistoryViewModel)
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} getTransactionHistory");
        //    var getIncomeWithdrawalHistory = await _serviceManager.walletReportService.getTransactionHistory(incomeWithdrawalHistoryViewModel);
        //    return Ok(getIncomeWithdrawalHistory);
        //}

        //[HttpPost("getRechargeTransactByTId")]
        //public async Task<IActionResult> getRechargeTransactByTId(Guid URID)
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} appLogin");
        //    var getRechargeTransact = await _serviceManager.walletReportService.getRechargeTransact(URID);
        //    return Ok(getRechargeTransact);
        //}

        //[HttpPost("addRechargeTransact")]
        //public async Task<IActionResult> addRechargeTransact(AddRechargeTransactionViewModel addRechargeTransactionViewModel)
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} appLogin");
        //    var addRechargeTransact = await _serviceManager.walletReportService.addRechargeTransact(addRechargeTransactionViewModel);
        //    return Ok(addRechargeTransact);
        //}

        //[HttpPost("getRentWalletByURID")]
        //public async Task<IActionResult> getRentWalletByURID(Guid URID)
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} appLogin");
        //    var getRentWalletByURID = await _serviceManager.walletReportService.getRentWalletByURID(URID);
        //    return Ok(getRentWalletByURID);
        //}

        //[HttpPost("getNetworkTree")]
        //public async Task<IActionResult> getNetworkTree(string authlogin)
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} appLogin");
        //    var getRentWalletByURID = await _serviceManager.walletReportService.getNetworkTree(authlogin);
        //    return Ok(getRentWalletByURID);
        //}

        //[HttpPost("getRentWalletWallerReport")]
        //// [Authorize]
        //public async Task<IActionResult> getRentWalletWallerReport(RentWalletReportViewModel rentWalletReportViewModel)
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} getRentWalletWallerReport");
        //    var getRentWalletWallerReport = await _serviceManager.walletReportService.getRentWalletWallerReport(rentWalletReportViewModel);
        //    return Ok(getRentWalletWallerReport);
        //}

        //[HttpPost("getLeaderShipbyURID")]
        //[Authorize]
        //public async Task<IActionResult> getLeaderShipbyURID(Guid URID)
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} getLeaderShipbyURID");
        //    var getLeaderShipbyURID = await _serviceManager.walletReportService.getleaderShipURID(URID);
        //    return Ok(getLeaderShipbyURID);
        //}

        //[HttpPost("getPerformanceRewardListByURID")]
        ////  [Authorize]
        //public async Task<IActionResult> getPerformanceRewardListByURID(Guid URID)
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} getLeaderShipbyURID");
        //    var getLeaderShipbyURID = await _serviceManager.walletReportService.getPerformanceRewardList(URID);
        //    return Ok(getLeaderShipbyURID);
        //}


        //[HttpPost("getTransactionHistory")]
        //// [Authorize]
        //public async Task<IActionResult> getTransactionHistory(IncomeWithdrawalHistoryViewModel incomeWithdrawalHistoryViewModel)
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} appLogin");
        //    var getIncomeWithdrawalHistory = await _serviceManager.walletReportService.getTransactionHistory(incomeWithdrawalHistoryViewModel);
        //    return Ok(getIncomeWithdrawalHistory);
        //}

        //[HttpPost("updateRentWalletAdress")]
        //// [Authorize]
        //public async Task<IActionResult> updateRentWalletAdress(UpdateRentWalletAdressViewModel updateRentWalletAdressViewModel)
        //{
        //    _logger.logInfo($" {LoggingEvents.updateItem} updateRentWalletAdress");
        //    var update = await _serviceManager.walletReportService.updateRentWalletAdress(updateRentWalletAdressViewModel);
        //    return Ok(update);
        //}

        //[HttpPost("updateIncomeWalletAdress")]
        //// [Authorize]
        //public async Task<IActionResult> updateIncomeWalletAdress(UpdateIncometWalletAdressViewModel updateIncometWalletAdressViewModel)
        //{
        //    _logger.logInfo($" {LoggingEvents.updateItem} updateIncomeWalletAdress");
        //    var update = await _serviceManager.walletReportService.updateIncomeWalletAdress(updateIncometWalletAdressViewModel);
        //    return Ok(update);
        //}

        //[HttpPost("getAccStatemtnt")]
        //// [Authorize]
        //public async Task<IActionResult> getAccStatemtnt(accStateMent accStateMent)
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} appLogin");
        //    var getIncomeWithdrawalHistory = await _serviceManager.walletReportService.getAccStatemtnt(accStateMent);
        //    return Ok(getIncomeWithdrawalHistory);
        //}

        //[HttpPost("getAllWalletHistory")]
        //// [Authorize]
        //public async Task<IActionResult> getAllWalletHistory(AllWalletHistory allWalletHistory)
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} getAllWalletHistory");
        //    var getAllWalletHistory = await _serviceManager.walletReportService.getAllWalletHistory(allWalletHistory);
        //    return Ok(getAllWalletHistory);
        //}

        //[HttpPost("getRechargeTransactionAdmin")]
        ////  [Authorize]
        //public async Task<IActionResult> getRechargeTransactionAdmin(RechargeTransactionAdminViewModel rechargeTransactionAdminViewModel)
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} appLogin");
        //    var getRechargeTransactionAdmin = await _serviceManager.walletReportService.getRechargeTransactionAdmin(rechargeTransactionAdminViewModel);
        //    return Ok(getRechargeTransactionAdmin);
        //}
        //[HttpPost("addRechargeTransactionAdmin")]
        ////  [Authorize]
        //public async Task<IActionResult> addRechargeTransactionAdmin(AddRechargeTransactionAdminViewModel addRechargeTransactionAdminViewModel)
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} addRechargeTransactionAdmin");
        //    var addRechargeTransactionAdmin = await _serviceManager.walletReportService.addRechargeTransactionAdmin(addRechargeTransactionAdminViewModel);
        //    return Ok(addRechargeTransactionAdmin);
        //}

        //[HttpPost("addRechargeTransactionUser")]
        //[Authorize]
        //public async Task<IActionResult> addRechargeTransactionUser(AddRechargeTransactionUserViewModel addRechargeTransactionUserViewModel)
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} addRechargeTransactionUser");
        //    var addRechargeTransactionUser = await _serviceManager.walletReportService.addRechargeTransactionUser(addRechargeTransactionUserViewModel);
        //    return Ok(addRechargeTransactionUser);
        //}
        //[HttpPost("getBindBuyPackageList")]
        //[Authorize]
        //public async Task<IActionResult> getBindBuyPackageList(Guid URID)
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} getBindBuyPackageList");
        //    var getBindBuyPackageList = await _serviceManager.walletReportService.getBindBuyPackageList(URID);
        //    return Ok(getBindBuyPackageList);
        //}
        //[HttpPost("getSingleLeg_Report")]
        //[Authorize]
        //public async Task<IActionResult> getSingleLeg_Report(String AuthLogin)
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} getSingleLeg_Report");
        //    var getSingleLeg_Report = await _serviceManager.walletReportService.getSingleLeg_Report(AuthLogin);
        //    return Ok(getSingleLeg_Report);
        //}
        //[HttpPost("getUserAllWalletBalance")]
        //[Authorize]
        //public async Task<IActionResult> getUserAllWalletBalance(Guid URID)
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} getUserAllWalletBalance");
        //    var getUserAllWalletBalance = await _serviceManager.walletReportService.getUserAllWalletBalance(URID);
        //    return Ok(getUserAllWalletBalance);
        //}

        //[HttpPost("genrateROI_BOTCLICK")]
        //[Authorize]
        //public async Task<IActionResult> genrateROI_BOTCLICK(Guid URID)
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} genrateROI_BOTCLICK");
        //    var genrateROI_BOTCLICK = await _serviceManager.walletReportService.genrateROI_BOTCLICK(URID);
        //    return Ok(genrateROI_BOTCLICK);
        //}

        //[HttpPost("checkROI_BOTCLICK")]
        //[Authorize]
        //public async Task<IActionResult> checkROI_BOTCLICK(Guid URID)
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} checkROI_BOTCLICK");
        //    var checkROI_BOTCLICK = await _serviceManager.walletReportService.checkROI_BOTCLICK(URID);
        //    return Ok(checkROI_BOTCLICK);
        //}

        //[HttpPost("getSettings")]
        ////[Authorize]
        //public async Task<IActionResult> getSettings()
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} getSettings");
        //    var getSettings = await _serviceManager.walletReportService.getSettings();
        //    return Ok(getSettings);
        //}

        //[HttpPost("updateSettings")]
        //// [Authorize]
        //public async Task<IActionResult> updateSettings(updateSettingsViewModel updateSettingsViewModel)
        //{
        //    _logger.logInfo($" {LoggingEvents.updateItem} updateSettings");
        //    var update = await _serviceManager.walletReportService.updateSettings(updateSettingsViewModel);
        //    return Ok(update);
        //}

        //[HttpPost("getUplineTeamList")]
        //public async Task<IActionResult> getUplineTeamList(string authlogin)
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} getUplineTeamList");
        //    var getUplineTeamList = await _serviceManager.walletReportService.getUplineTeamList(authlogin);
        //    return Ok(getUplineTeamList);
        //}

        //[HttpPost("userSearchBindBuyPackage")]
        //public async Task<IActionResult> userSearchBindBuyPackage(string AuthLogin)
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} userSearchBindBuyPackage");
        //    var userSearchBindBuyPackage = await _serviceManager.walletReportService.userSearchBindBuyPackage(AuthLogin);
        //    return Ok(userSearchBindBuyPackage);
        //}

        //[HttpPost("getSalaryRankList")]
        //public async Task<IActionResult> getSalaryRankList(Guid URID)
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} getSalaryRankList");
        //    var getSalaryRankList = await _serviceManager.walletReportService.getSalaryRankList(URID);
        //    return Ok(getSalaryRankList);
        //}
    }
}
