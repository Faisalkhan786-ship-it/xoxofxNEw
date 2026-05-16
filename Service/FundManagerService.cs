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
    public class FundManagerService: IFundManagerService
    {
        private readonly IRepositoryManager _repositoryManager;
        public FundManagerService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

    

        public async Task<ResponseViewModel> addUploadFund(FundManagerViewModel fundManagerViewModel)
        {
            return await _repositoryManager.fundManagerRepository.addUploadFund(fundManagerViewModel); ;
        }
        public async Task<ResponseViewModel> getUserWalletDetails(Guid URID)
        {
            var getUserWalletDetails = await _repositoryManager.fundManagerRepository.getUserWalletDetails(URID);
            return getUserWalletDetails;
        }
        public async Task<ResponseViewModel> getPayModeMaster()
        {
            var getPayModeMaster = await _repositoryManager.fundManagerRepository.getPayModeMaster();
            return getPayModeMaster;
        }
       
        public async Task<ResponseViewModel> getMandatoryWithdrawalDetails(WithDrawalRequestViewModel withDrawalRequestViewModel)
        {
            var getMandatoryWithdrawalDetails = await _repositoryManager.fundManagerRepository.getMandatoryWithdrawalDetails(withDrawalRequestViewModel);
            return getMandatoryWithdrawalDetails;
        }

        public async Task<ResponseViewModel> addRequestUserwithdrawalCoin(RequestUserwithdrawalCoin requestUserwithdrawalCoin)
        {
            var getMandatoryWithdrawalDetails = await _repositoryManager.fundManagerRepository.addRequestUserwithdrawalCoin(requestUserwithdrawalCoin);
            return getMandatoryWithdrawalDetails;
        }

        public async Task<ResponseViewModel> transferP2(P2PViewModel P2PViewModel)
        {
            return await _repositoryManager.fundManagerRepository.transferP2(P2PViewModel); ;
        }

        public async Task<ResponseViewModel> getUserWalletBalance(Guid URID)
        {
            var getUserWalletBalance = await _repositoryManager.fundManagerRepository.getUserWalletBalance(URID);
            return getUserWalletBalance;
        }

        public async Task<ResponseViewModel> addTransferIncomeToDepositWallet(TransferIncomeToDepositWalletViewModel TransferIncomeToDepositWalletViewModel)
        {
            var getMandatoryWithdrawalDetails = await _repositoryManager.fundManagerRepository.addTransferIncomeToDepositWallet(TransferIncomeToDepositWalletViewModel);
            return getMandatoryWithdrawalDetails;
        }

        public async Task<ResponseViewModel> getIncomeToDepositWalletReport(Guid URID)
        {
            var getUserWalletBalance = await _repositoryManager.fundManagerRepository.getIncomeToDepositWalletReport(URID);
            return getUserWalletBalance;
        }

        public async Task<ResponseViewModel> getAllFundRequestReport_Admin(AppUnAppFundRequestModel appUnAppFundRequestModel)
        {
            var getAllFundRequestReport_Admin = await _repositoryManager.fundManagerRepository.getAllFundRequestReport_Admin(appUnAppFundRequestModel);
            return getAllFundRequestReport_Admin;
        }



        public async Task<ResponseViewModel> updateFundRequestStatus_Admin(AppRejFundViewModel appRejFundViewModel)
        {
            var updateFundRequestStatus_Admin = await _repositoryManager.fundManagerRepository.updateFundRequestStatus_Admin(appRejFundViewModel);
            return updateFundRequestStatus_Admin;
        }

        public async Task<ResponseViewModel> getAllUserWithdrawalRequest_Admin(AppUnAppIncomeVideoModel appUnAppIncomeVideoModel)
        {
            var getAllUserWithdrawalRequest_Admin = await _repositoryManager.fundManagerRepository.getAllUserWithdrawalRequest_Admin(appUnAppIncomeVideoModel);
            return getAllUserWithdrawalRequest_Admin;
        }

        public async Task<ResponseViewModel> upIncWithdReqStatus_Admin(AppRejFundViewModel appRejFundViewModel)
        {
            var upIncWithdReqStatus_Admin = await _repositoryManager.fundManagerRepository.upIncWithdReqStatus_Admin(appRejFundViewModel);
            return upIncWithdReqStatus_Admin;
        }
        public async Task<ResponseViewModel> getUserAutoDeposit(Guid URID)
        {
            var getUserAutoDeposit = await _repositoryManager.fundManagerRepository.getUserAutoDeposit(URID);
            return getUserAutoDeposit;
        }

        public async Task<ResponseViewModel> addAutoDeposit(TokenDepositsViewModel tokenDepositsViewModel)
        {
            var addAutoDeposit = await _repositoryManager.fundManagerRepository.addAutoDeposit(tokenDepositsViewModel);
            return addAutoDeposit;
        }
        public async Task<ResponseViewModel> upRentWithdReqStatus_Admin(AppRejFundViewModel appRejFundViewModel)
        {
            var upIncWithdReqStatus_Admin = await _repositoryManager.fundManagerRepository.upRentWithdReqStatus_Admin(appRejFundViewModel);
            return upIncWithdReqStatus_Admin;
        }
        public async Task<ResponseViewModel> addRechargeTransaction(addRechargeTransactionViewModel addRechargeTransactionViewModel)
        {
            var upIncWithdReqStatus_Admin = await _repositoryManager.fundManagerRepository.addRechargeTransaction(addRechargeTransactionViewModel);
            return upIncWithdReqStatus_Admin;
        }
        public async Task<ResponseViewModel> getspBindPackageUserSide()
        {
            var getUserAutoDeposit = await _repositoryManager.fundManagerRepository.getspBindPackageUserSide();
            return getUserAutoDeposit;
        }
        public async Task<ResponseViewModel> getUserDormantReportDetails(Guid URID)
        {
            var getUserDormantReportDetails = await _repositoryManager.fundManagerRepository.getUserDormantReportDetails(URID);
            return getUserDormantReportDetails;
        }
        public async Task<ResponseViewModel> getRechargeTransaction(Guid URID)
        {
            var getRechargeTransaction = await _repositoryManager.fundManagerRepository.getRechargeTransaction(URID);
            return getRechargeTransaction;
        }

        public async Task<ResponseViewModel> getAllUserROIWithdrawalRequest_Admin(AppUnAppIncomeVideoModel appUnAppIncomeVideoModel)
        {
            var getAllUserROIWithdrawalRequest_Admin = await _repositoryManager.fundManagerRepository.getAllUserROIWithdrawalRequest_Admin(appUnAppIncomeVideoModel);
            return getAllUserROIWithdrawalRequest_Admin;
        }
        public async Task<ResponseViewModel> upROIWithdReqStatus_Admin(AppRejFundViewModel appRejFundViewModel)
        {
            var upROIWithdReqStatus_Admin = await _repositoryManager.fundManagerRepository.upROIWithdReqStatus_Admin(appRejFundViewModel);
            return upROIWithdReqStatus_Admin;
        }
    }
}

