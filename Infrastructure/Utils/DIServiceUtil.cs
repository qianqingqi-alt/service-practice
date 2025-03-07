using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Utils
{
    public class DIServiceUtil
    {
        public static IServiceProvider ServiceProvider { get; set; }
        public static IServiceCollection Services { get; set; }
        public static T GetService<T>()
        {
            return ServiceProvider.GetService<T>()!;
        }

        public static T GetServiceInScope<T>()
        {
            var scope = ServiceProvider.CreateScope();
            return scope.ServiceProvider.GetService<T>()!;
        }
    }
}
