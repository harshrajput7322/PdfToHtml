

using HiQPdf;
using Microsoft.AspNetCore.Mvc;
using PdfToHtml.Models;
using System.Text;
using PdfDocument = iText.Kernel.Pdf.PdfDocument;


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

                    // Create the HTML to PDF converter
                    var converter = new HtmlToPdf();

                    // Set the base URL (can be null if not needed)
                    string baseUrl = null;

                    // Convert HTML to PDF
                    byte[] pdfBytes = converter.ConvertHtmlToMemory(htmlContent, baseUrl);

                    // Save the PDF to the memory stream
                    stream.Write(pdfBytes, 0, pdfBytes.Length);

                    var contentType = "application/pdf";
                    return File(stream.ToArray(), contentType, "output.pdf");
                }
            }
        }



    }
}

