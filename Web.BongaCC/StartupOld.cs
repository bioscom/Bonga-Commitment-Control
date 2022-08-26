using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Hosting;
using EF.BongaCC.Data.Repository;
using EF.BongaCC.Data;
using System.Text;
using System.Text.Encodings.Web;
using System.Net;
using Web.BongaCC.Controllers;
using Web.BongaCC.Models;
using App.Services;
using EnvironmentName = Microsoft.AspNetCore.Hosting.EnvironmentName;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Logging.Console;
using Kendo.Mvc.Examples;

namespace Web.BongaCC
{
    public class StartupOld
    {
        public StartupOld(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            WebRootPath = env.WebRootPath;
        }
        //public Startup(IWebHostEnvironment env)
        //{
        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(env.ContentRootPath)
        //        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
        //        .AddEnvironmentVariables();
        //    Configuration = builder.Build();

        //    WebRootPath = env.WebRootPath;
        //}

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        public static string WebRootPath { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            // // Add Entity Framework services to the services container.
            //NOTE: This service is still under investigation. Source: https://github.com/aspnet/Hosting/issues/793
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDbContext<BongaCCDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //Service to connect to Oracle Database to fetch Users Information from Shell Information Ware House
            //services.AddDbContext<ManifestOracleDbContext>(options => options.UseOracle(Configuration.GetConnectionString("IWHConnection")));


            // Add MVC services to the services container.
            services
                .AddMvc(options => options.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            services
                .AddDistributedMemoryCache()
                .AddSession(opts => {
                    opts.Cookie.IsEssential = true;
                });

            // Add Kendo UI services to the services container
            services.AddKendo();

            // Add Demo database services to the services container
            //services.AddKendoDemo();






            // Add framework services.
            // Angular's default header name for sending the XSRF token.
            //services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

            //services
            //    .AddMvc()
            //    // Maintain property names during serialization. See:
            //    // https://github.com/aspnet/Announcements/issues/194
            //    //.AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            //    .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            //services.AddHttpContextAccessor();


            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IActionContextAccessor, ActionContextAccessor>();

            //Source: https://www.c-sharpcorner.com/article/reading-values-from-appsettings-json-in-asp-net-core/
            services.Configure<appSettingsModel>(Configuration.GetSection("EmailConfiguration"));
            //services.Configure<appSettingsModel>(Configuration.GetSection("ThresholdConfig"));

            //// Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            ////Inject Email service into application
            //services.AddSingleton<IEmailConfiguration>(Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
            //services.AddTransient<IEmailService, EmailService>();
            //https://stackoverflow.com/questions/16870413/how-to-call-another-controller-action-from-a-controller-in-mvc
            //services.AddTransient<CommitmentsController>();
            services.AddTransient<AppUsersController>();

            services
               .AddDistributedMemoryCache()
                   .AddSession();

            services.AddDistributedMemoryCache();
            services.AddRazorPages();
            services.AddControllersWithViews();
            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = false;
            });

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, BongaCCDbContext context)
        {
            app.UseDeveloperExceptionPage();
            env.EnvironmentName = EnvironmentName.Production;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //if (env.IsProduction() || env.IsStaging() || env.IsEnvironment("Staging_2"))
            //{
            //    app.UseExceptionHandler("/Error");
            //    //app.UseHsts();
            //}

            //Note: The order of middleware is important.In the preceding example, 
            //an InvalidOperationException exception occurs when UseSession is invoked after UseMvc.
            //For more information, see https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/index?view=aspnetcore-2.1#ordering


            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {
                routes.AddHyphenatedRoute();

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //// Add MVC to the request pipeline.
            //app.UseMvc(routes =>
            //{
            //    //routes.AddHyphenatedRoute();

            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Commitments}/{action=Index}/{id?}");
            //});

            // Configure Kendo UI
            //app.UseKendo(env);


#if StatusCodePages
            #region snippet_StatusCodePages
            // Expose the members of the 'Microsoft.AspNetCore.Http' namespace 
            // at the top of the file:
            // using Microsoft.AspNetCore.Http;
            app.UseStatusCodePages(async context =>
            {
                context.HttpContext.Response.ContentType = "text/plain";

                await context.HttpContext.Response.WriteAsync(
                    "Status code page, status code: " + 
                    context.HttpContext.Response.StatusCode);
            });
            #endregion
#endif

#if StatusCodePagesWithRedirect
            #region snippet_StatusCodePagesWithRedirect
            app.UseStatusCodePagesWithRedirects("/error/{0}");
            #endregion
#endif

            app.MapWhen(oContext => oContext.Request.Path == "/missingpage", builder => { });

            // "/error/400"
            app.Map("/error", error =>
            {
                error.Run(async oContext =>
                {
                    var builder = new StringBuilder();
                    builder.AppendLine("<html><body>");
                    builder.AppendLine("An error occurred.<br />");
                    var path = oContext.Request.Path.ToString();
                    if (path.Length > 1)
                    {
                        builder.AppendLine("Status Code: " +
                            HtmlEncoder.Default.Encode(path.Substring(1)) + "<br />");
                    }
                    var referrer = oContext.Request.Headers["referer"];
                    if (!string.IsNullOrEmpty(referrer))
                    {
                        builder.AppendLine("Return to <a href=\"" +
                            HtmlEncoder.Default.Encode(referrer) + "\">" +
                            WebUtility.HtmlEncode(referrer) + "</a><br />");
                    }
                    builder.AppendLine("</body></html>");
                    oContext.Response.ContentType = "text/html";
                    await oContext.Response.WriteAsync(builder.ToString());
                });
            });

            #region snippet_AppRun
            app.Run(async (oContext) =>
            {
                if (oContext.Request.Query.ContainsKey("throw"))
                {
                    throw new Exception("Exception triggered!");
                }
                var builder = new StringBuilder();
                builder.AppendLine("<html><body>Hello World!");
                builder.AppendLine("<ul>");
                builder.AppendLine("<li><a href=\"/?throw=true\">Throw Exception</a></li>");
                builder.AppendLine("<li><a href=\"/missingpage\">Missing Page</a></li>");
                builder.AppendLine("</ul>");
                builder.AppendLine("</body></html>");

                oContext.Response.ContentType = "text/html";
                await oContext.Response.WriteAsync(builder.ToString());
            });
            #endregion
        }
    }


}