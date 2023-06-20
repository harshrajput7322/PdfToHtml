
using Microsoft.AspNetCore.Mvc;
using PdfToHtml.Models;
using SelectPdf;

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

            // Load the HTML file
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                var htmlContent = reader.ReadToEnd();

                // Create the PDF converter
                var converter = new SelectPdf.HtmlToPdf();

                // Convert HTML to PDF
                var pdf = converter.ConvertHtmlString(htmlContent);

                // Save the PDF document to a MemoryStream
                var stream = new MemoryStream();
                pdf.Save(stream);
                stream.Position = 0;

                // Set the response content type
                var contentType = "application/pdf";

                // Return the PDF file as a downloadable attachment
                return File(stream, contentType, "output.pdf");
            }
        }



    }
}

