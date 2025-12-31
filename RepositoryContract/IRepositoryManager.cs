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

        ICategoryRepository categoryRepository { get; }
        ISubCategoryRepository subCategoryRepository { get; }
        ISubCategoryTypeRepository subCategoryTypeRepository { get; }
        ISellerRepository sellerRepository { get; }
        IProductRepository productRepository { get; }
        ICartRepository cartRepository { get; }
        IMenuRepository menuRepository { get; }
        ISubMenuRepository subMenuRepository { get; }
        IAdminAuthenticationRepository adminAuthenticationRepository { get; }
        IEventRepository eventRepository { get; }
        IAuthenticationRepository authenticationRepository { get; }
        IGeographyRepository geographyRepository { get; }
        IFundManagerRepository fundManagerRepository { get; }
        IAdminManageFundRepository adminManageFundRepository { get; }
        ICommunityRepository communityRepository { get; }
        ITicketRepository ticketRepository { get; }
        IWalletReportRepository walletReportRepository { get; }


    }
}
