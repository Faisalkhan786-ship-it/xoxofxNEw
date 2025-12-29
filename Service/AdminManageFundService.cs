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
        public async Task<ResponseViewModel> getUserWalletDetails(string LoginId)
        {
            var getUserWalletDetails = await _repositoryManager.adminManageFundRepository.getUserWalletDetails(LoginId);
            return getUserWalletDetails;
        }

        public async Task<ResponseViewModel> addCreditAndDebitFund(AdminManageFundViewModel adminManageFundViewModel)
        {
            var getUserWalletDetails = await _repositoryManager.adminManageFundRepository.addCreditAndDebitFund(adminManageFundViewModel);
            return getUserWalletDetails;
        }
        public async Task<ResponseViewModel> getFundType()
        {
            var getUserWalletDetails = await _repositoryManager.adminManageFundRepository.getFundType();
            return getUserWalletDetails;
        }
        public async Task<ResponseViewModel> getFundTypeWiseCrDr(int WalletId)
        {
            var getUserWalletDetails = await _repositoryManager.adminManageFundRepository.getFundTypeWiseCrDr(WalletId);
            return getUserWalletDetails;
        }
        public async Task<ResponseViewModel> getUserWalletDetailsF(string loginId)
        {
            var getUserWalletDetails = await _repositoryManager.adminManageFundRepository.getUserWalletDetailsF(loginId);
            return getUserWalletDetails;
        }
        public async Task<ResponseViewModel> allWalletHistory(AllWalletHistoryViewModel allWalletHistoryViewModel)
        {
            var getUserWalletDetails = await _repositoryManager.adminManageFundRepository.allWalletHistory(allWalletHistoryViewModel);
            return getUserWalletDetails;
        }
    }
}
