using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using System;

namespace SerilogDemo
{
    public class Program
    {
        public static int Main(string[] args)
        {
            // 取得設定檔。
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddCommandLine(args)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration) // 使用設定檔來配置。需要安裝 Serilog.Settings.Configuration
                .MinimumLevel.Debug() // 設定最小日誌事件等級。
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information) // 針對指定的類別限制日誌事件等級。
                .Enrich.FromLogContext() // 允許使用 LogContext 擴充事件資訊。
                .Enrich.WithThreadId() // Serilog.Enrichers.ThreadContext
                .Enrich.WithProperty("Hello", "World") // 自由擴充更多事件資訊。
                .WriteTo.Debug() // Serilog.Sinks.Debug
                .WriteTo.Console() // Serilog.Sinks.Console
                .WriteTo.File(new RenderedCompactJsonFormatter(), "logs\\myapp.txt",
                    rollingInterval: RollingInterval.Day,
                    shared: true) // Serilog.Sinks.File
                .WriteTo.Seq("http://localhost:5341") // Serilog.Sinks.Seq
                .CreateLogger(); // 建立全域 Logger 物件。

            try
            {
                Log.Information("Starting Serilog demo web host!");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
