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
    public class EventController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILoggerManager _logger;
        public EventController(IServiceManager serviceManager, ILoggerManager logger, IConfiguration configuration)
        {
            _serviceManager = serviceManager;
            _logger = logger;
        }
        [HttpPost("addEventMaster")]
        public async Task<IActionResult> addEventMaster(EventViewModel eventViewModel)
        {
            _logger.logInfo($" {LoggingEvents.addItem} addEvent");
            var add = await _serviceManager.eventService.addEvent(eventViewModel);
            return Ok(add);
        }

        [HttpPost("UpdateEvent")]
        public async Task<IActionResult> UpdateEvent(UpdateEventViewModel updateEventViewModel)
        {
            _logger.logInfo($" {LoggingEvents.addItem} UpdateEvent");
            var update = await _serviceManager.eventService.UpdateEvent(updateEventViewModel);
            return Ok(update);
        }


        [HttpGet("getAllEventMaster")]
        public async Task<IActionResult> getAllEventMaster(int Id)
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} getAllEvent");
            var getAllEvent = await _serviceManager.eventService.getAllEvent(Id);
            if (getAllEvent.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Event Found");
            }
            return Ok(getAllEvent);
        }

        [HttpPost("addEventPreImages")]
        public async Task<IActionResult> addEventPreImages(AddEventPreImagesViewModel addEventPreImagesViewModel)
        {
            _logger.logInfo($" {LoggingEvents.addItem} addEventPreImages");
            var add = await _serviceManager.eventService.addEventPreImages(addEventPreImagesViewModel);
            return Ok(add);
        }

        [HttpGet("getAllUserEvent")]
        public async Task<IActionResult> getAllUserEvent(int Id)
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} getAllUserEvent");
            var getAllUserEvent = await _serviceManager.eventService.getAllUserEvent(Id);
            if (getAllUserEvent.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Event Found");
            }
            return Ok(getAllUserEvent);
        }
        [Authorize]
        [HttpGet("getScheduleByEID")]
        public async Task<IActionResult> getScheduleByEID(Guid EventMasterID)
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} getScheduleByEID");
            var getScheduleByEID = await _serviceManager.eventService.getScheduleByEID(EventMasterID);
            if (getScheduleByEID.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Event Found");
            }
            return Ok(getScheduleByEID);
        }

        [HttpPost("addEventSchedule")]
        public async Task<IActionResult> addEventSchedule(EventScheduleMasterViewModel eventScheduleMasterViewModel)
        {
            _logger.logInfo($" {LoggingEvents.addItem} addEventSchedule");
            var add = await _serviceManager.eventService.addEventSchedule(eventScheduleMasterViewModel);
            return Ok(add);
        }

        [Authorize]
        [HttpPost("addUserEventbooking")]
        public async Task<IActionResult> addUserEventbooking(AddUserEventbookingViewModel addUserEventbookingViewModel)
        {
            _logger.logInfo($" {LoggingEvents.addItem} addUserEventbooking");
            var add = await _serviceManager.eventService.addUserEventbooking(addUserEventbookingViewModel);
            return Ok(add);
        }

        [HttpGet("getAllUserEventbookingMaster")]
        public async Task<IActionResult> getAllUserEventbookingMaster()
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} getAllUserEventbookingMaster");
            var getAllUserEventbookingMaster = await _serviceManager.eventService.getAllUserEventbookingMaster();
            if (getAllUserEventbookingMaster.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Event Found");
            }
            return Ok(getAllUserEventbookingMaster);
        }

        [Authorize]
        [HttpGet("getUserEventbookingbyURID")]
        public async Task<IActionResult> getUserEventbookingbyURID(Guid URID)
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} getScheduleByEID");
            var getUserEventbookingbyURID = await _serviceManager.eventService.getUserEventbookingbyURID(URID);
            if (getUserEventbookingbyURID.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Event Found");
            }
            return Ok(getUserEventbookingbyURID);
        }

        [HttpGet("closeEventMaster")]
        public async Task<IActionResult> closeEventMaster()
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} CloseEventMaster");
            var CloseEventMaster = await _serviceManager.eventService.CloseEventMaster();
            if (CloseEventMaster.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Event Found");
            }
            return Ok(CloseEventMaster);
        }

        [HttpGet("deleteEventImages")]
        public async Task<IActionResult> deleteEventImages(int Id)
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} DeleteEventImages");
            var DeleteEventImages = await _serviceManager.eventService.DeleteEventImages(Id);
            if (DeleteEventImages.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Event Found");
            }
            return Ok(DeleteEventImages);
        }

        [HttpGet("getEventImagesbyEMID")]
        public async Task<IActionResult> getEventImagesbyEMID(Guid EventMasterID)
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} DeleteEventImages");
            var getEventImagesbyEMID = await _serviceManager.eventService.getEventImagesbyEMID(EventMasterID);
            if (getEventImagesbyEMID.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Event Found");
            }
            return Ok(getEventImagesbyEMID);
        }
        [Authorize]
        [HttpPost("sendEmailsAllUser")]
        public async Task<IActionResult> sendEmailsAllUser(SendEmailsAllUserViewModel model)
        {
            _logger.logInfo($" {LoggingEvents.addItem} addEvent");
            var send = await _serviceManager.eventService.SendEmailsAllUser(model);
            return Ok(send);
        }
        [Authorize]

        [HttpGet("getVerifyEventUser")]
        public async Task<IActionResult> getVerifyEventUser(string AuthLogin)
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} getAllEvent");
            var getVerifyEventUser = await _serviceManager.eventService.getVerifyEventUser(AuthLogin);
            if (getVerifyEventUser.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Event Found");
            }
            return Ok(getVerifyEventUser);
        }
        [HttpGet("editScheduleByID")]
        public async Task<IActionResult> editScheduleByID(int Id)
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} getAllEvent");
            var editScheduleByID = await _serviceManager.eventService.editScheduleByID(Id);
            if (editScheduleByID.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Event Found");
            }
            return Ok(editScheduleByID);
        }

        [HttpPost("getClosedEveMaster")]
        public async Task<IActionResult> getClosedEveMaster(ClosedEveMasterViewModel closedEveMasterViewModel)
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} getClosedEveMaster");
            var getClosedEveMaster = await _serviceManager.eventService.getClosedEveMaster(closedEveMasterViewModel);
            if (getClosedEveMaster.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Event Found");
            }
            return Ok(getClosedEveMaster);
        }

        [HttpGet("bindKitAdmin")]
        public async Task<IActionResult> bindKitAdmin()
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} bindKitAdmin");
            var bindKitAdmin = await _serviceManager.eventService.bindKitAdmin();
            if (bindKitAdmin.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No Event Found");
            }
            return Ok(bindKitAdmin);
        }
    }
}
