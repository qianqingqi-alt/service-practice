using Application.System.Dtos;
using Application.System.Services;
using Domain.EntityFramework;
using Domain.ValueObjests;
using Infrastructure.Serilog;
using Serilog;

namespace HttpApi.Configurations
{
    public class SerilogConfig
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public SerilogConfig(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        public Serilog.ILogger LoggerConfiguration()
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DataDbContext>();

            var configDict = dbContext.Config.ToDictionary(c => c.ConfigKey, c => c.ConfigValue);

            var storageType = GetConfigValue(configDict, "Serilog.StorageType");
            var logEventLevel = GetConfigValue(configDict, "Serilog.LogEventLevel", "debug");

            try
            {
                if (storageType == "File")
                {
                    ConfigureFileLogging(configDict, logEventLevel);
                }
                else if (storageType == "Http")
                {
                    ConfigureHttpLogging(configDict, logEventLevel);
                }
            }
            catch
            {
                SerilogConfigDetail.FileLoggerConfiguration(logEventLevel, "");
            }

            return Log.Logger;
        }

        private void ConfigureFileLogging(IDictionary<string, string> configDict, string logLevel)
        {
            var fileStorageId = GetConfigValue(configDict, "Serilog.FileStorageId");
            var systemFileStorageId = GetConfigValue(configDict, "System.FileStorageId");

            using var scope = _serviceProvider.CreateScope();
            var fileStorageSrv = scope.ServiceProvider.GetService<FileStorageSrv>();

            var storageId = Guid.TryParse(fileStorageId, out var guid) ? guid : Guid.Parse(systemFileStorageId);

            var fileStorage = fileStorageSrv?.GetFileStorage(storageId);
            if (fileStorage == null) return;

            var storageType = DetermineStorageType(fileStorage.FileStorageType);
            ConfigureStorage(logLevel, fileStorage, storageType);
        }

        private static void ConfigureStorage(string logLevel, FileStorageDto fileStorage, string storageType)
        {
            switch (storageType)
            {
                case SerilogStorageType.File:
                    SerilogConfigDetail.FileLoggerConfiguration(logLevel, fileStorage.FileSystemBasePath);
                    break;

                case SerilogStorageType.Azure:
                    SerilogConfigDetail.AzureLoggerConfiguration(logLevel, fileStorage.AzureStorageAccountConnectionString, fileStorage.AzureStorageAccountContainerName);
                    break;
            }
        }

        private static void ConfigureHttpLogging(IDictionary<string, string> configDict, string logLevel)
        {
            var requestHeaderJson = GetConfigValue(configDict, "Serilog.HttpHeaderTemplate");
            var requestBodyJson = GetConfigValue(configDict, "Serilog.HttpBodyTemplate");
            var requestUri = GetConfigValue(configDict, "Serilog.HttpUrl");

            SerilogConfigDetail.HttpLoggerConfiguration(logLevel, requestUri, requestHeaderJson, requestBodyJson);
        }

        private static string GetConfigValue(IDictionary<string, string> configDict, string key, string defaultValue = "")
        {
            return configDict.TryGetValue(key, out var value) ? value : defaultValue;
        }

        private static string DetermineStorageType(string storageType)
        {
            return storageType switch
            {
                FileStorageType.FileSystem => SerilogStorageType.File,
                FileStorageType.AzureStorageAccountGen1 or
                FileStorageType.AzureStorageAccountGen2 => SerilogStorageType.Azure,
                _ => SerilogStorageType.File
            };
        }
    }
}
