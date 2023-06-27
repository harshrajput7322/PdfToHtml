
using iText.Html2pdf;
using iText.Kernel.Pdf;
using iTextSharp.text;
using iTextSharp.tool.xml;
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

            using (var stream = new MemoryStream()) // Create a MemoryStream object
            {
                // Load the HTML file
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    var htmlContent = reader.ReadToEnd();

                    // Create the PDF writer
                    var writer = new PdfWriter(stream);

                    // Create the PDF document
                    var pdfDocument = new PdfDocument(writer);

                    // Convert HTML to PDF
                    var converter = new ConverterProperties();
                    HtmlConverter.ConvertToPdf(htmlContent, pdfDocument, converter);

                    // Close the PDF document
                    pdfDocument.Close();

                    var pdfBytes = stream.ToArray();
                    var contentType = "application/pdf";
                    return File(pdfBytes, contentType, "output.pdf");
                }
            }
        }



    }
}

