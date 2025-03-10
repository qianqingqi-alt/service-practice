using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Http.HttpClients;

namespace Infrastructure.Serilog
{
    public class SerilogConfigDetail
    {
        private const string _outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:w} {MachineName} | {_userContext} | {UserId} |{} {DisplayName} | {Ip} {Url} {NewLine}{Exception}";
        public static void FileLoggerConfiguration(string storageLevel, string? filePath)
        {
            string machineName = Environment.MachineName;
            var path = filePath != null ? filePath + "\\logs\\log_.log" : "logs\\log_.log";
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(ConvertLogLevel(storageLevel))
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Machine.Name", machineName)
                .WriteTo.Async(config => config.File(
                    path,
                    outputTemplate: _outputTemplate,
                    rollingInterval: RollingInterval.Day
                ))
                .CreateLogger();
        }

        public static void AzureLoggerConfiguration(string storageLevel, string connectionString, string storageContainerName)
        {
            string machineName = Environment.MachineName;
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(ConvertLogLevel(storageLevel))
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Machine.Name", machineName)
                .WriteTo.Async(config => config.AzureBlobStorage(
                    connectionString: connectionString,
                    storageContainerName: storageContainerName,
                    outputTemplate: _outputTemplate,
                    storageFileName: "logs\\log_{yyyy}{MM}{dd}.log",
                    period: TimeSpan.FromSeconds(30),
                    batchPostingLimit: 500))
                .CreateLogger();
        }

        public static void HttpLoggerConfiguration(string storageLevel, string requestUri, string requestHeaderJson, string requestBodyJson)
        {
            string machineName = Environment.MachineName;
            var httpClient = new HttpClient();
            try
            {
                IList<RequestHeader> requestHeaders = JsonConvert.DeserializeObject<List<RequestHeader>>(requestHeaderJson);
                foreach (var item in requestHeaders)
                {
                    httpClient.DefaultRequestHeaders.Add(item.Name, item.Value);
                }
            }
            catch (System.Exception e)
            {
            }
            var JsonHttpClient = new JsonHttpClient(httpClient);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(ConvertLogLevel(storageLevel))
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Machine.Name", machineName)
                .WriteTo.Async(config => config.Http(
                        requestUri: requestUri,
                        httpClient: JsonHttpClient,
                        textFormatter: new SerilogTextFormatter(requestBodyJson),
                        //batchFormatter: new SerilogBatchFormatter(),
                        logEventsInBatchLimit: 500,
                        period: TimeSpan.FromSeconds(30),
                        queueLimitBytes: null
                    ))
                .CreateLogger();
        }

        private static LogEventLevel ConvertLogLevel(string logLevelStr)
        {
            switch (logLevelStr?.ToLower())
            {
                case "verbose":
                    return LogEventLevel.Verbose;
                case "debug":
                    return LogEventLevel.Debug;
                case "warning":
                    return LogEventLevel.Warning;
                case "error":
                    return LogEventLevel.Error;
                case "fatal":
                    return LogEventLevel.Fatal;
                default:
                    return LogEventLevel.Information;
            }
        }
    }
}
