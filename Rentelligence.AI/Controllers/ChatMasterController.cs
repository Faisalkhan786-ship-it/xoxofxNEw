using Common;
using EmailSystem;
using LoggerService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceContract;
using System.Net;
using ViewModel;

namespace ChatBot_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatMasterController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILoggerManager _logger;
        private readonly IConfiguration _configuration;
        private readonly EmailService emailService;
        private readonly ExtractToken extractToken;
        public ChatMasterController(IServiceManager serviceManager, ILoggerManager logger, IConfiguration configuration)
        {
            _serviceManager = serviceManager;
            _logger = logger;
            extractToken = new ExtractToken(configuration);
            emailService = new EmailService(configuration);
            _configuration = configuration;
        }

        [HttpPost("addChatMessage")]
        public async Task<IActionResult> addChatMessage(ChatMasterViewModel chatMasterViewModel)
        {
            _logger.logInfo($" {LoggingEvents.addItem} addChatMessage");
            var addChatMessage = await _serviceManager.chatMasterServices.addChatMessage(chatMasterViewModel);
            return Ok(addChatMessage);
        }
        //[HttpPost("addNewChat")]
        //public async Task<IActionResult> addNewChat(NewChatViewModel newChatViewModel)
        //{
        //     _logger.logInfo($" {LoggingEvents.addItem} addNewChat");
        //    var addNewChat = await _serviceManager.chatMasterServices.addNewChat(newChatViewModel);
        //    return Ok(addNewChat);
        //}

        [HttpGet("getUserAllChatsbyUserId")]
        public async Task<IActionResult> getUserAllChatsbyUserId(Guid USERID)
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} getUserAllChatsbyUserId");
            var getUserAllChatsbyUserId = await _serviceManager.chatMasterServices.getUserAllChatsbyUserId(USERID);
            if (getUserAllChatsbyUserId.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No chat Found");
            }
            return Ok(getUserAllChatsbyUserId);
        }
        [HttpPost("getChatMessagesChatId")]
        public async Task<IActionResult> getChatMessagesChatId(ChatMessagesViewModel chatMessagesViewModel)
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} getChatMessagesChatId");
            var getChatMessagesChatId = await _serviceManager.chatMasterServices.getChatMessagesChatId(chatMessagesViewModel);
            if (getChatMessagesChatId.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No chat Found");
            }
            return Ok(getChatMessagesChatId);
        }

        [HttpGet("getUserAllChatsAdmin")]
        public async Task<IActionResult> getUserAllChatsAdmin(Guid USERID)
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} getUserAllChatsAdmin");
            var getUserAllChatsAdmin = await _serviceManager.chatMasterServices.getUserAllChatsAdmin(USERID);
            if (getUserAllChatsAdmin.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No chat Found");
            }
            return Ok(getUserAllChatsAdmin);
        }


        [HttpGet("chatMsgByIdAdmin")]
        public async Task<IActionResult> chatMsgByIdAdmin(int ChatId)
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} chatMsgByIdAdmin");
            var chatMsgByIdAdmin = await _serviceManager.chatMasterServices.chatMsgByIdAdmin(ChatId);
            if (chatMsgByIdAdmin.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No chat Found");
            }
            return Ok(chatMsgByIdAdmin);
        }
    }
}
