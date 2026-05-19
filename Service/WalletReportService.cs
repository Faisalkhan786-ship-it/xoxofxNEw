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
    public class WalletReportService: IWalletReportService
    {
        private readonly IRepositoryManager _repositoryManager;
        public WalletReportService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }
        public async Task<ResponseViewModel> getIncomeWalletWallerReport(WalletReportViewModel walletReportViewModel)
        {
            var getIncomeWalletWallerReport = await _repositoryManager.walletReportRepository.getIncomeWalletWallerReport(walletReportViewModel);
            return getIncomeWalletWallerReport;
        }
        public async Task<ResponseViewModel> getIncomeAndDepositTransType(Guid URID)
        {
            var getIncomeAndDepositTransType = await _repositoryManager.walletReportRepository.getIncomeAndDepositTransType(URID);
            return getIncomeAndDepositTransType;
        }

        public async Task<ResponseViewModel> getDepositWalletReport(DepositReportViewModel depositReportViewModel)
        {
            var getDepositWalletWallerReport = await _repositoryManager.walletReportRepository.getDepositWalletReport(depositReportViewModel);
            return getDepositWalletWallerReport;
        }

        public async Task<ResponseViewModel> getIncomeWithdrawalHistory(IncomeWithdrawalHistoryViewModel1 incomeWithdrawalHistoryViewModel)
        {
            var getIncomeWithdrawalHistory = await _repositoryManager.walletReportRepository.getIncomeWithdrawalHistory(incomeWithdrawalHistoryViewModel);
            return getIncomeWithdrawalHistory;
        }

        public async Task<ResponseViewModel> getRechargeTransact(Guid URID)
        {
            var getRechargeTransact = await _repositoryManager.walletReportRepository.getRechargeTransact(URID);
            return getRechargeTransact;
        }

        public async Task<ResponseViewModel> addRechargeTransact(AddRechargeTransactionViewModel addRechargeTransactionViewModel)
        {
            var addRechargeTransact = await _repositoryManager.walletReportRepository.addRechargeTransact(addRechargeTransactionViewModel);
            return addRechargeTransact;
        }
        public async Task<ResponseViewModel> getRentWalletByURID(Guid URID)
        {
            var getRentWalletByURID = await _repositoryManager.walletReportRepository.getRentWalletByURID(URID);
            return getRentWalletByURID;
        }
        public async Task<ResponseViewModel> getRentWalletWallerReport(RentWalletReportViewModel rentWalletReportViewModel)
        {
            var getRentWalletWallerReport = await _repositoryManager.walletReportRepository.getRentWalletWallerReport(rentWalletReportViewModel);
            return getRentWalletWallerReport;
        }

        public async Task<ResponseViewModel> getleaderShipURID(Guid URID)
        {
            var getleaderShipURID = await _repositoryManager.walletReportRepository.getleaderShipURID(URID);
            return getleaderShipURID;
        }
        public async Task<ResponseViewModel> getPerformanceRewardList(Guid URID)
        {
            var getleaderShipURID = await _repositoryManager.walletReportRepository.getPerformanceRewardList(URID);
            return getleaderShipURID;
        }
        public async Task<ResponseViewModel> getNetworkTree(string authlogin)
        {
            var getleaderShipURID = await _repositoryManager.walletReportRepository.getNetworkTree(authlogin);
            return getleaderShipURID;
        }

        public async Task<ResponseViewModel> getTransactionHistory(IncomeWithdrawalHistoryViewModel incomeWithdrawalHistoryViewModel)
        {
            var getRentWalletByURID = await _repositoryManager.walletReportRepository.getTransactionHistory(incomeWithdrawalHistoryViewModel);
            return getRentWalletByURID;
        }
        public async Task<ResponseViewModel> updateRentWalletAdress(UpdateRentWalletAdressViewModel updateRentWalletAdressViewModel)
        {
            var update = await _repositoryManager.walletReportRepository.updateRentWalletAdress(updateRentWalletAdressViewModel);
            return update;
        }

        public async Task<ResponseViewModel> updateIncomeWalletAdress(UpdateIncometWalletAdressViewModel updateIncometWalletAdressViewModel)
        {
            var update = await _repositoryManager.walletReportRepository.updateIncomeWalletAdress(updateIncometWalletAdressViewModel);
            return update;
        }

        public async Task<ResponseViewModel> getAccStatemtnt(accStateMent accStateMent)
        {
            var update = await _repositoryManager.walletReportRepository.getAccStatemtnt(accStateMent);
            return update;
        }

        public async Task<ResponseViewModel> getAllWalletHistory(AllWalletHistory allWalletHistory)
        {
            var getAllWalletHistory = await _repositoryManager.walletReportRepository.getAllWalletHistory(allWalletHistory);
            return getAllWalletHistory;
        }
        //public async Task<ResponseViewModel> getAccStatemtnt(accStateMent accStateMent)
        public async Task<ResponseViewModel> getRechargeTransactionAdmin(RechargeTransactionAdminViewModel rechargeTransactionAdminViewModel)
        {
            var getRechargeTransactionAdmin = await _repositoryManager.walletReportRepository.getRechargeTransactionAdmin(rechargeTransactionAdminViewModel);
            return getRechargeTransactionAdmin;
        }

        public async Task<ResponseViewModel> getDownloadleaseagentbyRID(Guid RechargeId)
        {
            var getDownloadleaseagentbyRID = await _repositoryManager.walletReportRepository.getDownloadleaseagentbyRID(RechargeId);
            return getDownloadleaseagentbyRID;
        }
        public async Task<ResponseViewModel> addRechargeTransactionAdmin(AddRechargeTransactionAdminViewModel addRechargeTransactionAdminViewModel)
        {
            var addRechargeTransactionAdmin = await _repositoryManager.walletReportRepository.addRechargeTransactionAdmin(addRechargeTransactionAdminViewModel);
            return addRechargeTransactionAdmin;
        }
        public async Task<ResponseViewModel> addRechargeTransactionUser(AddRechargeTransactionUserViewModel addRechargeTransactionUserViewModel)
        {
            var addRechargeTransactionUser = await _repositoryManager.walletReportRepository.addRechargeTransactionUser(addRechargeTransactionUserViewModel);
            return addRechargeTransactionUser;
        }
        public async Task<ResponseViewModel> getBindBuyPackageList(Guid URID)
        {
            var getBindBuyPackageList = await _repositoryManager.walletReportRepository.getBindBuyPackageList(URID);
            return getBindBuyPackageList;
        }

        public async Task<ResponseViewModel> getSingleLeg_Report(String AuthLogin)
        {
            var getSingleLeg_Report = await _repositoryManager.walletReportRepository.getSingleLeg_Report(AuthLogin);
            return getSingleLeg_Report;
        }
        public async Task<ResponseViewModel> getUserAllWalletBalance(Guid URID)
        {
            var getUserAllWalletBalance = await _repositoryManager.walletReportRepository.getUserAllWalletBalance(URID);
            return getUserAllWalletBalance;
        }

        public async Task<ResponseViewModel> genrateROI_BOTCLICK(Guid URID)
        {
            var genrateROI_BOTCLICK = await _repositoryManager.walletReportRepository.genrateROI_BOTCLICK(URID);
            return genrateROI_BOTCLICK;
        }
        public async Task<ResponseViewModel> checkROI_BOTCLICK(Guid URID)
        {
            var checkROI_BOTCLICK = await _repositoryManager.walletReportRepository.checkROI_BOTCLICK(URID);
            return checkROI_BOTCLICK;
        }
        public async Task<ResponseViewModel> getSettings()
        {
            var getSettings = await _repositoryManager.walletReportRepository.getSettings();
            return getSettings;
        }

        public async Task<ResponseViewModel> updateSettings(updateSettingsViewModel updateSettingsViewModel)
        {
            var update = await _repositoryManager.walletReportRepository.updateSettings(updateSettingsViewModel);
            return update;
        }
        public async Task<ResponseViewModel> getROIWalletWallerReport(ROIWalletReportViewModel rOIWalletReportViewModel)
        {
            var getROIWalletWallerReport = await _repositoryManager.walletReportRepository.getROIWalletWallerReport(rOIWalletReportViewModel);
            return getROIWalletWallerReport;
        }

        public async Task<ResponseViewModel> getUplineTeamList(string authlogin)
        {
            var getUplineTeamList = await _repositoryManager.walletReportRepository.getUplineTeamList(authlogin);
            return getUplineTeamList;
        }
        public async Task<ResponseViewModel> userSearchBindBuyPackage(string authlogin)
        {
            var userSearchBindBuyPackage = await _repositoryManager.walletReportRepository.userSearchBindBuyPackage(authlogin);
            return userSearchBindBuyPackage;
        }

        public async Task<ResponseViewModel> getSalaryRankList(Guid URID)
        {
            var getSalaryRankList = await _repositoryManager.walletReportRepository.getSalaryRankList(URID);
            return getSalaryRankList;
        }

       
    }
}
