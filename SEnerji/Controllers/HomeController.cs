using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using SEnerji.Models;
using System.Diagnostics;

namespace SEnerji.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var url = "https://esarj.com/harita"; // Harita sayfasının URL'si
            var httpClient = new HttpClient();

            // User-Agent ekleyerek istek yapalım
            httpClient.DefaultRequestHeaders.Add("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

            try
            {
                var html = await httpClient.GetStringAsync(url);

                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);

                var iframeNode = document.DocumentNode.SelectSingleNode("//iframe");
                var iframeSrc = iframeNode?.GetAttributeValue("src", string.Empty);

                ViewBag.MapUrl = iframeSrc;
            }
            catch (HttpRequestException ex)
            {
                ViewBag.ErrorMessage = "Erişim engellendi: " + ex.Message;
            }

            return View();
        }
       

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
