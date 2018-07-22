using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Feature.Reports
{
    internal class CustomAssemblyLoadContext : AssemblyLoadContext
    {
        public IntPtr LoadUnmanagedLibrary(string absolutePath)
        {
            return LoadUnmanagedDll(absolutePath);
        }
        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            return LoadUnmanagedDllFromPath(unmanagedDllName);
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            throw new NotImplementedException();
        }
    }
    
    public class ReportsController : Controller
    {
        private string _apiKey = "ApiKey e1a0ebc62ece47de9e6d96db50a78501";
        private string _apiKeySencond = "e83061eabb1b42b29e437f1d14666dac";

        private string _irelandUrl = "https://eunorth.rotativahq.com";
        private readonly IConverter _converter;

        public ReportsController(IConverter converter)  
        {
            _converter = converter;
        }

        [Authorize]
        [HttpGet("api/reports/demo")]
        public ActionResult PrintIndex(string id)
        {
            // CustomAssemblyLoadContext context = new CustomAssemblyLoadContext();
            // context.LoadUnmanagedLibrary(path);
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