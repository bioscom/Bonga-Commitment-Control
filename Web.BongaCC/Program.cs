using System;
using System.IO;
using EF.BongaCC.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Web.BongaCC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    var context = services.GetRequiredService<BongaCCDbContext>();
                    BongaCCDbContextSeed.SeedAsync(context, loggerFactory).Wait();

                    //var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    //AppIdentityDbContextSeed.SeedAsync(userManager).Wait();
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();
        }


        public static IWebHostBuilder CreateHostBuilder(string[] args) => WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((webHostBuilderContext, configurationbuilder) =>
            {
                var environment = webHostBuilderContext.HostingEnvironment;
                string pathOfCommonSettingsFile = Path.Combine(environment.ContentRootPath, "..", "SolutionItems");
                configurationbuilder
                        .AddJsonFile("appSettings.json", optional: true)
                        .AddJsonFile(Path.Combine(pathOfCommonSettingsFile, "global.json"), optional: true);

                configurationbuilder.AddEnvironmentVariables();
            })
            .UseUrls("http://phc-s-04151:8192")
            .UseStartup<Startup>();

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });
    }
}