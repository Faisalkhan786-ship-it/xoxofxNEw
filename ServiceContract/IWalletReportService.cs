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
        public Task<ResponseViewModel> getIncomeWithdrawalHistory(IncomeWithdrawalHistoryViewModel1 incomeWithdrawalHistoryViewModel);

        public Task<ResponseViewModel> addRechargeTransact(AddRechargeTransactionViewModel addRechargeTransactionViewModel);
        public Task<ResponseViewModel> getRechargeTransact(Guid URID);
        public Task<ResponseViewModel> getRentWalletByURID(Guid URID);
        public Task<ResponseViewModel> getRentWalletWallerReport(RentWalletReportViewModel rentWalletReportViewModel);
        public Task<ResponseViewModel> getRankAchievementbyURID(Guid URID);
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
        public Task<ResponseViewModel> getBindBuyPackageList(Guid URID);
        public Task<ResponseViewModel> getSingleLeg_Report(String AuthLogin);
        public Task<ResponseViewModel> getUserAllWalletBalance(Guid URID);
        public Task<ResponseViewModel> genrateROI_BOTCLICK(Guid URID);
        public Task<ResponseViewModel> checkROI_BOTCLICK(Guid URID);
        public Task<ResponseViewModel> getSettings();
        public Task<ResponseViewModel> updateSettings(updateSettingsViewModel updateSettingsViewModel);

        public Task<ResponseViewModel> getROIWalletWallerReport(ROIWalletReportViewModel rOIWalletReportViewModel);
        public Task<ResponseViewModel> getUplineTeamList(string AuthLogin);

        public Task<ResponseViewModel> userSearchBindBuyPackage(string AuthLogin);
        public Task<ResponseViewModel> getSalaryRankList(Guid URID);
        public Task<ResponseViewModel> getRewardStatusDashboard(Guid URID);
        public Task<ResponseViewModel> downLineTree_Details_fourlvl(Guid URID);
        public Task<ResponseViewModel> getReferalink(string Authlogin);
        public Task<ResponseViewModel> getTransType(int? Type);


    }
}
