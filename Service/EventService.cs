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
    public class EventService : IEventService
    {
        private readonly IRepositoryManager _repositoryManager;
        public EventService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }
        public async Task<ResponseViewModelSendEventEmail> addEvent(EventViewModel eventViewModel)
        {
            var add = await _repositoryManager.eventRepository.addEvent(eventViewModel);
            return add;
        }

        public async Task<ResponseViewModelSendEventEmail> UpdateEvent(UpdateEventViewModel updateEventViewModel)
        {
            var update = await _repositoryManager.eventRepository.UpdateEvent(updateEventViewModel);
            return update;
        }

        public async Task<ResponseViewModelSendEventEmail> getAllEvent(int Id)
        {
            var getAllEvent = await _repositoryManager.eventRepository.getAllEvent(Id);
            return getAllEvent;
        }

        public async Task<ResponseViewModelSendEventEmail> addEventPreImages(AddEventPreImagesViewModel addEventPreImagesViewModel)
        {
            var add = await _repositoryManager.eventRepository.addEventPreImages(addEventPreImagesViewModel);
            return add;
        }
        public async Task<ResponseViewModelSendEventEmail> getAllUserEvent(int Id)
        {
            var getAllUserEvent = await _repositoryManager.eventRepository.getAllUserEvent(Id);
            return getAllUserEvent;
        }

        public async Task<ResponseViewModelSendEventEmail> addEventSchedule(EventScheduleMasterViewModel eventScheduleMasterViewModel)
        {
            var add = await _repositoryManager.eventRepository.addEventSchedule(eventScheduleMasterViewModel);
            return add;
        }
        public async Task<ResponseViewModelSendEventEmail> getScheduleByEID(Guid EventMasterID)
        {
            var getScheduleByEID = await _repositoryManager.eventRepository.getScheduleByEID(EventMasterID);
            return getScheduleByEID;
        }
        public async Task<ResponseViewModelSendEventEmail> addUserEventbooking(AddUserEventbookingViewModel addUserEventbookingViewModel)
        {
            var add = await _repositoryManager.eventRepository.addUserEventbooking(addUserEventbookingViewModel);
            return add;
        }
        public async Task<ResponseViewModelSendEventEmail> getAllUserEventbookingMaster()
        {
            var getAllUserEventbookingMaster = await _repositoryManager.eventRepository.getAllUserEventbookingMaster();
            return getAllUserEventbookingMaster;
        }

        public async Task<ResponseViewModelSendEventEmail> getUserEventbookingbyURID(Guid URID)
        {
            var getUserEventbookingbyURID = await _repositoryManager.eventRepository.getUserEventbookingbyURID(URID);
            return getUserEventbookingbyURID;
        }
        public async Task<ResponseViewModelSendEventEmail> CloseEventMaster()
        {
            var CloseEventMaster = await _repositoryManager.eventRepository.CloseEventMaster();
            return CloseEventMaster;
        }

        public async Task<ResponseViewModelSendEventEmail> DeleteEventImages(int Id)
        {
            var DeleteEventImages = await _repositoryManager.eventRepository.DeleteEventImages(Id);
            return DeleteEventImages;
        }

        public async Task<ResponseViewModelSendEventEmail> getEventImagesbyEMID(Guid EventMasterID)
        {
            var getEventImagesbyEMID = await _repositoryManager.eventRepository.getEventImagesbyEMID(EventMasterID);
            return getEventImagesbyEMID;
        }

        public async Task<ResponseViewModelSendEventEmail> SendEmailsAllUser(SendEmailsAllUserViewModel sendEmailsAllUserViewModel)
        {
            var send = await _repositoryManager.eventRepository.SendEmailsAllUser(sendEmailsAllUserViewModel);
            return send;
        }
        public async Task<ResponseViewModelSendEventEmail> getVerifyEventUser(string AuthLogin)
        {
            var getVerifyEventUser = await _repositoryManager.eventRepository.getVerifyEventUser(AuthLogin);
            return getVerifyEventUser;
        }

        public async Task<ResponseViewModelSendEventEmail> editScheduleByID(int Id)
        {
            var editScheduleByID = await _repositoryManager.eventRepository.editScheduleByID(Id);
            return editScheduleByID;
        }

        public async Task<ResponseViewModelSendEventEmail> getClosedEveMaster(ClosedEveMasterViewModel closedEveMasterViewModel)
        {
            var get = await _repositoryManager.eventRepository.getClosedEveMaster(closedEveMasterViewModel);
            return get;
        }
        public async Task<ResponseViewModelSendEventEmail> bindKitAdmin()
        {
            var getbindKitAdmin = await _repositoryManager.eventRepository.bindKitAdmin();
            return getbindKitAdmin;
        }
    }
}
