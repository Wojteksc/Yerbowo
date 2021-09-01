using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using Yerbowo.Api.Builders;
using HostBuilder = Yerbowo.Api.Builders.HostBuilder;

namespace Yerbowo.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = ConfigBuilder
                .CreateConfigBuilder()
                .Build();

            //Initialize Logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();

            try
            {
                Log.Information("Application Starting.");
                HostBuilder.CreateHostBuilder(args, config).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "The Application failed to start.");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}