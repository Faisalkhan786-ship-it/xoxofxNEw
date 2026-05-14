using Common;
using EmailSystem;
using LoggerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceContract;
using System.Net;
using ViewModel;

namespace Rentelligence.AI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminMasterController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILoggerManager _logger;
        private readonly IConfiguration _configuration;
        private readonly EmailService emailService;
        private readonly ExtractToken extractToken;
        public AdminMasterController(IServiceManager serviceManager, ILoggerManager logger, IConfiguration configuration)
        {
            _serviceManager = serviceManager;
            _logger = logger;
            extractToken = new ExtractToken(configuration);
            emailService = new EmailService(configuration);
            _configuration = configuration;
        }
        [HttpPost("chanegAdminPassword")]
        public async Task<IActionResult> chanegAdminPassword(AdminMasterViewModel adminMasterViewModel)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} chanegAdminPassword");
            var chanegAdminPassword = await _serviceManager.adminMasterService.chanegAdminPassword(adminMasterViewModel);
            return Ok(chanegAdminPassword);
        }
        //[HttpPost("chanegAdminSponsorID")]
        //public async Task<IActionResult> chanegAdminSponsorID(AdminChangeSponsorIdViewModel AdminChangeSponsorIdViewModel)
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} chanegAdminSponsorID");
        //    var chanegAdminSponsorID = await _serviceManager.adminMasterService.chanegAdminSponsorID(AdminChangeSponsorIdViewModel);
        //    return Ok(chanegAdminSponsorID);
        //}

        //[HttpPost("blockUserByAdmin")]
        //public async Task<IActionResult> blockUserByAdmin(string authLogin)
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} blockUserByAdmin");
        //    var blockUserByAdmin = await _serviceManager.adminMasterService.blockUserByAdmin(authLogin);
        //    return Ok(blockUserByAdmin);
        //}
       
        [HttpGet("userNameByLoginId")]
        public async Task<IActionResult> userNameByLoginId(string authLogin)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} userNameByLoginId");
            var userNameByLoginId = await _serviceManager.adminMasterService.userNameByLoginId(authLogin);
            return Ok(userNameByLoginId);
        }
        [HttpPost("addCreditAndDebitFund")]
        public async Task<IActionResult> addCreditAndDebitFund(AdminManageFundViewModel adminManageFundViewModel)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} adminLogin");
            var add = await _serviceManager.adminMasterService.addCreditAndDebitFund(adminManageFundViewModel);
            return Ok(add);
        }
        //[HttpPost("downloadExcel")]
        //public async Task<IActionResult> downloadExcel(AdminDownloadExcelViewModel adminDownloadExcelViewModel)
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} downloadExcel");
        //    var downloadExcel = await _serviceManager.adminMasterService.downloadExcel(adminDownloadExcelViewModel);
        //    return Ok(downloadExcel);
        //}

        //[HttpPost("getNews")]
        //public async Task<IActionResult> getNews(NewsViewModel newsViewModel)
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} downloadExcel");
        //    var getEditNews = await _serviceManager.adminMasterService.getEditNews(newsViewModel);
        //    return Ok(getEditNews);
        //}
        //[HttpPost("updateNews")]
        //public async Task<IActionResult> updateNews(UpdateViewModel updateViewModel)
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} downloadExcel");
        //    var updateNews = await _serviceManager.adminMasterService.updateNews(updateViewModel);
        //    return Ok(updateNews);
        //}

        //[HttpGet("getSettinDetails")]
        //public async Task<IActionResult> getSettinDetails(SettinViewModel settinViewModel)
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} downloadExcel");
        //    var getSettinDetails = await _serviceManager.adminMasterService.getSettinDetails(settinViewModel);
        //    return Ok(getSettinDetails);
        //}

        //[HttpPost("updateSetting")]
        //public async Task<IActionResult> updateSetting(UpdateSettingViewModel updateSettingViewModel)
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} updateSetting");
        //    var updateSetting = await _serviceManager.adminMasterService.updateSetting(updateSettingViewModel);
        //    return Ok(updateSetting);
        //}
        //[HttpGet("getLeaseAgent")]
        //public async Task<IActionResult> getLeaseAgent()
        //{
        //    _logger.logInfo($" {LoggingEvents.getByIdItem} blockUserByAdmin");
        //    var getLeaseAgent = await _serviceManager.adminMasterService.getLeaseAgent();
        //    return Ok(getLeaseAgent);
        //}

        //[HttpPost("getGetLeaseStatement")]
        //public async Task<IActionResult> GetGetLeaseStatement([FromBody] LeaseStatementViewModel leaseStatementViewModel)
        //{
        //    _logger.logInfo($"{LoggingEvents.getByIdItem} getGetLeaseStatement");
        //    var response = await _serviceManager.adminMasterService.getGetLeaseStatement(leaseStatementViewModel);
        //    return Ok(response);
        //}
    }
}
