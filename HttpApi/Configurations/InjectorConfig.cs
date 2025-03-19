using Application.System.Services;
using Domain.System.Services;

namespace HttpApi.Configurations
{
    public static class InjectorConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // ASP.NET HttpContext dependency
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //注入领域层
            //具有多个类型参数的泛型接口的依赖注入方式（重要）
            //services.AddScoped(typeof(IRepository<,,>), typeof(Repository<,,>));
            services.AddTransient<UserService>();
            services.AddTransient<ConfigService>();
            services.AddTransient<FileStorageService>();


            //注入应用层
            services.AddTransient<UserSrv>();
            services.AddTransient<ConfigSrv>();
            services.AddTransient<FileStorageSrv>();

            //注入AutoMapping
            services.AddAutoMapper(typeof(Application.System.MappingProfile).Assembly);

            //注入Serilog日志配置
            services.AddSingleton(serviceProvider =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                if (configuration != null)
                {
                    return new SerilogConfig(configuration, serviceProvider).LoggerConfiguration();
                }
                throw new Exception("Pls Inject IConfiguration");
            });
        }
    }
}
