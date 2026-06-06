using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace RepositoryContract
{
    public interface IAdminMasterRepository
    {
        public Task<ResponseViewModel> userNameByLoginId(string authLogin);
        public Task<ResponseViewModel> addCreditAndDebitFund(AdminManageFundViewModel adminManageFundViewModel);
        public Task<ResponseViewModel> chanegAdminPassword(AdminMasterViewModel adminMasterViewModel);
        public Task<ResponseViewModel> chanegAdminSponsorID(AdminChangeSponsorIdViewModel AdminChangeSponsorIdViewModel);
        public Task<ResponseViewModel> blockUserByAdmin(string authLogin);
        public Task<ResponseViewModel> chanegAdminlvl(AdminChangelvlViewModel adminChangelvlViewModel);

        public Task<ResponseViewModel> downloadExcel(AdminDownloadExcelViewModel adminDownloadExcelViewModel);
        public Task<ResponseViewModel> getEditNews(NewsViewModel newsViewModel);
        public Task<ResponseViewModel> updateNews(UpdateViewModel updateViewModel);
        public Task<ResponseViewModel> getSettinDetails(SettinViewModel settinViewModel); 
        public Task<ResponseViewModel> updateSetting(UpdateSettingViewModel updateSettingViewModel);
        public Task<ResponseViewModel> getLeaseAgent();
        public Task<ResponseViewModel> getUserWalletDetailsF(string loginId);

        public Task<ResponseViewModel> getGetLeaseStatement(LeaseStatementViewModel leaseStatementViewModel);

    }
}
