using DataBaseLayer;
using EntityLayer.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace SEnerji.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context = new();

        public IActionResult Index()
        {
            var userIdString = User.FindFirst("id")?.Value;
            var userId = Convert.ToInt32(userIdString); // String'den int'e dönüşüm yapıyoruz

            var role = User.FindFirst("Rol")?.Value;

            // Eğer userId null veya boş ise yetkilendirme hatası döndürüyoruz
            if (string.IsNullOrEmpty(userId.ToString()))
            {
                return Unauthorized("Kullanıcı bilgisi bulunamadı.");
            }

            // UserDTO'yu oluşturuyoruz
            var userDTO = new UserDTO { Id = userId, UserType = role };

            // Eğer kullanıcı müşteri ise
            if (role == "Customer")
            {
                // Müşteri tablosunda kullanıcıyı buluyoruz
                var customer = _context.customers.FirstOrDefault(c => c.Id == userId);
                if (customer != null)
                {
                    // Müşteriye ait bilgileri DTO'ya ekliyoruz
                    userDTO.FullName = $"{customer.Name} {customer.Surname}";
                    userDTO.Email = customer.Email;
                    userDTO.City = customer.City;
                    userDTO.Identity = customer.Identity;
                    userDTO.Birthday = customer.Birthday;
                    userDTO.Plate = customer.Plate; // Sadece Customer için geçerli
                }
            }
            // Eğer kullanıcı personel ise
            else if (role == "Personel")
            {
                // Personel tablosunda kullanıcıyı buluyoruz
                var personel = _context.personels.FirstOrDefault(p => p.Id == userId);
                if (personel != null)
                {
                    // Personel'e ait bilgileri DTO'ya ekliyoruz
                    userDTO.FullName = $"{personel.Name} {personel.Surname}";
                    userDTO.Email = personel.Email;
                    userDTO.City = personel.City;
                    userDTO.Identity = personel.Identity;
                    
                    userDTO.Salary = personel.Salary; // Sadece Personel için geçerli
                }
            }

            // Sonuçları UserDTO modeline göre görüntülüyoruz
            return View(userDTO);
        }
    }
}


