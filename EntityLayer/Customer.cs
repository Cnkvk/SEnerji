using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class Customer
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? City { get; set; }
        public string? Password { get; set; }
        public string? Identity { get; set; }
        public string? Plate { get; set; }
        public string? Email { get; set; }
        public DateTime? Birthday { get; set; }
        public int Status { get; set; }
        public ICollection<Sale>? Sales { get; set; }

    }
}
