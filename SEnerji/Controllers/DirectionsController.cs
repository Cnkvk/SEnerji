using Microsoft.AspNetCore.Mvc;
using SEnerji.Models;

namespace SEnerji.Controllers
{
    public class DirectionsController : Controller
    {
      
        public IActionResult Index(double? lat, double? lng)
        {
            // Model oluştur ve query string'den gelen değerleri ata
            var model = new DirectionsModel
            {
                Latitude = lat,
                Longitude = lng
            };

            // Modeli view'a gönder
            return View(model);
        }
        
    }
}
