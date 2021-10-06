using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.SystemConsole.Themes;

namespace social_network_otus
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, configuration) =>
                {
                    configuration.MinimumLevel.Verbose()
                        .WriteTo.Console(theme:ConsoleTheme.None)
                        .Enrich.FromLogContext()
                        .Enrich.WithEnvironmentName()
                        .Enrich.WithMachineName()
                        .Enrich.WithExceptionDetails();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
