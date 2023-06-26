
using Microsoft.AspNetCore.Mvc;
using PdfToHtml.Models;

namespace PdfToHtml.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]

        public IActionResult ConvertToPdf(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                // Handle invalid file
                return BadRequest();
            }

            using (var stream = new MemoryStream())
            {
                var htmlContent = new StreamReader(file.OpenReadStream()).ReadToEnd();

                var Renderer = new IronPdf.HtmlToPdf();

                // Convert HTML to PDF
                var pdf = Renderer.RenderHtmlAsPdf(htmlContent);

                var contentType = "application/pdf";
                return File(pdf.BinaryData, contentType, "output.pdf");
            }
        }
    }
}

