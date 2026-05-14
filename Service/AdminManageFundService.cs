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

        //public async Task<ResponseViewModel> addCreditAndDebitFund(AdminManageFundViewModel adminManageFundViewModel)
        //{
        //    var getUserWalletDetails = await _repositoryManager.adminManageFundRepository.addCreditAndDebitFund(adminManageFundViewModel);
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
    }
}
