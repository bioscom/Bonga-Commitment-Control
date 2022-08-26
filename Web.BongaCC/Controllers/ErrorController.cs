using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EF.BongaCC.Data;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Web.BongaCC.Controllers
{
    [Route("error")]
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //[Route("500")]
        //public IActionResult AppError()
        //{
        //    var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        //    _telemetryClient.TrackException(exceptionHandlerPathFeature.Error);
        //    _telemetryClient.TrackEvent("Error.ServerError", new Dictionary<string, string>
        //    {
        //        ["originalPath"] = exceptionHandlerPathFeature.Path,
        //        ["error"] = exceptionHandlerPathFeature.Error.Message
        //    });
        //    return View();


        //    var ex = BongaCCDbContext.Features.Get<IExceptionHandlerFeature>();
        //    if (ex != null)
        //    {
        //        var err = $"<h1>Error: {ex.Error.Message}</h1>{ex.Error.Source}<hr />{context.Request.Path}<br />";
        //        err += $"QueryString: {context.Request.QueryString}<hr />";

        //        err += $"Stack Trace<hr />{ex.Error.StackTrace.Replace(Environment.NewLine, "<br />")}";
        //        if (ex.Error.InnerException != null) err += $"Inner Exception<hr />{ex.Error.InnerException?.Message.Replace(Environment.NewLine, "<br />")}";
        //        // This bit here to check for a form collection!
        //        if (context.Request.HasFormContentType && context.Request.Form.Any())
        //        {
        //            err += "<table border=\"1\"><tr><td colspan=\"2\">Form collection:</td></tr>";
        //            foreach (var form in context.Request.Form)
        //            {
        //                err += $"<tr><td>{form.Key}</td><td>{form.Value}</td></tr>";
        //            }
        //            err += "</table>";
        //        }

        //        await _emailSender.SendEmailAsync("Isaac.Bejide@shell.com", "Bonga Commitment Control v2 error", err);
        //        context.Response.Redirect("/Error?r=" + System.Net.WebUtility.UrlEncode(context.Request.Path + "?" + context.Request.QueryString));
        //    }
        //}
    }


}
