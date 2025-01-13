using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Sale
    {
        public int? Id { get; set; }
        public string? SocketType { get; set; }
        public string? SalesQty { get; set; }
        public decimal? Price { get; set; }
        public DateTime? SalesDate { get; set; }
        public int? CustomerId { get; set; }
    }
}
