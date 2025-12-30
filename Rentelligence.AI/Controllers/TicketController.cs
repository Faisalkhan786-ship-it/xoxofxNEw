using LoggerService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceContract;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using ViewModel;
using static ViewModel.TicketViewModel;

namespace Rentelligence.AI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TicketController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILoggerManager _logger;
        public TicketController(IServiceManager serviceManager, ILoggerManager logger)
        {
            _serviceManager = serviceManager;
            _logger = logger;
        }

        [HttpPost("addTicket")]
        public async Task<IActionResult> addTicket(AddTicket addTicket)
        {
            _logger.logInfo($" {LoggingEvents.addItem} ticketService");
            var add = await _serviceManager.ticketService.addTicket(addTicket);
            return Ok(add);
        }


        [HttpPost("getAllTicketBYURID")]
        public async Task<IActionResult> getAllTicketBYURID(Guid URID)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getByIdBanner bannerId ${URID}");
            var getAllTicketBYURID = await _serviceManager.ticketService.getAllTicketBYURID(URID);
            if (getAllTicketBYURID.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Ticket Found");
            }
            return Ok(getAllTicketBYURID);
        }
        [HttpPost("getTicketBYTicketId")]
        public async Task<IActionResult> getTicketBYTicketId(Guid TicketId)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getByIdBanner bannerId ${TicketId}");
            var getTicketBYTicketId = await _serviceManager.ticketService.getTicketBYTicketId(TicketId);
            if (getTicketBYTicketId.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Ticket Found");
            }
            return Ok(getTicketBYTicketId);
        }
        [HttpPost("addTicketReply")]
        public async Task<IActionResult> addTicketReply(AddTicketReply addTicketReply)
        {
            _logger.logInfo($" {LoggingEvents.addItem} addTicketReply");
            var add = await _serviceManager.ticketService.addTicketReply(addTicketReply);
            return Ok(add);
        }

        [HttpPost("getAllTicketAdmin")]
        public async Task<IActionResult> getAllTicketAdmin()
        {
            var getAllTicketAdmin = await _serviceManager.ticketService.getAllTicketAdmin();
            if (getAllTicketAdmin.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Ticket Found");
            }
            return Ok(getAllTicketAdmin);
        }
        [HttpPost("closeTicket")]
        public async Task<IActionResult> closeTicket(Guid TicketId)
        {
            var closeTicketTest = await _serviceManager.ticketService.closeTicket(TicketId);
            if (closeTicketTest.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Ticket Found");
            }
            return Ok(closeTicketTest);
        }

        [HttpPost("getAllclosedTicket")]
        public async Task<IActionResult> getAllclosedTicket()
        {
            var GetAllclosedTicket = await _serviceManager.ticketService.GetAllclosedTicket();
            if (GetAllclosedTicket.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Ticket Found");
            }
            return Ok(GetAllclosedTicket);
        }       

        [HttpPost("getUserNotificationListbyURID")]
        public async Task<IActionResult> getUserNotificationListbyURID(Guid URID)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getLeaderShipbyURID");
            var getLeaderShipbyURID = await _serviceManager.ticketService.getUserNotificationList(URID);
            return Ok(getLeaderShipbyURID);
        }

        [HttpPost("updateUserNotification")]
        public async Task<IActionResult> updateUserNotification(Guid URID)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getLeaderShipbyURID");
            var getLeaderShipbyURID = await _serviceManager.ticketService.updateUserNotiSeenStatus(URID);
            return Ok(getLeaderShipbyURID);
        }

        [HttpPost("getAllUserNotificationList")]
        public async Task<IActionResult> getAllUserNotificationList(Guid URID)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getLeaderShipbyURID");
            var getLeaderShipbyURID = await _serviceManager.ticketService.getAllUserNotificationList(URID);
            return Ok(getLeaderShipbyURID);
        }

        [HttpPost("addExpoTokens")]
        public async Task<IActionResult> addExpoTokens(AddExpoTokensViewModel addExpoTokensViewModel)
        {            
            var addExpoTokens = await _serviceManager.ticketService.addExpoTokens(addExpoTokensViewModel);
            return Ok(addExpoTokens);
        }

        [HttpPost("getExpoNotiByURID")]
        public async Task<IActionResult> getExpoNotiByURID(Guid URID)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getExpoNotiByURID");
            var getExpoNotiByURID = await _serviceManager.ticketService.getExpoNotiByURID(URID);
            return Ok(getExpoNotiByURID);
        }


        [HttpPost("sendNotification")]
        public async Task<IActionResult> sendNotification([FromBody] SendNotificationViewModel model)
        {
            var result = await _serviceManager.ticketService.sendNotification(model);
            return Ok(result);
        }
        //--------- Ticket Repy Count       

        [HttpPost("adminReplyCount")]
        public async Task<IActionResult> adminReplyCount(Guid URID,Guid TicketId)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} adminReplyCount");
            var adminReplyCount = await _serviceManager.ticketService.adminReplyCount(URID, TicketId);
            return Ok(adminReplyCount);
        }


        [HttpPost("userReplyCount")]
        public async Task<IActionResult> userReplyCount(Guid URID, Guid TicketId)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} userReplyCount");
            var userReplyCount = await _serviceManager.ticketService.userReplyCount(URID, TicketId);
            return Ok(userReplyCount);
        }


        [HttpPost("updateAdminReplyCount")]
        public async Task<IActionResult> updateAdminReplyCount(Guid URID,Guid TicketId)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} updateAdminReplyCount");
            var updateAdminReplyCount = await _serviceManager.ticketService.updateAdminReplyCount(URID, TicketId);
            return Ok(updateAdminReplyCount);
        }
        [HttpPost("updateUserReplyCount")]
        public async Task<IActionResult> updateUserReplyCount(Guid URID, Guid TicketId)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} updateUserReplyCount");
            var updateUserReplyCount = await _serviceManager.ticketService.updateUserReplyCount(URID, TicketId);
            return Ok(updateUserReplyCount);
        }
    }
}
