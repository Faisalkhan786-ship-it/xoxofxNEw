using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace RepositoryContract
{
    public interface IAdminManageRepository
    {
        public Task<ResponseViewModel> adminSearchAllUsers(AdminManageViewModel adminManageViewModel);
        public Task<ResponseViewModel> getRentWallet(AppUnApprentViewModel appUnApprentViewModel);

    }
}
