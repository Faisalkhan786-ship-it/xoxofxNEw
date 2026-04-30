using ViewModel;

namespace RepositoryContract
{
    public interface IAuthenticationRepository
    {
        public Task<ResponseViewModel> appLogin(AppLoginViewModel appLogin);
        public Task<ResponseViewModel> adminUserLogin(AppUserAdminLoginViewModel appUserAdminLoginViewModel);

        public Task<ResponseViewModellogin> addAppUser(AddAppUserViewModel addAppUser);
        public Task<ResponseViewModel> getByReferralId(string loginId);
        public Task<ResponseViewModel> changePassword(ChangePasswordViewModel changePasswordViewModel);

        public Task<ResponseViewModel> GetUserKycByLoginId(string loginId);
        public Task<ResponseViewModel> sendOtp(SendOtpViewModel sendOtp);
        public Task<ResponseViewModel> forgotPassword(ForgotPasswordViewModel forgotPassword);
        public Task<ResponseViewModel> VerifyLoginid(verifyloginidViewModel verifyloginid);
        public Task<ResponseViewModel> GetAllUserRegitration();
        public Task<ResponseViewModel> validateOtpbyEmail(ValidateOtpViewModelbyemail validateOtpViewModelbyemail);

        public Task<ResponseViewModel> updateUserProfile(UpdateUserProfileViewModel updateUserProfileViewModel);
        public Task<ResponseViewModel> UserDashboardDetails(Guid URID);
        public Task<ResponseViewModel> sendOtpRequest(SendOtpFundRequestViewModel sendOtp);
        public Task<ResponseViewModel> validateOtp(ValidateOtpViewModel validateOtpViewModel);
        public Task<ResponseViewModel> UserUserRentelligenceDashboard(Guid URID);
        public Task<ResponseViewModel> getLBRank();
        public Task<ResponseViewModel> sendOtpWithdrawal(SendOtpWithdrawalViewModel sendOtp);
        public Task<ResponseViewModel> updateUserProfileImage(UpdateUserImageViewModel updateUserImageViewModel);

        public Task<ResponseViewModel> getAgentAnalyticsUser(Guid URID);
        public Task<ResponseViewModel> sendOtpEvent(SendOtpFundRequestViewModel sendOtp);
        public Task<ResponseViewModel> userDashboard(Guid URID);
        public Task<ResponseViewModel> getTransactionLog(Guid URID);
        public Task<ResponseViewModel> getABREngine(Guid URID);
        public Task<ResponseViewModel> getUserAnalytics(Guid URID);
        public Task<ResponseViewModel> getUserLinkedIds(Guid URID);

    }
}
