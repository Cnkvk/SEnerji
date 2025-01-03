using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Stock
    {
        public int Id { get; set; }
        public string SocketType { get; set; }
        public decimal Price { get; set; }
        public string PowerRange { get; set; }
    }
}
