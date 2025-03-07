using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel.Design;
using System.Resources;
using System.Security;

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



            //注入应用层


        }
    }
}
