using EmailSystem;
using RepositoryContract;
using System.Management;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly DapperContext _dapperContext;
        private readonly EmailService _emailService; //Add Nwew
        private readonly Lazy<IAdminAuthenticationRepository> _adminAuthenticationRepository;
        private readonly Lazy<IAuthenticationRepository> _authenticationRepository;

        private readonly Lazy<IAdminManageFundRepository> _adminManageFundRepository;
        private readonly Lazy<IAdminMasterRepository> _adminMasterRepository;

        private readonly Lazy<IAdminManageRepository> _adminManageRepository;
        private readonly Lazy<IMenuRepository> _menuRepository;
        private readonly Lazy<ISubMenuRepository> _subMenuRepository;
        private readonly Lazy<IChatMasterRepository> _chatMasterRepository;



        public RepositoryManager(DapperContext dapperContext, EmailService emailService)
        {
            _dapperContext = dapperContext; 
            _emailService = emailService; //Add New
            _adminMasterRepository = new Lazy<IAdminMasterRepository>(() => new AdminMasterRepository(_dapperContext));

            _authenticationRepository = new Lazy<IAuthenticationRepository>(() => new AuthenticationRepository(_dapperContext, _emailService));//Add Email section

            _adminAuthenticationRepository = new Lazy<IAdminAuthenticationRepository>(() => new AdminAuthenticationRepository(_dapperContext));
            _adminManageFundRepository = new Lazy<IAdminManageFundRepository>(() => new AdminManageFundRepository(_dapperContext));
            _adminManageRepository = new Lazy<IAdminManageRepository>(() => new AdminManageRepository(_dapperContext));
            _menuRepository = new Lazy<IMenuRepository>(() => new MenuRepository(_dapperContext));
            _subMenuRepository = new Lazy<ISubMenuRepository>(() => new SubMenuRepository(_dapperContext));
            _chatMasterRepository = new Lazy<IChatMasterRepository>(() => new ChatMasterRepository(_dapperContext));

        }


        public IAdminMasterRepository adminMasterRepository => _adminMasterRepository.Value;

        public IAuthenticationRepository authenticationRepository => _authenticationRepository.Value;
       
        public IAdminAuthenticationRepository adminAuthenticationRepository => _adminAuthenticationRepository.Value;
       
        public IAdminManageFundRepository adminManageFundRepository => _adminManageFundRepository.Value;
        public IAdminManageRepository adminManageRepository => _adminManageRepository.Value;
        public IMenuRepository menuRepository => _menuRepository.Value;
        public ISubMenuRepository subMenuRepository => _subMenuRepository.Value;
        public IChatMasterRepository chatMasterRepository => _chatMasterRepository.Value;
    }
}
