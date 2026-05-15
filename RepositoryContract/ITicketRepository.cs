using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;
using static ViewModel.TicketViewModel;

namespace RepositoryContract
{
    public interface ITicketRepository
    {


        //---------------------
        public Task<ResponseViewModel> addTicket(AddTicket addTicket);
        public Task<ResponseViewModel> getAllTicketBYURID(Guid URID);
        public Task<ResponseViewModel> getTicketBYTicketId(Guid TicketId);
        public Task<ResponseViewModel> addTicketReply(AddTicketReply addTicketReply);
        public Task<ResponseViewModel> getAllTicketAdmin();
        public Task<ResponseViewModel> closeTicket(Guid TicketId);
        public Task<ResponseViewModel> GetAllclosedTicket();


        //-------------

        public Task<ResponseViewModel> getUserNotificationList(Guid URID);
        public Task<ResponseViewModel> getUserUnseenNotiCount(Guid URID);
        public Task<ResponseViewModel> updateUserNotiSeenStatus(Guid URID);

        public Task<ResponseViewModel> getAllUserNotificationList(Guid URID);
        public Task<ResponseViewModel> addExpoTokens(AddExpoTokensViewModel addExpoTokensViewModel);
        public Task<ResponseViewModel> getExpoNotiByURID(Guid URID);
        public Task<ResponseViewModel> sendNotification(SendNotificationViewModel sendNotificationViewModel);

        //--Count Notification
        public Task<ResponseViewModel> adminReplyCount(Guid URID, Guid TicketId);
        public Task<ResponseViewModel> userReplyCount(Guid URID, Guid TicketId);
        public Task<ResponseViewModel> updateAdminReplyCount(Guid URID, Guid TicketId);
        public Task<ResponseViewModel> updateUserReplyCount(Guid URID, Guid TicketId);

    }
}
