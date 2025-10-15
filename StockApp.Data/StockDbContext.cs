using Microsoft.EntityFrameworkCore;
using StockApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Data
{
    public class StockDbContext : DbContext
    {
        public StockDbContext(DbContextOptions<StockDbContext> options) : base(options)
        {
        }

        public DbSet<Urun> Urunler { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Urun>().HasData(
                new Urun { Id = 1, Adi = "Laptop", StokMiktari = 50 },
                new Urun { Id = 2, Adi = "Mouse", StokMiktari = 200 }
            );
        }
    }
}
