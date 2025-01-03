using Microsoft.AspNetCore.Mvc;

namespace SEnerji.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
