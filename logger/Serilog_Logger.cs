using Serilog;
using Serilog.Formatting.Compact;
using ILogger = Serilog.ILogger;
using System.Configuration;
using Microsoft.Extensions.Configuration;


namespace logger
{
    public static class Serilog_Logger
    {
        private static readonly ILogger _errorLogger;
        private static readonly ILogger _InformationLogger;

        private static string ErrorFilePath;
        private static string InformationFilePath ;

        static Serilog_Logger()
        {
            var configuration = new ConfigurationBuilder()
                                                        .SetBasePath(Directory.GetCurrentDirectory())
                                                        .AddJsonFile("appsettings.json")
                                                        .Build();
            ErrorFilePath = configuration["LogError"];
            InformationFilePath = configuration["LogInfo"];
            _errorLogger = new LoggerConfiguration()
                .WriteTo.File(new RenderedCompactJsonFormatter(), ErrorFilePath,
                rollingInterval: RollingInterval.Day,
                fileSizeLimitBytes: 10000000000,
                rollOnFileSizeLimit: true, retainedFileCountLimit: 20
                )
                .CreateLogger();

            _InformationLogger = new LoggerConfiguration()
               .WriteTo.File(new RenderedCompactJsonFormatter(), InformationFilePath,
               rollingInterval: RollingInterval.Day,
               fileSizeLimitBytes: 10000000000,
               rollOnFileSizeLimit: true, retainedFileCountLimit: 20
               )
               .CreateLogger();
        }

        public static void LogError(Exception error)
        {
            string Error = error.Message;
            _errorLogger.Error(Error);
        }
        public static void LogError(string error)
        {
            _errorLogger.Error(error);
        }
        public static void LogInformation(string info)
        {
            _InformationLogger.Information(info);
        }
        public static void LogDebug(string debug)
        {
            _errorLogger.Error(debug);
        }
    }
}
