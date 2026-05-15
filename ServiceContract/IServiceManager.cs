using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContract
{
    public interface IServiceManager
    {
        IAdminMasterService adminMasterService { get; }

        IAdminAuthenticationContract adminAuthenticationContract { get; }

        IAuthenticationService authenticationContract { get; }

        IAdminManageFundService adminManageFundService { get; }
        IAdminManageService adminManageService { get; }
        IMenuContract menuContract { get; }
        ISubMenuContract subMenuContract { get; }
        //IChatMasterServices chatMasterServices { get; }
        IGeographyContract geographyContract { get; }
        ITransactionsLogService transactionsLogService { get; }
        ITicketService ticketService { get; }

    }
}
