using Microsoft.AspNetCore.Mvc;

namespace SEnerji.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
       public IActionResult SignUp()
            {
                return View();
            }
        public IActionResult ResetPassword()
        {
            return View();
        }
    }
    
}
