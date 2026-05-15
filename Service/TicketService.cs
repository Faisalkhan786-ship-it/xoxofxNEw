using RepositoryContract;
using ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;
using static Model.ModelType;
using static ViewModel.TicketViewModel;

namespace Service
{
    public class TicketService : ITicketService
    {
        private readonly IRepositoryManager _repositoryManager;
        public TicketService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }
        
        public async Task<ResponseViewModel> getUserNotificationList(Guid URID)
        {
            var getleaderShipURID = await _repositoryManager.ticketRepository.getUserNotificationList(URID);
            return getleaderShipURID;
        }

        public async Task<ResponseViewModel> getUserUnseenNotiCount(Guid URID)
        {
            var getleaderShipURID = await _repositoryManager.ticketRepository.getUserUnseenNotiCount(URID);
            return getleaderShipURID;
        }

        public async Task<ResponseViewModel> updateUserNotiSeenStatus(Guid URID)
        {
            var getleaderShipURID = await _repositoryManager.ticketRepository.updateUserNotiSeenStatus(URID);
            return getleaderShipURID;
        }

        public async Task<ResponseViewModel> getAllUserNotificationList(Guid URID)
        {
            var getAllUserNotificationList = await _repositoryManager.ticketRepository.getAllUserNotificationList(URID);
            return getAllUserNotificationList;
        }

        public async Task<ResponseViewModel> addExpoTokens(AddExpoTokensViewModel addExpoTokensViewModel)
        {
            var addExpoTokens = await _repositoryManager.ticketRepository.addExpoTokens(addExpoTokensViewModel);
            return addExpoTokens;
        }

        public async Task<ResponseViewModel> getExpoNotiByURID(Guid URID)
        {
            var getExpoNotiByURID = await _repositoryManager.ticketRepository.getExpoNotiByURID(URID);
            return getExpoNotiByURID;
        }

        public async Task<ResponseViewModel> sendNotification(SendNotificationViewModel sendNotificationViewModel)
        {
            var sendNotification = await _repositoryManager.ticketRepository.sendNotification(sendNotificationViewModel);
            return sendNotification;
        }

        //-------------------
        public async Task<ResponseViewModel> addTicket(AddTicket addTicket)
        {
            var add = await _repositoryManager.ticketRepository.addTicket(addTicket);
            return add;
        }

        public async Task<ResponseViewModel> getAllTicketBYURID(Guid URID)
        {
            var getAllTicketBYURID = await _repositoryManager.ticketRepository.getAllTicketBYURID(URID);
            return getAllTicketBYURID;
        }
        public async Task<ResponseViewModel> getTicketBYTicketId(Guid TicketId)
        {
            var getTicketBYTicketId = await _repositoryManager.ticketRepository.getTicketBYTicketId(TicketId);
            return getTicketBYTicketId;
        }
        public async Task<ResponseViewModel> addTicketReply(AddTicketReply addTicketReply)
        {
            var add = await _repositoryManager.ticketRepository.addTicketReply(addTicketReply);
            return add;
        }
        public async Task<ResponseViewModel> getAllTicketAdmin()
        {
            var getAllTicketAdmin = await _repositoryManager.ticketRepository.getAllTicketAdmin();
            return getAllTicketAdmin;
        }
        public async Task<ResponseViewModel> closeTicket(Guid TicketId)
        {
            var closeTicketTest = await _repositoryManager.ticketRepository.closeTicket(TicketId);
            return closeTicketTest;
        }
        public async Task<ResponseViewModel> GetAllclosedTicket()
        {
            var GetAllclosedTicket = await _repositoryManager.ticketRepository.GetAllclosedTicket();
            return GetAllclosedTicket;
        }

        //----notification Count
        public async Task<ResponseViewModel> adminReplyCount(Guid URID,Guid TicketId)
        {
            var adminReplyCount = await _repositoryManager.ticketRepository.adminReplyCount(URID,TicketId);
            return adminReplyCount;
        }
        public async Task<ResponseViewModel> userReplyCount(Guid URID, Guid TicketId)
        {
            var userReplyCount = await _repositoryManager.ticketRepository.userReplyCount(URID, TicketId);
            return userReplyCount;
        }
        public async Task<ResponseViewModel> updateAdminReplyCount(Guid URID, Guid TicketId)
        {
            var updateAdminReplyCount = await _repositoryManager.ticketRepository.updateAdminReplyCount(URID,TicketId);
            return updateAdminReplyCount;
        }
        public async Task<ResponseViewModel> updateUserReplyCount(Guid URID, Guid TicketId)
        {
            var updateUserReplyCount = await _repositoryManager.ticketRepository.updateUserReplyCount(URID,TicketId);
            return updateUserReplyCount;
        }
    }
}
