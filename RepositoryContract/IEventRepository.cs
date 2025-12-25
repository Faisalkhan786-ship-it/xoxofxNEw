using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace RepositoryContract
{
    public interface IEventRepository
    {
        public Task<ResponseViewModelSendEventEmail> addEvent(EventViewModel eventViewModel);
        public Task<ResponseViewModelSendEventEmail> UpdateEvent(UpdateEventViewModel updateEventViewModel);

        public Task<ResponseViewModelSendEventEmail> getAllEvent(int Id);
        public Task<ResponseViewModelSendEventEmail> addEventPreImages(AddEventPreImagesViewModel addEventPreImagesViewModel);

        public Task<ResponseViewModelSendEventEmail> getAllUserEvent(int Id);
        public Task<ResponseViewModelSendEventEmail> getScheduleByEID(Guid EventMasterID);

        public Task<ResponseViewModelSendEventEmail> addEventSchedule(EventScheduleMasterViewModel eventScheduleMasterViewModel);
        public Task<ResponseViewModelSendEventEmail> addUserEventbooking(AddUserEventbookingViewModel addUserEventbookingViewModel);

        public Task<ResponseViewModelSendEventEmail> getAllUserEventbookingMaster();
        public Task<ResponseViewModelSendEventEmail> getUserEventbookingbyURID(Guid URID);
        public Task<ResponseViewModelSendEventEmail> CloseEventMaster();
        public Task<ResponseViewModelSendEventEmail> DeleteEventImages(int Id);

        public Task<ResponseViewModelSendEventEmail> getEventImagesbyEMID(Guid EventMasterID);
        public Task<ResponseViewModelSendEventEmail> SendEmailsAllUser(SendEmailsAllUserViewModel sendEmailsAllUserViewModel);
        public Task<ResponseViewModelSendEventEmail> getVerifyEventUser(string AuthLogin);
        public Task<ResponseViewModelSendEventEmail> editScheduleByID(int Id);

        public Task<ResponseViewModelSendEventEmail> getClosedEveMaster(ClosedEveMasterViewModel closedEveMasterViewModel);
        public Task<ResponseViewModelSendEventEmail> bindKitAdmin();

    }
}
