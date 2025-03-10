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
            services.AddTransient<ConfigService>();
            services.AddTransient<FileStorageService>();


            //注入应用层
            services.AddTransient<ConfigSrv>();
            services.AddTransient<FileStorageSrv>();

        }
    }
}
