using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace RepositoryContract
{
    public interface IAdminManageFundRepository
    {
        public Task<ResponseViewModel> getUserWalletDetails(string LoginId);
        public Task<ResponseViewModel> getFundType();
        public Task<ResponseViewModel> getFundTypeWiseCrDr(int WalletId);
        public Task<ResponseViewModel> addCreditAndDebitFund(AdminManageFundViewModel adminManageFundViewModel);
        public Task<ResponseViewModel> getUserWalletDetailsF(string loginId);
        public Task<ResponseViewModel> allWalletHistory(AllWalletHistoryViewModel allWalletHistoryViewModel);

    }
}
