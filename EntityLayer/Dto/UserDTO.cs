using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Dto
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? City { get; set; }
        public string? Identity { get; set; }

        public string? Password { get; set; }
        public string? Plate { get; set; } // Sadece Customer için dolu olabilir
        public decimal? Salary { get; set; } // Sadece Personel için dolu olabilir
        public DateTime? Birthday { get; set; } // Sadece Customer için dolu olabilir
        public string UserType { get; set; }
    }
}
