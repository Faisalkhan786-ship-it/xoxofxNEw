//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using ViewModel;

//namespace ServiceContract
//{
//    public interface IChatMasterServices
//    {
//        public Task<ResponseViewModelchatmaster> addChatMessage(ChatMasterViewModel chatMasterViewModel);
//        public Task<ResponseViewModelNewChat> addNewChat(NewChatViewModel newChatViewModel);
//        public Task<ResponseViewModel> getUserAllChatsbyUserId(Guid USERID);
//        public Task<ResponseViewModel> getChatMessagesChatId(ChatMessagesViewModel chatMessagesViewModel);
//        public Task<ResponseViewModel> getUserAllChatsAdmin(Guid USERID);
//        public Task<ResponseViewModel> chatMsgByIdAdmin(int ChatId);
//        public Task<ResponseViewModel> useCredit(UseCreditViewModel useCreditViewModel);
//        public Task<ResponseViewModel> insertlinkedid(UselinkedidViewModel uselinkedViewModel);
//        public Task<ResponseViewModel> userDeleteChat(ChatMessagesViewModel chatMessagesViewModel);

//    }
//}
