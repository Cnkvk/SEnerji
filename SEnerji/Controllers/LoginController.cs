using DataBaseLayer;
using EntityLayer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace SEnerji.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context = new();


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        public record LoginRequestModel
        {
            public string Identity { get; set; }
            public string Password { get; set; }

        }
        [HttpPost]
        public IActionResult SignIn([FromBody] LoginRequestModel loginRequestModel)
        {
            // İlk olarak müşteri tablosunda Identity kontrolü yap
            var customer = _context.customers.FirstOrDefault(c => c.Identity == loginRequestModel.Identity);

            if (customer != null) // Eğer müşteri bulunduysa
            {
                if (customer.Password == loginRequestModel.Password) // Şifre kontrolü
                {
                    // Kullanıcıyı oturum açtırmak için SignInUser metoduna yönlendirme
                    return SignInUser(customer.Id.ToString(), customer.Name, customer.Surname, "Customer");
                }
                else
                {
                    return BadRequest("Müşteri şifresi hatalı.");
                }
            }

            // Eğer müşteri bulunamadıysa, personel tablosunda Identity kontrolü yap
            var personel = _context.personels.FirstOrDefault(p => p.Identity == loginRequestModel.Identity);

            if (personel != null) // Eğer personel bulunduysa
            {
                if (personel.Password == loginRequestModel.Password) // Şifre kontrolü
                {
                    // Kullanıcıyı oturum açtırmak için SignInUser metoduna yönlendirme
                    return SignInUser(personel.Id.ToString(), personel.Name, personel.Surname, "Personel");
                }
                else
                {
                    return BadRequest("Personel şifresi hatalı.");
                }
            }

            // Eğer Identity hiçbir tabloda bulunamazsa
            return BadRequest("Kimlik bilgisi doğrulanamadı.");
        }

        // Kullanıcıyı oturum açmış olarak işaretlemek için ortak yöntem
        private IActionResult SignInUser(string id, string Name, string Surname, string role)
        {
            // Claim listesi oluştur
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("Name",Name));
            claims.Add(new Claim("Surname", Surname));
            claims.Add(new Claim("Rol", role));


            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);

            AuthenticationProperties properties = new AuthenticationProperties
            {
                IsPersistent = true, // Çerez kalıcı olsun
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30) // Çerez geçerlilik süresi
            };



      
            // Oturum aç
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);

            // Çerez ekle
            Response.Cookies.Append("UserName", $"{Name}", new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(30), // Çerezin geçerlilik süresi (30 dakika)
                HttpOnly = false, // Çerez sadece HTTP üzerinden erişilebilir
                Secure = false, // Çerez sadece HTTPS üzerinden gönderilir
                SameSite = SameSiteMode.None // Cross-site scripting'e karşı koruma
            });
            Response.Cookies.Append("Surname", $"{Surname}", new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(30), // Çerezin geçerlilik süresi (30 dakika)
                HttpOnly = true, // Çerez sadece HTTP üzerinden erişilebilir
                Secure = true, // Çerez sadece HTTPS üzerinden gönderilir
                SameSite = SameSiteMode.Strict // Cross-site scripting'e karşı koruma
            });
            Response.Cookies.Append("Rol", $"{role}", new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(30), // Çerezin geçerlilik süresi (30 dakika)
                HttpOnly = true, // Çerez sadece HTTP üzerinden erişilebilir
                Secure = true, // Çerez sadece HTTPS üzerinden gönderilir
                SameSite = SameSiteMode.Strict // Cross-site scripting'e karşı koruma
            });


            return Ok(new { message = $"{role} olarak giriş yapıldı.", role });
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
