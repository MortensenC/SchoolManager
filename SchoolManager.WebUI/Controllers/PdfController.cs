using System;
using System.Web.Mvc;
using NReco.PdfGenerator;
using System.IO;

namespace SchoolManager.WebUI.Controllers
{
    public class PdfController : Controller
    {
        public ActionResult DemoPage()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult GeneratePdf(string htmlContent, string htmlUrl)
        {
            var htmlToPdf = new HtmlToPdfConverter();

            var pdfContentType = "application/pdf";
            if (!String.IsNullOrEmpty(htmlUrl))
            {
                return File(htmlToPdf.GeneratePdfFromFile(htmlUrl, null), pdfContentType);
            }
            else
            {
                return File(htmlToPdf.GeneratePdf(htmlContent, null), pdfContentType);
            }
        }

        public ActionResult DescargarPDF(Controller controller, string viewName, object model = null)
        {
            
            string html = ViewToString(controller, viewName, model);


            //FileResult fileResult = null;
            var generator = new HtmlToPdfConverter();
            generator.ExecutionTimeout = new TimeSpan(200000000);
            
           //string filename = "PDFView.pdf";

            var pdfBytes = generator.GeneratePdf(html, null);
            generator = null;

            var pdfContentType = "application/pdf";
            return File(pdfBytes, pdfContentType);
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
