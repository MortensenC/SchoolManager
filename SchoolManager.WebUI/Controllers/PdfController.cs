
using PdfSharp;
using PdfSharp.Pdf;
using System;
using System.IO;
using System.Web.Mvc;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace SchoolManager.WebUI.Controllers
{
    public class PdfController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public ActionResult DemoPage()
        {
            return View();
        }

        //[ValidateInput(false)]
        //public ActionResult GeneratePdf(string htmlContent, string htmlUrl)
        //{
        //    //byte[] content;
        //    //var htmlToPdf = new HtmlToPdfConverter();

        //    //var pdfContentType = "application/pdf";
        //    //if (!String.IsNullOrEmpty(htmlUrl))
        //    //{
        //    //    content = htmlToPdf.GeneratePdfFromFile(htmlUrl, null);
        //    //    return File(content, pdfContentType);
        //    //}
        //    //else
        //    //{
        //    //    content = htmlToPdf.GeneratePdf(htmlContent, null);
        //    //    return File(content, pdfContentType);
        //    //}
        //    return null;
        //}

        public ActionResult DescargarPDF(Controller controller, string viewName, object model = null)
        {
            string html = ViewToString(controller, viewName, model);
            PdfGenerateConfig pdfConfig = new PdfGenerateConfig()
            {
                MarginLeft = 27,
                MarginRight = 27,
                MarginTop = 10,
                MarginBottom = 10,
                
                PageSize = PageSize.A4,
                PageOrientation = PageOrientation.Portrait
            };
            PdfDocument pdf = PdfGenerator.GeneratePdf(html, pdfConfig);
            //pdf.Save("document.pdf");

            //// Render any HTML fragment or document to HTML
            //var Renderer = new IronPdf.HtmlToPdf();
            ////Renderer.PrintOptions.DPI = 300;
            //Renderer.PrintOptions.PaperSize = PdfPrintOptions.PdfPaperSize.A4;
            //Renderer.PrintOptions.MarginLeft = 27;
            //Renderer.PrintOptions.MarginRight = 27;
            //Renderer.PrintOptions.MarginTop = 10;
            //Renderer.PrintOptions.MarginBottom = 10;
            //Renderer.PrintOptions.EnableJavaScript = true;
            //Renderer.PrintOptions.CssMediaType = PdfPrintOptions.PdfCssMediaType.Screen;

            //var PDF = Renderer.RenderHtmlAsPdf(ViewToString(controller, viewName, model)).BinaryData;
            //Creates a new Memory stream
            MemoryStream stream = new MemoryStream();
            // Saves the document as stream
            pdf.Save(stream);
            pdf.Close();
            // Converts the PdfDocument object to byte form.
            byte[] docBytes = stream.ToArray();

            var pdfContentType = "application/pdf";
            return File(docBytes, pdfContentType);
        }

        private string ViewToString(Controller controller, string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                if (viewResult.View == null)
                    throw new Exception("No existe la vista " + viewName + " en el controlador " + controller.ControllerContext.RouteData.Values["controller"].ToString());
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

    }
}
