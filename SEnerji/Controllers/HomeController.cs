using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SEnerji.Models;
using System.Diagnostics;
using System.IO.Compression;
using System.Net;
using static System.Net.WebRequestMethods;

namespace SEnerji.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        private static readonly string ApiUrl = "https://api.openchargemap.io/v3/poi";
        private static readonly string ApiKey = "1cff6936-69cb-4e68-80df-d479970e9251"; // API Anahtarınız

        private static readonly string CountryCode = "TR"; // Türkiye'nin ülke kodu
        private static readonly int MaxResults = 200; // İstediğiniz sayıda şarj istasyonu (isteğe bağlı)

        public async Task<IActionResult> Index()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // API'ye gerekli parametrelerle istek gönderiyoruz
                    string url = $"{ApiUrl}?countrycode={CountryCode}&maxresults={MaxResults}&key={ApiKey}";

                    // API'den veri alıyoruz
                    var response = await client.GetStringAsync(url);

                    // JSON verisini deserialize ediyoruz
                    var chargingStationsResponse = JsonConvert.DeserializeObject<List<ChargingStationResponse>>(response);

                    // Yalnızca AddressInfo ve Level bilgilerini alıp dönüştürüyoruz
                    var chargingStations = new List<ChargingStation>();

                    foreach (var station in chargingStationsResponse)
                    {
                        var chargingStation = new ChargingStation
                        {
                            Title = station.AddressInfo.Title,
                            AddressLine1 = station.AddressInfo.AddressLine1,
                            AddressLine2 = station.AddressInfo.AddressLine2,
                            Town = station.AddressInfo.Town,
                            StateOrProvince = station.AddressInfo.StateOrProvince,
                            Postcode = station.AddressInfo.Postcode,
                            Latitude = station.AddressInfo.Latitude,
                            Longitude = station.AddressInfo.Longitude,
                            LevelTitle = station.Connections.FirstOrDefault()?.Level.Title ?? "N/A",
                            LevelComments = station.Connections.FirstOrDefault()?.Level.Comments ?? "N/A",
                            IsFastChargeCapable = station.Connections.FirstOrDefault()?.Level.IsFastChargeCapable ?? false
                        };

                        chargingStations.Add(chargingStation);
                    }
                    ViewData["Markers"] = JsonConvert.SerializeObject(chargingStations);
                    // Şarj istasyonlarını View'a gönderiyoruz
                    return View();
                }
            }
            catch (System.Exception ex)
            {
                // Hata durumunda kullanıcıyı bilgilendiriyoruz
                ViewData["ErrorMessage"] = "API'den veri alınırken bir hata oluştu: " + ex.Message;
                return View();
            }
        }


        public class ChargingStation
    {
        // AddressInfo kısmındaki veriler
        public string Title { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Town { get; set; }
        public string StateOrProvince { get; set; }
        public string Postcode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // Level bilgileri
        public string LevelTitle { get; set; }
        public string LevelComments { get; set; }
        public bool IsFastChargeCapable { get; set; }
    }

    public class ChargingStationResponse
    {
        public AddressInfo AddressInfo { get; set; }
        public List<Connection> Connections { get; set; }
    }

    public class AddressInfo
    {
        public string Title { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Town { get; set; }
        public string StateOrProvince { get; set; }
        public string Postcode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class Connection
    {
        public Level Level { get; set; }
    }

    public class Level
    {
        public string Title { get; set; }
        public string Comments { get; set; }
        public bool IsFastChargeCapable { get; set; }
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
