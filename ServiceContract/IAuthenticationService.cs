using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace ServiceContract
{
    public interface IAuthenticationService
    {
        public Task<ResponseViewModel> appLogin(AppLoginViewModel appLogin);
        public Task<ResponseViewModel> adminUserLogin(AppUserAdminLoginViewModel appUserAdminLoginViewModel);
        public Task<ResponseViewModellogin> addAppUser(AddAppUserViewModel addAppUser);
        public Task<ResponseViewModel> getByReferralId(string loginId);
        public Task<ResponseViewModel> changePassword(ChangePasswordViewModel changePasswordViewModel);

        public Task<ResponseViewModel> GetUserKycByLoginId(string loginId);

        public Task<ResponseViewModel> sendOtp(SendOtpViewModel sendOtp);
        public Task<ResponseViewModel> forgotPassword(ForgotPasswordViewModel forgotPassword);
        public Task<ResponseViewModel> GetAllUserRegitration();
        public Task<ResponseViewModel> updateUserProfile(UpdateUserProfileViewModel updateUserProfileViewModel);
        public Task<ResponseViewModel> UserDashboardDetails(Guid URID);
        public Task<ResponseViewModel> UserSummaryDetails(Guid URID);
        public Task<ResponseViewModel> sendOtpRequest(SendOtpFundRequestViewModel sendOtp);
        public Task<ResponseViewModel> validateOtp(ValidateOtpViewModel validateOtpViewModel);
        public Task<ResponseViewModel> UserUserRentelligenceDashboard(Guid URID);
        public Task<ResponseViewModel> getLBRank();

        public Task<ResponseViewModel> sendOtpWithdrawal(SendOtpWithdrawalViewModel sendOtp);
        public Task<ResponseViewModel> updateUserProfileImage(UpdateUserImageViewModel updateUserImageViewModel);
        public Task<ResponseViewModel> getAgentAnalyticsUser(Guid URID);
        public Task<ResponseViewModel> sendOtpEvent(SendOtpFundRequestViewModel sendOtp);
    }
}
