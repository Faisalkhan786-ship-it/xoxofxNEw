using Microsoft.AspNetCore.Cors.Infrastructure;
using RepositoryContract;
using ServiceContract;

namespace Service
{
    public sealed class ServiceManager : IServiceManager
    {

        private readonly Lazy<ICategoryContract> _categoryContract;
        private readonly Lazy<ISubCategoryContract> _subCategoryContract;
        private readonly Lazy<ISubCategoryTypeContract> _subCategoryTypeContract;
        private readonly Lazy<ISellerContract> _sellerContract;
        private readonly Lazy<IProductContract> _productContract;
        private readonly Lazy<ICartContract> _cartContract;
        private readonly Lazy<IMenuContract> _menuContract;
        private readonly Lazy<ISubMenuContract> _subMenuContract;
        private readonly Lazy<IAdminAuthenticationContract> _adminAuthenticationContract;
        private readonly Lazy<IEventService> _eventService;
        private readonly Lazy<IAuthenticationService> _authenticationContract;
        private readonly Lazy<IGeographyContract> _geographyContract;

        private readonly Lazy<IFundManagerService> _fundManagerService;
        private readonly Lazy<IAdminManageFundService> _adminManageFundService;
        private readonly Lazy<ICommunityService> _communityContract;

        public ServiceManager(IRepositoryManager repositoryManager)
        {
            _communityContract = new Lazy<ICommunityService>(() => new CommunityService(repositoryManager));
            _categoryContract = new Lazy<ICategoryContract>(() => new CategoryService(repositoryManager));
            _subCategoryContract = new Lazy<ISubCategoryContract>(() => new SubCategoryService(repositoryManager));
            _subCategoryTypeContract = new Lazy<ISubCategoryTypeContract>(() => new SubCategoryTypeService(repositoryManager));
            _sellerContract = new Lazy<ISellerContract>(() => new SellerService(repositoryManager));
            _productContract = new Lazy<IProductContract>(() => new ProductService(repositoryManager));
            _cartContract = new Lazy<ICartContract>(() => new CartService(repositoryManager));
            _menuContract = new Lazy<IMenuContract>(() => new MenuService(repositoryManager));
            _subMenuContract = new Lazy<ISubMenuContract>(() => new SubMenuService(repositoryManager));
            _adminAuthenticationContract = new Lazy<IAdminAuthenticationContract>(() => new AdminAuthenticationService(repositoryManager));
            _eventService = new Lazy<IEventService>(() => new EventService(repositoryManager));
            _authenticationContract = new Lazy<IAuthenticationService>(() => new AuthenticationService(repositoryManager));
            _geographyContract = new Lazy<IGeographyContract>(() => new GeographyService(repositoryManager));
            _fundManagerService = new Lazy<IFundManagerService>(() => new FundManagerService(repositoryManager));
            _adminManageFundService = new Lazy<IAdminManageFundService>(() => new AdminManageFundService(repositoryManager));

        }
        public IAuthenticationService authenticationContract => _authenticationContract.Value;
        public ICategoryContract categoryContract => _categoryContract.Value;
        public ISubCategoryContract subCategoryContract => _subCategoryContract.Value;
        public ISubCategoryTypeContract subCategoryTypeContract => _subCategoryTypeContract.Value;
        public ISellerContract sellerContract => _sellerContract.Value;
        public IProductContract productContract => _productContract.Value;
        public ICartContract cartContract => _cartContract.Value;
        public IMenuContract menuContract => _menuContract.Value;
        public ISubMenuContract subMenuContract => _subMenuContract.Value;
        public IAdminAuthenticationContract adminAuthenticationContract => _adminAuthenticationContract.Value;
        public IEventService eventService => _eventService.Value;
        public IGeographyContract geographyContract => _geographyContract.Value;
        public IFundManagerService fundManagerService => _fundManagerService.Value;
        public IAdminManageFundService adminManageFundService => _adminManageFundService.Value;
        public ICommunityService communityContract => _communityContract.Value;

    }
}
