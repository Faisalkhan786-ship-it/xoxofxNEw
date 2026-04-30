using LoggerService;
using Repository;
using RepositoryContract;
using Service;
using ServiceContract;

namespace Rentelligence.AI.Extensions
{
    public static class ServiceConfigExtension
    {
        public static void addDapperContext(this IServiceCollection services)
        {
            services.AddSingleton<DapperContext>();
        }
        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddScoped<IRepositoryManager, RepositoryManager>();

        public static void ConfigureServiceManager(this IServiceCollection services) =>
            services.AddScoped<IServiceManager, ServiceManager>();


        public static void ConfigureLoggerServce(this IServiceCollection services) =>
         services.AddSingleton<ILoggerManager, LoggerManager>();
    }
}
