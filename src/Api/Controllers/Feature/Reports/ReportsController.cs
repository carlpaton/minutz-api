using System;
using System.IO;
using DinkToPdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Feature.Reports
{
    public class ReportsController : Controller
    {
        private string _apiKey = "ApiKey e1a0ebc62ece47de9e6d96db50a78501";
        private string _apiKeySencond = "e83061eabb1b42b29e437f1d14666dac";

        private string _irelandUrl = "https://eunorth.rotativahq.com";

        [Authorize]
        [HttpGet("api/reports/demo")]
        public ActionResult PrintIndex(string id)
        {
            var converter = new SynchronizedConverter(new PdfTools());
            var doc = new HtmlToPdfDocument
                      {
                          GlobalSettings =
                          {
                              ColorMode = ColorMode.Color,
                              Orientation = Orientation.Landscape,
                              PaperSize = PaperKind.A4Plus,
                          },
                          Objects =
                          {
                              new ObjectSettings()
                              {
                                  PagesCount = true,
                                  HtmlContent =
                                      @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. In consectetur mauris eget ultrices  iaculis. Ut                               odio viverra, molestie lectus nec, venenatis turpis.",
                                  WebSettings = {DefaultEncoding = "utf-8"},
                                  HeaderSettings =
                                  {
                                      FontSize = 9, Right = "Page [page] of [toPage]",
                                      Line = true, Spacing = 2.812
                                  }
                              }
                          }
                      };
            byte[] pdf = converter.Convert(doc);

            return File(pdf, "application/pdf");
        }

        public ActionResult Index(string name)
        {
            ViewBag.Message = string.Format("Hello {0} to ASP.NET MVC!", name);

            return View();
        }
    }
}