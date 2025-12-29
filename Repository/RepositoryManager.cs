using EmailSystem;
using RepositoryContract;
using System.Management;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly DapperContext _dapperContext;
        private readonly EmailService _emailService; //Add Nwew
        private readonly Lazy<ICommunityRepository> _communityRepository;

        private readonly Lazy<ICategoryRepository> _categoryRepository;
        private readonly Lazy<ISubCategoryRepository> _subCategoryRepository;
        private readonly Lazy<ISubCategoryTypeRepository> _subCategoryTypeRepository;
        private readonly Lazy<ISellerRepository> _sellerRepository;
        private readonly Lazy<IProductRepository> _productRepository;
        private readonly Lazy<ICartRepository> _cartRepository;
        private readonly Lazy<IMenuRepository> _menuRepository;
        private readonly Lazy<ISubMenuRepository> _subMenuRepository;
        private readonly Lazy<IAdminAuthenticationRepository> _adminAuthenticationRepository;
        private readonly Lazy<IEventRepository> _eventRepository;
        private readonly Lazy<IAuthenticationRepository> _authenticationRepository;
        private readonly Lazy<IGeographyRepository> _geographyRepository;
        private readonly Lazy<IFundManagerRepository> _fundManagerRepository;
        private readonly Lazy<IAdminManageFundRepository> _adminManageFundRepository;



        public RepositoryManager(DapperContext dapperContext, EmailService emailService)
        {
            _dapperContext = dapperContext; 
            _emailService = emailService; //Add New
            _authenticationRepository = new Lazy<IAuthenticationRepository>(() => new AuthenticationRepository(_dapperContext, _emailService));//Add Email section
            _communityRepository = new Lazy<ICommunityRepository>(() => new CommunityRepository(_dapperContext));

            _categoryRepository = new Lazy<ICategoryRepository>(() => new CategoryRepository(_dapperContext));
            _subCategoryRepository = new Lazy<ISubCategoryRepository>(() => new SubCategoryRepository(_dapperContext));
            _subCategoryTypeRepository = new Lazy<ISubCategoryTypeRepository>(() => new SubCategoryTypeRepository(_dapperContext));
            _sellerRepository = new Lazy<ISellerRepository>(() => new SellerRepository(_dapperContext));
            _productRepository = new Lazy<IProductRepository>(() => new ProductRepository(_dapperContext));
            _cartRepository = new Lazy<ICartRepository>(() => new CartRepository(_dapperContext));
            _menuRepository = new Lazy<IMenuRepository>(() => new MenuRepository(_dapperContext));
            _subMenuRepository = new Lazy<ISubMenuRepository>(() => new SubMenuRepository(_dapperContext));
            _adminAuthenticationRepository = new Lazy<IAdminAuthenticationRepository>(() => new AdminAuthenticationRepository(_dapperContext));
            _eventRepository = new Lazy<IEventRepository>(() => new EventRepository(_dapperContext, emailService));
            _geographyRepository = new Lazy<IGeographyRepository>(() => new GeographyRepository(_dapperContext));
            _fundManagerRepository = new Lazy<IFundManagerRepository>(() => new FundManagerRepository(_dapperContext));
            _adminManageFundRepository = new Lazy<IAdminManageFundRepository>(() => new AdminManageFundRepository(_dapperContext));

        }



        public IAuthenticationRepository authenticationRepository => _authenticationRepository.Value;
        public ICategoryRepository categoryRepository => _categoryRepository.Value;
        public ISubCategoryRepository subCategoryRepository => _subCategoryRepository.Value;
        public ISubCategoryTypeRepository subCategoryTypeRepository => _subCategoryTypeRepository.Value;
        public ISellerRepository sellerRepository => _sellerRepository.Value;
        public IProductRepository productRepository => _productRepository.Value;
        public ICartRepository cartRepository => _cartRepository.Value;

        public IMenuRepository menuRepository => _menuRepository.Value;
        public ISubMenuRepository subMenuRepository => _subMenuRepository.Value;
        public IAdminAuthenticationRepository adminAuthenticationRepository => _adminAuthenticationRepository.Value;
        public IEventRepository eventRepository => _eventRepository.Value;
        public IGeographyRepository geographyRepository => _geographyRepository.Value;
        public IFundManagerRepository fundManagerRepository => _fundManagerRepository.Value;
        public IAdminManageFundRepository adminManageFundRepository => _adminManageFundRepository.Value;
        public ICommunityRepository communityRepository => _communityRepository.Value;

    }
}
