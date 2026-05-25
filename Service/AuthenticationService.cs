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
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRepositoryManager _repositoryManager;
        public AuthenticationService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }
        public async Task<ResponseViewModel> appLogin(AppLoginViewModel appLogin)
        {
            var appLoginDetails = await _repositoryManager.authenticationRepository.appLogin(appLogin);
            return appLoginDetails;
        }
        public async Task<ResponseViewModel> adminUserLogin(AppUserAdminLoginViewModel appUserAdminLoginViewModel)
        {
            var adminUserLogin = await _repositoryManager.authenticationRepository.adminUserLogin(appUserAdminLoginViewModel);
            return adminUserLogin;
        }

        public async Task<ResponseViewModellogin> addAppUser(AddAppUserViewModel addAppUser)
        {
            return await _repositoryManager.authenticationRepository.addAppUser(addAppUser); ;
        }

        public async Task<ResponseViewModel> getByReferralId(string loginId)
        {
            var getByIdProduct = await _repositoryManager.authenticationRepository.getByReferralId(loginId);
            return getByIdProduct;
        }

        public async Task<ResponseViewModel> changePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            return await _repositoryManager.authenticationRepository.changePassword(changePasswordViewModel);
        }
        public async Task<ResponseViewModel> GetUserKycByLoginId(string loginId)
        {
            var GetUserKycByLoginId = await _repositoryManager.authenticationRepository.GetUserKycByLoginId(loginId);
            return GetUserKycByLoginId;
        }
        public async Task<ResponseViewModel> sendOtp(SendOtpViewModel sendOtp)
        {
            var sendOtpDetails = await _repositoryManager.authenticationRepository.sendOtp(sendOtp);
            return sendOtpDetails;
        }

        public async Task<ResponseViewModel> forgotPassword(ForgotPasswordViewModel forgotPassword)
        {
            return await _repositoryManager.authenticationRepository.forgotPassword(forgotPassword);
        }
        public async Task<ResponseViewModel> GetAllUserRegitration()
        {
            var appLoginDetails = await _repositoryManager.authenticationRepository.GetAllUserRegitration();
            return appLoginDetails;
        }

        public async Task<ResponseViewModel> updateUserProfile(UpdateUserProfileViewModel updateUserProfileViewModel)
        {
            return await _repositoryManager.authenticationRepository.updateUserProfile(updateUserProfileViewModel);
        }
        public async Task<ResponseViewModel> UserDashboardDetails(Guid URID)
        {
            var UserDashboardDetails = await _repositoryManager.authenticationRepository.UserDashboardDetails(URID);
            return UserDashboardDetails;
        }
        public async Task<ResponseViewModel> UserSummaryDetails(Guid URID)
        {
            var UserSummaryDetails = await _repositoryManager.authenticationRepository.UserSummaryDetails(URID);
            return UserSummaryDetails;
        }
        public async Task<ResponseViewModel> sendOtpRequest(SendOtpFundRequestViewModel sendOtp)
        {
            var sendOtpRequest = await _repositoryManager.authenticationRepository.sendOtpRequest(sendOtp);
            return sendOtpRequest;
        }
        public async Task<ResponseViewModel> validateOtp(ValidateOtpViewModel validateOtpViewModel)
        {
            var validateOtp = await _repositoryManager.authenticationRepository.validateOtp(validateOtpViewModel);
            return validateOtp;
        }
        public async Task<ResponseViewModel> UserUserRentelligenceDashboard(Guid URID)
        {
            var UserUserRentelligenceDashboard = await _repositoryManager.authenticationRepository.UserUserRentelligenceDashboard(URID);
            return UserUserRentelligenceDashboard;
        }
        public async Task<ResponseViewModel> getLBRank()
        {
            var UserUserRentelligenceDashboard = await _repositoryManager.authenticationRepository.getLBRank();
            return UserUserRentelligenceDashboard;
        }
        public async Task<ResponseViewModel> sendOtpWithdrawal(SendOtpWithdrawalViewModel sendOtp)
        {
            var sendOtpRequest = await _repositoryManager.authenticationRepository.sendOtpWithdrawal(sendOtp);
            return sendOtpRequest;
        }

        public async Task<ResponseViewModel> updateUserProfileImage(UpdateUserImageViewModel updateUserImageViewModel)
        {
            return await _repositoryManager.authenticationRepository.updateUserProfileImage(updateUserImageViewModel);
        }

        public async Task<ResponseViewModel> getAgentAnalyticsUser(Guid URID)
        {
            var getAgentAnalyticsUser = await _repositoryManager.authenticationRepository.getAgentAnalyticsUser(URID);
            return getAgentAnalyticsUser;
        }
        public async Task<ResponseViewModel> sendOtpEvent(SendOtpFundRequestViewModel sendOtp)
        {
            var sendOtpEvent = await _repositoryManager.authenticationRepository.sendOtpEvent(sendOtp);
            return sendOtpEvent;
        }
    }
}

