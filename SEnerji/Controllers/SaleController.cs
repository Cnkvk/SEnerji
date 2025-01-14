using DataBaseLayer;
using EntityLayer;
using EntityLayer.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SEnerji.Controllers
{
    public class SaleController : Controller
    {
        private readonly ApplicationDbContext _context = new();
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddSale([FromBody] SaleDTO saleDTO)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Model hatalı", errors });
            }
            var customer = _context.customers.FirstOrDefault(c => c.Identity == saleDTO.Identity);
            if (customer == null)
            {
                return BadRequest(new { message = "Müşteri bulunamadı." });
            }

            // Sale objesini oluştur
            var sale = new Sale()
            {
                
                SocketType = saleDTO.SocketType,
                CustomerId = saleDTO.CustomerId,
                Price = saleDTO.Price,
                SalesDate = saleDTO.SalesDate ?? DateTime.Now, // Null kontrolü ekledim
                SalesQty = saleDTO.SalesQty
                
            };
            _context.sales.Add(sale);
            _context.SaveChanges();
            return Json(new { success = true, message = "Veriler başarıyla kaydedildi." });

        }

        [HttpPost]
        public IActionResult UpdateSale(int id, [FromBody] SaleDTO saleDTO)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Model hatalı", errors });
            }

            var sale = _context.sales.FirstOrDefault(s => s.Id == id);
            if (sale == null)
            {
                return NotFound(new { message = "Satış bulunamadı." });
            }

            // Sale objesini güncelle
            sale.SocketType = saleDTO.SocketType;
            sale.CustomerId = saleDTO.CustomerId;
            sale.Price = saleDTO.Price;
            sale.SalesDate = saleDTO.SalesDate ?? sale.SalesDate; // Null kontrolü eklenebilir
            sale.SalesQty = saleDTO.SalesQty;

            _context.SaveChanges();
            return Json(new { success = true, message = "Satış başarıyla güncellendi." });
        }

        [HttpGet]
        public IActionResult RemoveSale(int id)
        {
            var sale = _context.sales.FirstOrDefault(s => s.Id == id);
            if (sale == null)
            {
                return NotFound(new { message = "Satış bulunamadı." });
            }

            _context.sales.Remove(sale);
            _context.SaveChanges();
            return Json(new { success = true, message = "Satış başarıyla silindi." });
        }

    }
}
