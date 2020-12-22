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
            // ���o�]�w�ɡC
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddCommandLine(args)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration) // �ϥγ]�w�ɨӰt�m�C�ݭn�w�� Serilog.Settings.Configuration
                .MinimumLevel.Debug() // �]�w�̤p��x�ƥ󵥯šC
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information) // �w����w�����O�����x�ƥ󵥯šC
                .Enrich.FromLogContext() // ���\�ϥ� LogContext �X�R�ƥ��T�C
                .Enrich.WithThreadId() // Serilog.Enrichers.ThreadContext
                .Enrich.WithProperty("Hello", "World") // �ۥ��X�R��h�ƥ��T�C
                .WriteTo.Debug() // Serilog.Sinks.Debug
                .WriteTo.Console() // Serilog.Sinks.Console
                .WriteTo.File(new RenderedCompactJsonFormatter(), "logs\\myapp.txt",
                    rollingInterval: RollingInterval.Day,
                    shared: true) // Serilog.Sinks.File
                .WriteTo.Seq("http://localhost:5341") // Serilog.Sinks.Seq
                .CreateLogger(); // �إߥ��� Logger ����C

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
