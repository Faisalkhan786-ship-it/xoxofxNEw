using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace ServiceContract
{
    public interface IFundManagerService
    {
        public Task<ResponseViewModel> addUploadFund(FundManagerViewModel fundManagerViewModel);
        public Task<ResponseViewModel> getUserWalletDetails(Guid URID);
        public Task<ResponseViewModel> getPayModeMaster();
        public Task<ResponseViewModel> getMandatoryWithdrawalDetails(WithDrawalRequestViewModel withDrawalRequestViewModel);
        public Task<ResponseViewModel> addRequestUserwithdrawalCoin(RequestUserwithdrawalCoin requestUserwithdrawalCoin);
        public Task<ResponseViewModel> transferP2(P2PViewModel P2PViewModel);

        public Task<ResponseViewModel> getUserWalletBalance(Guid URID);
        public Task<ResponseViewModel> addTransferIncomeToDepositWallet(TransferIncomeToDepositWalletViewModel TransferIncomeToDepositWalletViewModel);

        public Task<ResponseViewModel> getIncomeToDepositWalletReport(Guid URID);

        public Task<ResponseViewModel> getAllFundRequestReport_Admin(AppUnAppFundRequestModel appUnAppFundRequestModel);

        public Task<ResponseViewModel> getAllUserWithdrawalRequest_Admin(AppUnAppIncomeVideoModel appUnAppIncomeVideoModel);
        public Task<ResponseViewModel> updateFundRequestStatus_Admin(AppRejFundViewModel appRejFundViewModel);
        public Task<ResponseViewModel> upIncWithdReqStatus_Admin(AppRejFundViewModel appRejFundViewModel);
        public Task<ResponseViewModel> getUserAutoDeposit(Guid URID);
        
        public Task<ResponseViewModel> addAutoDeposit(TokenDepositsViewModel tokenDepositsViewModel);

        public Task<ResponseViewModel> upRentWithdReqStatus_Admin(AppRejFundViewModel appRejFundViewModel);
        public Task<ResponseViewModel> addRechargeTransaction(addRechargeTransactionViewModel addRechargeTransactionViewModel);
        public Task<ResponseViewModel> getspBindPackageUserSide();
        public Task<ResponseViewModel> getUserDormantReportDetails(Guid URID);
        public Task<ResponseViewModel> getRechargeTransaction(Guid URID);

    }
}
