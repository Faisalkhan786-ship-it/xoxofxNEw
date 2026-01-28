using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace ServiceContract
{
    public interface IWalletReportService
    {

        public Task<ResponseViewModel> getIncomeWalletWallerReport(WalletReportViewModel walletReportViewModel);
        public Task<ResponseViewModel> getIncomeAndDepositTransType(Guid URID);
        public Task<ResponseViewModel> getDepositWalletReport(DepositReportViewModel depositReportViewModel);
        public Task<ResponseViewModel> getIncomeWithdrawalHistory(IncomeWithdrawalHistoryViewModel incomeWithdrawalHistoryViewModel);

        public Task<ResponseViewModel> addRechargeTransact(AddRechargeTransactionViewModel addRechargeTransactionViewModel);
        public Task<ResponseViewModel> getRechargeTransact(Guid URID);
        public Task<ResponseViewModel> getRentWalletByURID(Guid URID);
        public Task<ResponseViewModel> getRentWalletWallerReport(RentWalletReportViewModel rentWalletReportViewModel);
        public Task<ResponseViewModel> getleaderShipURID(Guid URID);
        public Task<ResponseViewModel> getPerformanceRewardList(Guid URID);
        public Task<ResponseViewModel> getTransactionHistory(IncomeWithdrawalHistoryViewModel incomeWithdrawalHistoryViewModel);
        public Task<ResponseViewModel> updateRentWalletAdress(UpdateRentWalletAdressViewModel updateRentWalletAdressViewModel);
        public Task<ResponseViewModel> updateIncomeWalletAdress(UpdateIncometWalletAdressViewModel updateIncometWalletAdressViewModel);
        public Task<ResponseViewModel> getNetworkTree(string authlogin);
        public Task<ResponseViewModel> getAccStatemtnt(accStateMent accStateMent);
        public Task<ResponseViewModel> getAllWalletHistory(AllWalletHistory allWalletHistory);
        public Task<ResponseViewModel> getRechargeTransactionAdmin(RechargeTransactionAdminViewModel rechargeTransactionAdminViewModel);
        public Task<ResponseViewModel> getDownloadleaseagentbyRID(Guid RechargeId);
        public Task<ResponseViewModel> addRechargeTransactionAdmin(AddRechargeTransactionAdminViewModel addRechargeTransactionAdminViewModel);
        public Task<ResponseViewModel> addRechargeTransactionUser(AddRechargeTransactionUserViewModel addRechargeTransactionUserViewModel);
        public Task<ResponseViewModel> getBindBuyPackageList(Guid URID);

    }
}
