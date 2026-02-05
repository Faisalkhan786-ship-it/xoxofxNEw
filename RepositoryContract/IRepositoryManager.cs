using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryContract
{
    public interface IRepositoryManager
    {
        IAdminMasterRepository adminMasterRepository { get; }

        
        IAdminAuthenticationRepository adminAuthenticationRepository { get; }
        IAuthenticationRepository authenticationRepository { get; }      
        IAdminManageFundRepository adminManageFundRepository { get; }
        IAdminManageRepository adminManageRepository { get; }
        IMenuRepository menuRepository { get; }
        ISubMenuRepository subMenuRepository { get; }
        IChatMasterRepository chatMasterRepository { get; }

    }
}
