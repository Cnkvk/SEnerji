using DataBaseLayer;
using EntityLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace SEnerji.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context = new();
        public IActionResult Index()
        {
            var Customer = _context.customers
                                .Where(p => p.Status != 0)
                                .ToList();
            return View(Customer);
            
        }
        [HttpGet]
        public IActionResult GetCustomerById(int id)
        {
            var customer = _context.customers.FirstOrDefault(c => c.Id == id);

            // Eğer müşteri bulunamazsa, 404 dönüyoruz
            if (customer == null)
            {
                return NotFound();
            }

            return Json(customer); // Müşteri verisini JSON olarak döndürüyoruz
        }
        [HttpPost]
        public IActionResult AddCustomer([FromBody]Customer customer)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Model hatalı", errors });
            }
            customer.Status = 1;
            _context.Add(customer);
            _context.SaveChanges();
            return Json(new { success = true, message = "Veriler başarıyla kaydedildi." });

        }
        [HttpGet]
        public IActionResult RemoveCustomer(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            var customer = _context.customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }
            customer.Status = 0;
            _context.customers.Update(customer);
            _context.SaveChanges();
            return Ok(customer);
        }
    }
}
