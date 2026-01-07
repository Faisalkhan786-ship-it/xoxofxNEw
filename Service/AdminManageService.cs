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
    public class AdminManageService: IAdminManageService
    {
        private readonly IRepositoryManager _repositoryManager;
        public AdminManageService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }
        public async Task<ResponseViewModel> adminSearchAllUsers(AdminManageViewModel adminManageViewModel)
        {
            var adminSearchAllUsers = await _repositoryManager.adminManageRepository.adminSearchAllUsers(adminManageViewModel);
            return adminSearchAllUsers;
        }

        public async Task<ResponseViewModel> getRentWallet(AppUnApprentViewModel appUnApprentViewModel)
        {
            var getRentWallet = await _repositoryManager.adminManageRepository.getRentWallet(appUnApprentViewModel);
            return getRentWallet;
        }
    }
}
