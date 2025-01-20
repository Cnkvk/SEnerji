using DataBaseLayer;
using EntityLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SEnerji.Controllers
{
    public class PersonelController : Controller
    {
        
        private readonly ApplicationDbContext _context = new();

        [Authorize(Policy = "PersonelOnly")]
        public IActionResult Index()
        {
            var Personel = _context.personels
                               .Where(p => p.Status != 0)
                               .ToList();
            return View(Personel);
        }
        [HttpPost]
        public IActionResult AddPersonel([FromBody]Personel personel)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Model hatalı", errors });
            }
            personel.Status = 1;
            _context.Add(personel);
            _context.SaveChanges();
            return Json(new { success = true, message = "Veriler başarıyla kaydedildi." });

        }
        [HttpPost]
        public IActionResult UpdatePersonel([FromBody] Personel personel)
        {
            var Personeller = _context.personels.Where(x => x.Id == personel.Id).FirstOrDefault();
            if (personel == null)
            {
                return BadRequest();
            }
            Personeller.Name = string.IsNullOrWhiteSpace(personel.Name) ? Personeller.Name : personel.Name;
            Personeller.Surname = string.IsNullOrWhiteSpace(personel.Surname) ? Personeller.Surname : personel.Surname;
            Personeller.Email = string.IsNullOrWhiteSpace(personel.Email) ? Personeller.Email : personel.Email;
            Personeller.Password = string.IsNullOrWhiteSpace(personel.Password) ? Personeller.Password : personel.Password;
            Personeller.Identity = string.IsNullOrWhiteSpace(personel.Identity) ? Personeller.Identity : personel.Identity;
            Personeller.City = string.IsNullOrWhiteSpace(personel.City) ? Personeller.City : personel.City;
            Personeller.Salary = decimal.TryParse(personel.Salary.ToString(), out decimal result) ? result : personel.Salary;
            Personeller.Status = 1;

            _context.personels.Update(Personeller);
            _context.SaveChanges();
            return Ok();
        }
        [HttpGet]
        public IActionResult GetPersonelById(int id)
        {
            var personel = _context.personels.FirstOrDefault(c => c.Id == id);

            // Eğer müşteri bulunamazsa, 404 dönüyoruz
            if (personel == null)
            {
                return NotFound();
            }

            return Json(personel); // Müşteri verisini JSON olarak döndürüyoruz
        }
        [HttpGet]
        public IActionResult RemovePersonel(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            var personel = _context.personels.Find(id);
            if (personel == null)
            {
                return NotFound();
            }
            personel.Status = 0;
            _context.personels.Update(personel);
            _context.SaveChanges();
            return Ok(personel);
        }

    }
}
