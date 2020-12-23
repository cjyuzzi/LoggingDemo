using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace ILoggerDemo
{
    public class Program
    {
        private static IConfigurationRoot _configuration;
        private static Func<string, string, LogLevel, bool> _logFilter;

        public static void Main(string[] args)
        {
            // 取得日誌記錄的設定檔：
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("logging.json")
                .AddCommandLine(args)
                .Build();

            // 過濾條件設定（使用區域函式定義）。
            _logFilter = (provider, category, level) =>
               category switch
               {
                   "Foo" => level >= LogLevel.Debug,
                   "Bar" => level >= LogLevel.Warning,
                   "Baz" => level >= LogLevel.None,
                   _ => level >= LogLevel.Information
               };

            var host = CreateHostBuilder(args).Build();

            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Host created.");

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)

                // 註冊與記錄日誌相關的服務。
                .ConfigureLogging(builder => builder
                    .AddConfiguration(_configuration) // 設定採用外部的配置。
                    .SetMinimumLevel(LogLevel.Information) // 設定最低的記錄等級。
                    .AddFilter(_logFilter) // 過濾日誌。
                    .ClearProviders()　// 清除記錄提供者。
                    .AddSystemdConsole(options => options.IncludeScopes = true) // 啟動日誌範圍功能。

                    //.AddSimpleConsole(options => options.SingleLine = true)

                    //.AddSystemdConsole(options => options.UseUtcTimestamp = true)

                    //.AddJsonConsole(options => options.IncludeScopes = true)

                    .AddDebug()
                )
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
