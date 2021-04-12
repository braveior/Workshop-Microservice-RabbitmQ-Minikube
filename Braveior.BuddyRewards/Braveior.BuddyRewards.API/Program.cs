using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using MongoDB.Entities;
using Serilog;
using System;

namespace Braveior.BuddyRewards.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConfigureLogging();
            DB.InitAsync("buddyrewards", "127.0.0.1", 27017).GetAwaiter().GetResult();
            CreateHostBuilder(args).Build().Run();
        }

        private static string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name.ToLower()) ?? Environment.GetEnvironmentVariable(name.ToUpper());
        }

        private static void ConfigureLogging()
        {
            var configuration = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .Build();

            Log.Logger = new LoggerConfiguration()
              .WriteTo.Console()
              .ReadFrom.Configuration(configuration)
              .CreateLogger();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).UseSerilog(Log.Logger);
    }
}
