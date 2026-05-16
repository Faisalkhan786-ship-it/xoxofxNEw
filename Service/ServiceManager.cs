using Microsoft.AspNetCore.Cors.Infrastructure;
using RepositoryContract;
using ServiceContract;

namespace Service
{
    public sealed class ServiceManager : IServiceManager
    {


        private readonly Lazy<IAdminAuthenticationContract> _adminAuthenticationContract;
        private readonly Lazy<IAuthenticationService> _authenticationContract;

        private readonly Lazy<IAdminManageFundService> _adminManageFundService;
        private readonly Lazy<IAdminMasterService> _adminMasterService;

        private readonly Lazy<IAdminManageService> _adminManageService;
        private readonly Lazy<IMenuContract> _menuContract;
        private readonly Lazy<ISubMenuContract> _subMenuContract;
        //private readonly Lazy<IChatMasterServices> _chatMasterServices;
        private readonly Lazy<ITransactionsLogService> _transactionsLogService;
        private readonly Lazy<IGeographyContract> _geographyContract;
        private readonly Lazy<ITicketService> _ticketService; 
        private readonly Lazy<IFundManagerService> _fundManagerService;

        public ServiceManager(IRepositoryManager repositoryManager)
        {

            _adminAuthenticationContract = new Lazy<IAdminAuthenticationContract>(() => new AdminAuthenticationService(repositoryManager));

            _authenticationContract = new Lazy<IAuthenticationService>(() => new AuthenticationService(repositoryManager));
            _adminManageFundService = new Lazy<IAdminManageFundService>(() => new AdminManageFundService(repositoryManager));
            _adminMasterService = new Lazy<IAdminMasterService>(() => new AdminMasterService(repositoryManager));
            _adminManageService = new Lazy<IAdminManageService>(() => new AdminManageService(repositoryManager));
            _menuContract = new Lazy<IMenuContract>(() => new MenuService(repositoryManager));
            _subMenuContract = new Lazy<ISubMenuContract>(() => new SubMenuService(repositoryManager));
            //_chatMasterServices = new Lazy<IChatMasterServices>(() => new ChatMasterServices(repositoryManager));
            _transactionsLogService = new Lazy<ITransactionsLogService>(() => new TransactionsLogService(repositoryManager));
            _geographyContract = new Lazy<IGeographyContract>(() => new GeographyService(repositoryManager));
            _ticketService = new Lazy<ITicketService>(() => new TicketService(repositoryManager));
            _fundManagerService = new Lazy<IFundManagerService>(() => new FundManagerService(repositoryManager));

        }

        public IAdminMasterService adminMasterService => _adminMasterService.Value;
        public IAuthenticationService authenticationContract => _authenticationContract.Value;

        public IAdminAuthenticationContract adminAuthenticationContract => _adminAuthenticationContract.Value;

        public IAdminManageFundService adminManageFundService => _adminManageFundService.Value;

        public IAdminManageService adminManageService => _adminManageService.Value;
        public IMenuContract menuContract => _menuContract.Value;
        public ISubMenuContract subMenuContract => _subMenuContract.Value;
        //public IChatMasterServices chatMasterServices => _chatMasterServices.Value;
        public ITransactionsLogService transactionsLogService => _transactionsLogService.Value;
        public IGeographyContract geographyContract => _geographyContract.Value;
        public ITicketService ticketService => _ticketService.Value;
        public IFundManagerService fundManagerService => _fundManagerService.Value;

    }
}
