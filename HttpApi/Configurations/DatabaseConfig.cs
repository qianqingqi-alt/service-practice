using Microsoft.EntityFrameworkCore;
using Domain.EntityFramework;
using Infrastructure.Utils;
using Microsoft.Extensions.Options;

namespace HttpApi.Configurations
{
    public static class DatabaseConfig
    {
        public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddDbContext<DataDbContext>(options => {
                var encryptKey = services.BuildServiceProvider().GetService<IOptions<ApplicationConfig>>()?.Value.EncryptionKey;
                options.UseSqlServer(EncryptUtil.AESDecrypt(configuration.GetConnectionString("DataConnection") ?? "", encryptKey ?? ""));
                //非调试时  取消EF 日志打印
#if DEBUG
                options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddFilter((_, __) => false).AddConsole()));
#endif
            });
        }
    }
}
