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

        public ServiceManager(IRepositoryManager repositoryManager)
        {

            _adminAuthenticationContract = new Lazy<IAdminAuthenticationContract>(() => new AdminAuthenticationService(repositoryManager));

            _authenticationContract = new Lazy<IAuthenticationService>(() => new AuthenticationService(repositoryManager));

            _adminManageFundService = new Lazy<IAdminManageFundService>(() => new AdminManageFundService(repositoryManager));
            _adminMasterService = new Lazy<IAdminMasterService>(() => new AdminMasterService(repositoryManager));
            _adminManageService = new Lazy<IAdminManageService>(() => new AdminManageService(repositoryManager));

        }

        public IAdminMasterService adminMasterService => _adminMasterService.Value;
        public IAuthenticationService authenticationContract => _authenticationContract.Value;

        public IAdminAuthenticationContract adminAuthenticationContract => _adminAuthenticationContract.Value;

        public IAdminManageFundService adminManageFundService => _adminManageFundService.Value;

        public IAdminManageService adminManageService => _adminManageService.Value;

    }
}
