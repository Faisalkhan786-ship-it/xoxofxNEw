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
    public class AdminManageFundService: IAdminManageFundService
    {
        private readonly IRepositoryManager _repositoryManager;
        public AdminManageFundService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }
        //public async Task<ResponseViewModel> getUserWalletDetails(string LoginId)
        //{
        //    var getUserWalletDetails = await _repositoryManager.adminManageFundRepository.getUserWalletDetails(LoginId);
        //    return getUserWalletDetails;
        //}


        //public async Task<ResponseViewModel> getFundType()
        //{
        //    var getUserWalletDetails = await _repositoryManager.adminManageFundRepository.getFundType();
        //    return getUserWalletDetails;
        //}
        //public async Task<ResponseViewModel> getFundTypeWiseCrDr(int WalletId)
        //{
        //    var getUserWalletDetails = await _repositoryManager.adminManageFundRepository.getFundTypeWiseCrDr(WalletId);
        //    return getUserWalletDetails;
        //}
        //public async Task<ResponseViewModel> getUserWalletDetailsF(string loginId)
        //{
        //    var getUserWalletDetails = await _repositoryManager.adminManageFundRepository.getUserWalletDetailsF(loginId);
        //    return getUserWalletDetails;
        //}
        //public async Task<ResponseViewModel> allWalletHistory(AllWalletHistoryViewModel allWalletHistoryViewModel)
        //{
        //    var getUserWalletDetails = await _repositoryManager.adminManageFundRepository.allWalletHistory(allWalletHistoryViewModel);
        //    return getUserWalletDetails;
        //}
        public async Task<ResponseViewModel> getAllFundRequestReport_Admin(UnAppIncomeViewModel appUnAppFundRequestModel)
        {
            var getAllFundRequestReport_Admin = await _repositoryManager.adminManageFundRepository.getAllFundRequestReport_Admin(appUnAppFundRequestModel);
            return getAllFundRequestReport_Admin;
        }
        public async Task<ResponseViewModel> getAllUserWithdrawalRequest_Admin(UnAppIncomeViewModel appUnAppIncomeVideoModel)
        {
            var getAllUserWithdrawalRequest_Admin = await _repositoryManager.adminManageFundRepository.getAllUserWithdrawalRequest_Admin(appUnAppIncomeVideoModel);
            return getAllUserWithdrawalRequest_Admin;
        }
        public async Task<ResponseViewModel> getAllUserROIWithdrawalRequest_Admin(UnAppIncomeViewModel appUnAppIncomeVideoModel)
        {
            var getAllUserROIWithdrawalRequest_Admin = await _repositoryManager.adminManageFundRepository.getAllUserROIWithdrawalRequest_Admin(appUnAppIncomeVideoModel);
            return getAllUserROIWithdrawalRequest_Admin;
        }
        public async Task<ResponseViewModel> updateIncomeWalletAdress(UpdateIncometWalletAdressViewModel updateIncometWalletAdressViewModel)
        {
            var update = await _repositoryManager.adminManageFundRepository.updateIncomeWalletAdress(updateIncometWalletAdressViewModel);
            return update;
        }
        public async Task<ResponseViewModel> upIncWithdReqStatus_Admin(AppRejFundViewModel appRejFundViewModel)
        {
            var upIncWithdReqStatus_Admin = await _repositoryManager.adminManageFundRepository.upIncWithdReqStatus_Admin(appRejFundViewModel);
            return upIncWithdReqStatus_Admin;
        }
        public async Task<ResponseViewModel> upROIWithdReqStatus_Admin(AppRejFundViewModel appRejFundViewModel)
        {
            var upROIWithdReqStatus_Admin = await _repositoryManager.adminManageFundRepository.upROIWithdReqStatus_Admin(appRejFundViewModel);
            return upROIWithdReqStatus_Admin;
        }
        public async Task<ResponseViewModel> updateFundRequestStatus_Admin(AppRejFundViewModel appRejFundViewModel)
        {
            var updateFundRequestStatus_Admin = await _repositoryManager.adminManageFundRepository.updateFundRequestStatus_Admin(appRejFundViewModel);
            return updateFundRequestStatus_Admin;
        }
        public async Task<ResponseViewModel> updateRoiWalletAdress(UpdateIncometWalletAdressViewModel updateIncometWalletAdressViewModel)
        {
            var update = await _repositoryManager.adminManageFundRepository.updateRoiWalletAdress(updateIncometWalletAdressViewModel);
            return update;
        }
    }
}
