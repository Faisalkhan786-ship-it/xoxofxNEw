using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace ServiceContract
{
    public interface IAdminManageFundService
    {
        //public Task<ResponseViewModel> getUserWalletDetails(String LoginId);
        //public Task<ResponseViewModel> getFundType();
        //public Task<ResponseViewModel> addCreditAndDebitFund(AdminManageFundViewModel adminManageFundViewModel);
        //public Task<ResponseViewModel> getFundTypeWiseCrDr(int WalletId);
        //public Task<ResponseViewModel> getUserWalletDetailsF(string loginId);
        //public Task<ResponseViewModel> allWalletHistory(AllWalletHistoryViewModel allWalletHistoryViewModel);

        public Task<ResponseViewModel> getAllFundRequestReport_Admin(UnAppIncomeViewModel appUnAppFundRequestModel);
        public Task<ResponseViewModel> getAllUserWithdrawalRequest_Admin(UnAppIncomeViewModel appUnAppIncomeVideoModel);
        public Task<ResponseViewModel> getAllUserROIWithdrawalRequest_Admin(UnAppIncomeViewModel appUnAppIncomeVideoModel);

    }
}
