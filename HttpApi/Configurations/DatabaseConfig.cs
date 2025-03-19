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
            var encryptKey = services.BuildServiceProvider().GetService<IOptions<ApplicationConfig>>()?.Value.EncryptionKey;
            var str = EncryptUtil.AESEncrypt("Server=localhost;Port=3306;Database=qianqingqi;Uid=root;Pwd=1qaz!QAZ;", encryptKey);
            var connectionString = EncryptUtil.AESDecrypt(configuration.GetConnectionString("DataConnection") ?? "", encryptKey ?? "");
            services.AddDbContext<DataDbContext>(options =>
            {
                var serverVersion = new MySqlServerVersion(new Version(8, 0, 40));
                options.UseMySql(connectionString, serverVersion);
                //非调试时  取消EF 日志打印
#if DEBUG
                options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddFilter((_, __) => false).AddConsole()));
#endif
            });
        }
    }
}
