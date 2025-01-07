using Microsoft.AspNetCore.Mvc;

namespace SEnerji.Controllers
{
    public class PersonelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
