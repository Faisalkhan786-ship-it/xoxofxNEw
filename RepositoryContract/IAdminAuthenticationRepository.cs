using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace RepositoryContract
{
    public interface IAdminAuthenticationRepository
    {
        public Task<ResponseViewModel> adminUserLogin(AdminUserLoginViewModel adminUserLogin);
        public Task<ResponseViewModel> addAdminUser(AddAdminUserViewModel addAdminUser);
        public Task<ResponseViewModel> adminSendOtp(AdminSendOtpViewModel adminSendOtp);
        public Task<ResponseViewModel> adminVerifyOtp(AdminVerifyOtpViewModel adminVerifyOtp);
        public Task<ResponseViewModel> adminForgotPassword(AdminForgotPasswordViewModel adminForgotPassword);
        public Task<ResponseViewModel> getAdminUserDetails(AdminUserGuidViewModel adminUserGuid);
        public Task<ResponseViewModel> getAdminDashboardDetails(Guid adminUserId);
        public Task<ResponseViewModel> getAllAdminList();
        public Task<ResponseViewModel> updateAdminStatusActivate(Guid adminuserId);
        public Task<ResponseViewModel> updateAdminStatusDeActivate(Guid adminuserId);
        public Task<ResponseViewModel> addBulkRegsitration(BulkRegsitrationViewModel bulkRegsitrationViewModel);
        public Task<ResponseViewModel> adminForgotPassword(string username);
        public Task<ResponseViewModel> updateAdminProfile(UpdateAdminProfileViewModel updateAdminProfileViewModel);


    }
}
