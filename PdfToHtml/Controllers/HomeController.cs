using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using System.Text;

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
                var task = new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
                task.Wait();

                var browser = Puppeteer.LaunchAsync(new LaunchOptions
                {
                    Headless = true
                }).Result;

                try
                {
                    var pageTask = browser.NewPageAsync();
                    pageTask.Wait();
                    var page = pageTask.Result;

                    var htmlContent = new StreamReader(file.OpenReadStream()).ReadToEnd();

                    var setContentTask = page.SetContentAsync(htmlContent);
                    setContentTask.Wait();

                    var pdfStreamTask = page.PdfStreamAsync();
                    pdfStreamTask.Wait();
                    var pdfStream = pdfStreamTask.Result;

                    pdfStream.CopyTo(stream);
                }
                finally
                {
                    browser.CloseAsync().Wait();
                }

                var contentType = "application/pdf";

                return File(stream.ToArray(), contentType, "output.pdf");
            }
        }

    }

}








