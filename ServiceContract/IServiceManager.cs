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
        ITicketService ticketService { get; }
        IWalletReportService walletReportService { get; }

        ICategoryContract categoryContract { get; }
        ISubCategoryContract subCategoryContract { get; }
        ISubCategoryTypeContract subCategoryTypeContract { get; }
        ISellerContract sellerContract { get; }
        IProductContract productContract { get; }
        ICartContract cartContract { get; }
        IMenuContract menuContract { get; }
        ISubMenuContract subMenuContract { get; }
        IAdminAuthenticationContract adminAuthenticationContract { get; }
        IEventService eventService { get; }
        IAuthenticationService authenticationContract { get; }
        IGeographyContract geographyContract { get; }
        IFundManagerService fundManagerService { get; }
        IAdminManageFundService adminManageFundService { get; }
        ICommunityService communityContract { get; }

    }
}
