using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DinkToPdf;
using DinkToPdf.Contracts;
//using PDF_Generator.Utility;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Web.BongaCC.Controllers
{
    [Route("api/pdfcreator")]
    [ApiController]
    public class PdfCreatorController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private IConverter _converter;

        public PdfCreatorController(IConverter converter, IWebHostEnvironment env)
        {
            _converter = converter;
            _env = env;
        }

        [HttpGet]
        public IActionResult CreatePDF()
        {
            string fullPath = _env.WebRootPath + Path.DirectorySeparatorChar.ToString() + "assets" + Path.DirectorySeparatorChar.ToString() + "styles.css";

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Landscape,
                PaperSize = PaperKind.A3,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Bonga Commitment Control PDF Report",
                Out = @"D:\PDFCreator\Employee_Report.pdf"
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                //HtmlContent = TemplateGenerator.GetHTMLString(),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            _converter.Convert(pdf);

            return Ok("Successfully created PDF document.");
        }
    }



    public class PDFGeneratorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }



}