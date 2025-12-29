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
    public class CommunityController : ControllerBase
    {

        private readonly IServiceManager _serviceManager;
        private readonly ILoggerManager _logger;
        private readonly IConfiguration _configuration;
        private readonly EmailService emailService;
        private readonly ExtractToken extractToken;
        public CommunityController(IServiceManager serviceManager, ILoggerManager logger, IConfiguration configuration)
        {
            _serviceManager = serviceManager;
            _logger = logger;
            _configuration = configuration;
            emailService = new EmailService(configuration);
            extractToken = new ExtractToken(configuration);
        }

        [HttpPost("getdirectMember")]
        public async Task<IActionResult> getdirectMember(DirectMemberViewModel directMemberViewModel)
        {
            _logger.logInfo($" {LoggingEvents.updateItem} directMember");
            var returnData = await _serviceManager.communityContract.GetDirectMemberDetails(directMemberViewModel);
            return Ok(returnData);
        }

        [HttpPost("getPersonalTeam")]
        public async Task<IActionResult> getPersonalTeam(PersonalTeamViewModel PersonalTeamViewModel)
        {
            _logger.logInfo($" {LoggingEvents.updateItem} personalTeam");
            var returnData = await _serviceManager.communityContract.GetPersonalTeam(PersonalTeamViewModel);
            return Ok(returnData);
        }

        [HttpPost("getPersonalTeamList")]
        public async Task<IActionResult> getPersonalTeamList(PersonalTeamReportViewModel personalTeamReportViewModel)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} appLogin");
            var getPersonalTeamList = await _serviceManager.communityContract.getPersonalTeamList(personalTeamReportViewModel);
            return Ok(getPersonalTeamList);
        }
        [HttpGet("getAgentLeaseCredit")]
        public async Task<IActionResult> getAgentLeaseCredit(Guid urid)
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} getAllEvent");
            var getAgentLeaseCredit = await _serviceManager.communityContract.getAgentLeaseCredit(urid);
            if (getAgentLeaseCredit.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Event Found");
            }
            return Ok(getAgentLeaseCredit);
        }

    }
}
