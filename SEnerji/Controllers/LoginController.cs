using DataBaseLayer;
using EntityLayer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SEnerji.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context = new();
        [HttpPost]
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
