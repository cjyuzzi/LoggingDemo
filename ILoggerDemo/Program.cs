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
            // ���o��x�O�����]�w�ɡG
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("logging.json")
                .AddCommandLine(args)
                .Build();

            // �L�o����]�w�]�ϥΰϰ�禡�w�q�^�C
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

                // ���U�P�O����x�������A�ȡC
                .ConfigureLogging(builder => builder
                    .AddConfiguration(_configuration) // �]�w�ĥΥ~�����t�m�C
                    .SetMinimumLevel(LogLevel.Information) // �]�w�̧C���O�����šC
                    .AddFilter(_logFilter) // �L�o��x�C
                    .ClearProviders()�@// �M���O�����Ѫ̡C
                    .AddSystemdConsole(options => options.IncludeScopes = true) // �Ұʤ�x�d��\��C

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
