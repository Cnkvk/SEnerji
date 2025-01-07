using Microsoft.AspNetCore.Mvc;

namespace SEnerji.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
