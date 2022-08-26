using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Console;
using EF.BongaCC.Data;
using EF.BongaCC.Data.Repository;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Web.BongaCC.Models;
using App.Services;
using Web.BongaCC.Controllers;
using Microsoft.Extensions.Hosting;
using System.Text;
using System.Text.Encodings.Web;
using System.Net;
using System;
using Microsoft.AspNetCore.Diagnostics;
using System.Linq;
using Microsoft.Extensions.Options;
using MimeKit;
using EnvironmentName = Microsoft.AspNetCore.Hosting.EnvironmentName;
using DinkToPdf;
using DinkToPdf.Contracts;

namespace Web.BongaCC
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public static string WebRootPath { get; private set; }
        private readonly IEmailSender _emailSender;

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            WebRootPath = env.WebRootPath;
        }

        public void ConfigureServices(IServiceCollection services)
        {
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
            //services.AddDbContext<SampleEntitiesDataContext>();
            services.AddDbContext<BongaCCDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            // Add MVC services to the services container.
            services
                .AddMvc(options => options.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            services
                .AddDistributedMemoryCache()
                .AddSession(opts => {
                    opts.Cookie.IsEssential = true;
                });


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


            // Add Kendo UI services to the services container
            services.AddKendo();

            // Add Demo database services to the services container
            //services.AddKendoDemo();
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, BongaCCDbContext context)
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();
            //app.UseDeveloperExceptionPage();
            env.EnvironmentName = EnvironmentName.Development;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
            }
            else
            {
                //app.UseExceptionHandler("/error");
                app.UseExceptionHandler(options =>
                {
                    //options.UseDeveloperExceptionPage();
                    options.Run(
                        async context =>
                        {
                            var ex = context.Features.Get<IExceptionHandlerFeature>();
                            if (ex != null)
                            {
                                var err = $"<h1>Error: {ex.Error.Message}</h1>{ex.Error.Source}<hr />{context.Request.Path}<br />";
                                err += $"QueryString: {context.Request.QueryString}<hr />";

                                err += $"Stack Trace<hr />{ex.Error.StackTrace.Replace(Environment.NewLine, "<br />")}";
                                if (ex.Error.InnerException != null) err += $"Inner Exception<hr />{ex.Error.InnerException?.Message.Replace(Environment.NewLine, "<br />")}";
                                // This bit here to check for a form collection!
                                if (context.Request.HasFormContentType && context.Request.Form.Any())
                                {
                                    err += "<table border=\"1\"><tr><td colspan=\"2\">Form collection:</td></tr>";
                                    foreach (var form in context.Request.Form)
                                    {
                                        err += $"<tr><td>{form.Key}</td><td>{form.Value}</td></tr>";
                                    }
                                    err += "</table>";
                                }

                                await _emailSender.SendEmailAsync("Isaac.Bejide@shell.com", "Bonga Commitment Control v2 error", err);
                                context.Response.Redirect("/Error?r=" + System.Net.WebUtility.UrlEncode(context.Request.Path + "?" + context.Request.QueryString));
                            }
                        });
                    //options.UseExceptionHandler("/Home/Error");
                    options.UseStatusCodePagesWithReExecute("/Error/{0}");
                });

                app.UseHsts();
            }
            app.UseHttpsRedirection();
            // Add static files to the request pipeline.
            app.UseStaticFiles();

            app.UseSession();
            app.UseCookiePolicy();

            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {
                routes.AddHyphenatedRoute();

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Commitments}/{action=Index}/{id?}");
            });



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
                        builder.AppendLine("Status Code: " + HtmlEncoder.Default.Encode(path.Substring(1)) + "<br />");
                    }
                    var referrer = oContext.Request.Headers["referer"];
                    if (!string.IsNullOrEmpty(referrer))
                    {
                        builder.AppendLine("Return to <a href=\"" + HtmlEncoder.Default.Encode(referrer) + "\">" + WebUtility.HtmlEncode(referrer) + "</a><br />");
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