using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Login
    {
        public int Id { get; set; }
        public string Identity { get; set; }
        public int CustomerId { get; set; }
        public int PersonelId { get; set; }
        public Customer Customer { get; set; }
        public Personel Personel { get; set; }
    }
}
