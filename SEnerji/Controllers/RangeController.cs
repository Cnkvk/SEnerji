using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace SEnerji.Controllers
{
    public class RangeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(); // Fiyat hesaplama sayfası
        }

        // POST: Range/Hesapla
        [HttpPost]
        public IActionResult Hesapla(List<string> options, int chargeLevel)
        {
            // Fiyat hesaplama
            decimal totalPrice = 0;

            // Fiyatlar (Şarj istasyonu türlerine göre sabit fiyatlar)
            var fiyatlar = new Dictionary<string, decimal>
            {
                { "evde-duvar-prizi", 500 },   // Evde Duvar Prizi
                { "evde-wallbox", 1000 },      // Wallbox
                { "ac-sarj", 1500 },           // AC Şarj İstasyonu
                { "dc-hizli-sarj", 3000 },     // DC Hızlı Şarj (60 kW'a kadar)
                { "dc-hizli-sarj-60", 5000 }   // DC Hızlı Şarj (60 kW üstü)
            };

            // Seçilen seçeneklere göre fiyatları topla
            foreach (var option in options)
            {
                if (fiyatlar.ContainsKey(option))
                {
                    totalPrice += fiyatlar[option]; // Fiyatı ekle
                }
            }

            // Şarj seviyesini hesaba kat
            decimal chargeMultiplier = 1 + (chargeLevel / 100m); // Şarj oranını 0-1 arası çevir
            totalPrice *= chargeMultiplier; // Toplam fiyatı şarj oranına göre çarp

            // Hesaplanan fiyatı View'e gönder
            return View("Index", totalPrice); // Fiyatı geri gönderiyoruz
        }
    }
}

