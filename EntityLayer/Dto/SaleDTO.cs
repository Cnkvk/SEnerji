using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Dto
{
    public class SaleDTO
    {
        public int Id { get; set; }
        public string? SocketType { get; set; }
        public string? SalesQty { get; set; }
        public decimal? Price { get; set; }
        public DateTime? SalesDate { get; set; } 
        public int CustomerId { get; set; }
        public string? Plate { get; set; }
        public string? Identity { get; set; }
        public int Status { get; set; }
    }
}
