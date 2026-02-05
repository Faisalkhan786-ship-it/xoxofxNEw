using RepositoryContract;
using ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace Service
{
    public class ChatMasterServices: IChatMasterServices
    {
        private readonly IRepositoryManager _repositoryManager;
        public ChatMasterServices(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<ResponseViewModelchatmaster> addChatMessage(ChatMasterViewModel chatMasterViewModel)
        {
            var addChatMessage = await _repositoryManager.chatMasterRepository.addChatMessage(chatMasterViewModel);
            return addChatMessage;
        }
        public async Task<ResponseViewModelNewChat> addNewChat(NewChatViewModel newChatViewModel)
        {
            var addNewChat = await _repositoryManager.chatMasterRepository.addNewChat(newChatViewModel);
            return addNewChat;
        }
        public async Task<ResponseViewModel> getUserAllChatsbyUserId(Guid USERID)
        {
            var getUserAllChatsbyUserId = await _repositoryManager.chatMasterRepository.getUserAllChatsbyUserId(USERID);
            return getUserAllChatsbyUserId;
        }
        public async Task<ResponseViewModel> getChatMessagesChatId(ChatMessagesViewModel chatMessagesViewModel)
        {
            var getChatMessagesChatId = await _repositoryManager.chatMasterRepository.getChatMessagesChatId(chatMessagesViewModel);
            return getChatMessagesChatId;
        }
    }
}
