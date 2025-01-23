using EntityLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLayer
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext()
        {
            
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server = CNKVK\\CNKVK; Database = SEnerji; Trusted_Connection = True; TrustServerCertificate=True;");
                //optionsBuilder.UseSqlServer("Server = DESKTOP-EQOD4HE; Database = SEnerji; Trusted_Connection = True; TrustServerCertificate=True;");
            }
        }
        public DbSet<Customer> customers { get; set; }
        public DbSet<Personel> personels { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Sale> sales { get; set; }
        public DbSet<Stock> stocks { get; set; }
    }
}
