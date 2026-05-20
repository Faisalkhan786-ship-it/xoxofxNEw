using Nethereum.BlockchainProcessing.BlockStorage.Repositories;
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
        IWalletReportRepository walletReportRepository { get; }
        IGeographyRepository geographyRepository { get; }
        ITransactionsLogRepository transactionsLogRepository  { get; }
        ITicketRepository ticketRepository { get; }
        IFundManagerRepository fundManagerRepository { get; }
        ICommunityRepository communityRepository { get; }
        ICategoryRepository categoryRepository { get; }
        IProductRepository productRepository { get; }

    }
}
