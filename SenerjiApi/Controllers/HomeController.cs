using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;

namespace SenerjiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        // Web kazıma işlemi yaparak veri döndürme
        [Route("HomeScrap")]
        [AcceptVerbs("GET")]
        public async Task<IActionResult> HomeScrap()
        {
            try
            {
                var url = "https://www.sarj.dev/";  // Hedef URL

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");

                var response = await httpClient.GetAsync(url);

                // Response status code ve içerik
                var statusCode = response.StatusCode;
                var content = await response.Content.ReadAsStringAsync();

                // Loglama
                Console.WriteLine($"Status Code: {statusCode}");
                Console.WriteLine($"Response Content: {content}");

                response.EnsureSuccessStatusCode();

                var html = await response.Content.ReadAsStringAsync();

                var document = new HtmlAgilityPack.HtmlDocument();
                document.LoadHtml(html);

                var iframeNode = document.DocumentNode.SelectSingleNode("//iframe");
                var iframeSrc = iframeNode?.GetAttributeValue("src", string.Empty);

                if (string.IsNullOrEmpty(iframeSrc))
                {
                    return NotFound("Harita URL'si bulunamadı.");
                }

                var result = new
                {
                    MapUrl = iframeSrc
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }
    }
}

