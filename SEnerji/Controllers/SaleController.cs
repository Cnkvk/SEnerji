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
            var sales = _context.sales
       .Join(
           _context.customers,
           sale => sale.CustomerId,
           customer => customer.Id,
           (sale, customer) => new SaleDTO
           {
               SocketType = sale.SocketType,
               SalesQty = sale.SalesQty,
               Price = sale.Price,
               SalesDate = sale.SalesDate,
               CustomerId = sale.CustomerId,
               CustomerPlate = customer.Plate,
               Identity = customer.Identity,
               Status = customer.Status,
               CustomerName = customer.Name,
               CustomerSurname = customer.Surname// Customer tablosundaki Name alanı
           }).ToList();
            if (sales == null || !sales.Any())
            {
                // Boş liste durumu
                return Content("No sales data found.");
            }

            return View(sales);
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
                CustomerId = customer.Id,
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
        [HttpGet]
        public IActionResult GetSaleById(int CustomerId)
        {
            var sale = _context.sales
         .Join(
             _context.customers,
             sale => sale.CustomerId,
             customer => customer.Id,
             (sale, customer) => new SaleDTO
             {
                 CustomerId = sale.CustomerId,
                 CustomerPlate = customer.Plate,
                 Identity = customer.Identity,
                 SocketType = sale.SocketType // Eğer SocketType'ı da almak isterseniz
             })
         .FirstOrDefault(s => s.CustomerId == CustomerId);  // CustomerId'yi kullanarak filtreleme yapıyoruz

            // Eğer satış bulunamazsa 404 döndürüyoruz


            // Satışı JSON olarak döndürüyoruz
            return Json(sale);

        }

        public IActionResult SalePage() {
            return View();
        }

    }
}

