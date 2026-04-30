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

        public async Task<ResponseViewModel> getUserAllChatsAdmin(Guid USERID)
        {
            var getUserAllChatsAdmin = await _repositoryManager.chatMasterRepository.getUserAllChatsAdmin(USERID);
            return getUserAllChatsAdmin;
        }

        public async Task<ResponseViewModel> chatMsgByIdAdmin(int ChatId)
        {
            var chatMsgByIdAdmin = await _repositoryManager.chatMasterRepository.chatMsgByIdAdmin(ChatId);
            return chatMsgByIdAdmin;
        }
        public async Task<ResponseViewModel> useCredit(UseCreditViewModel useCreditViewModel)
        {
            var useCredit = await _repositoryManager.chatMasterRepository.useCredit(useCreditViewModel);
            return useCredit;
        }
        public async Task<ResponseViewModel> insertlinkedid(UselinkedidViewModel uselinkedViewModel)
        {
            var uselinkedid = await _repositoryManager.chatMasterRepository.insertlinkedid(uselinkedViewModel);
            return uselinkedid;
        }
        public async Task<ResponseViewModel> userDeleteChat(ChatMessagesViewModel chatMessagesViewModel)
        {
            var userDeleteChat = await _repositoryManager.chatMasterRepository.userDeleteChat(chatMessagesViewModel);
            return userDeleteChat;
        }
    }
}
