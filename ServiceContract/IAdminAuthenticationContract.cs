using ViewModel;

namespace ServiceContract
{
    public interface IAdminAuthenticationContract
    {
        public Task<ResponseViewModel> adminUserLogin(AdminUserLoginViewModel adminAppLogin);

        public Task<ResponseViewModel> addAdminUser(AddAdminUserViewModel adminAddAppUser);
        public Task<ResponseViewModel> adminSendOtp(AdminSendOtpViewModel adminSendOtp);
        public Task<ResponseViewModel> adminVerifyOtp(AdminVerifyOtpViewModel adminVerifyOtp);
        public Task<ResponseViewModel> adminForgotPassword(AdminForgotPasswordViewModel adminUpdatePassword);
        public Task<ResponseViewModel> getAdminUserDetails(AdminUserGuidViewModel adminUserGuid);
        public Task<ResponseViewModel> getAdminDashboardDetails(Guid adminUserId);
        public Task<ResponseViewModel> getAllAdminList();

        public Task<ResponseViewModel> updateAdminStatusActivate(Guid adminuserId);
        public Task<ResponseViewModel> updateAdminStatusDeActivate(Guid adminuserId);
        public Task<ResponseViewModel> adminForgotPassword(string username);
        public Task<ResponseViewModel> addBulkRegsitration(BulkRegsitrationViewModel bulkRegsitrationViewModel);

        public Task<ResponseViewModel> updateAdminProfile(UpdateAdminProfileViewModel updateAdminProfileViewModel);
    }
}
