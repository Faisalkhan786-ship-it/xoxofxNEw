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
    public class AdminMasterService: IAdminMasterService
    {
        private readonly IRepositoryManager _repositoryManager;
        public AdminMasterService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<ResponseViewModel> addCreditAndDebitFund(AdminManageFundViewModel adminManageFundViewModel)
        {
            var getUserWalletDetails = await _repositoryManager.adminMasterRepository.addCreditAndDebitFund(adminManageFundViewModel);
            return getUserWalletDetails;
        }
        public async Task<ResponseViewModel> userNameByLoginId(string authLogin)
        {
            var userNameByLoginId = await _repositoryManager.adminMasterRepository.userNameByLoginId(authLogin);
            return userNameByLoginId;
        }


        public async Task<ResponseViewModel> chanegAdminlvl(AdminChangelvlViewModel adminChangelvlViewModel)
        {
            var chanegAdminlvl = await _repositoryManager.adminMasterRepository.chanegAdminlvl(adminChangelvlViewModel);
            return chanegAdminlvl;
        }
        public async Task<ResponseViewModel> chanegAdminPassword(AdminMasterViewModel adminMasterViewModel)
        {
            var chanegAdminPassword = await _repositoryManager.adminMasterRepository.chanegAdminPassword(adminMasterViewModel);
            return chanegAdminPassword;
        }

        public async Task<ResponseViewModel> chanegAdminSponsorID(AdminChangeSponsorIdViewModel AdminChangeSponsorIdViewModel)
        {
            var chanegAdminSponsorID = await _repositoryManager.adminMasterRepository.chanegAdminSponsorID(AdminChangeSponsorIdViewModel);
            return chanegAdminSponsorID;
        }
        public async Task<ResponseViewModel> blockUserByAdmin(string authLogin)
        {
            var blockUserByAdmin = await _repositoryManager.adminMasterRepository.blockUserByAdmin(authLogin);
            return blockUserByAdmin;
        }
        public async Task<ResponseViewModel> downloadExcel(AdminDownloadExcelViewModel adminDownloadExcelViewModel)
        {
            var downloadExcel = await _repositoryManager.adminMasterRepository.downloadExcel(adminDownloadExcelViewModel);
            return downloadExcel;
        }
        public async Task<ResponseViewModel> getEditNews(NewsViewModel newsViewModel)
        {
            var getEditNews = await _repositoryManager.adminMasterRepository.getEditNews(newsViewModel);
            return getEditNews;
        }

        public async Task<ResponseViewModel> updateNews(UpdateViewModel updateViewModel)
        {
            var updateNews = await _repositoryManager.adminMasterRepository.updateNews(updateViewModel);
            return updateNews;
        }
        public async Task<ResponseViewModel> getSettinDetails(SettinViewModel settinViewModel)
        {
            var getSettinDetails = await _repositoryManager.adminMasterRepository.getSettinDetails(settinViewModel);
            return getSettinDetails;
        }
        //update Setting
        public async Task<ResponseViewModel> updateSetting(UpdateSettingViewModel updateSettingViewModel)
        {
            var updateSetting = await _repositoryManager.adminMasterRepository.updateSetting(updateSettingViewModel);
            return updateSetting;
        }

        //update getLeaseAgent
        public async Task<ResponseViewModel> getLeaseAgent()
        {
            var getLeaseAgent = await _repositoryManager.adminMasterRepository.getLeaseAgent();
            return getLeaseAgent;
        }
        //update getLeaseAgent
        public async Task<ResponseViewModel> getGetLeaseStatement(LeaseStatementViewModel leaseStatementViewModel)
        {
            var getGetLeaseStatement = await _repositoryManager.adminMasterRepository.getGetLeaseStatement(leaseStatementViewModel);
            return getGetLeaseStatement;
        }

        public async Task<ResponseViewModel> getUserWalletDetailsF(string loginId)
        {
            var getUserWalletDetails = await _repositoryManager.adminMasterRepository.getUserWalletDetailsF(loginId);
            return getUserWalletDetails;
        }
    }
}
