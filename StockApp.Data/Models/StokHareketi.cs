using System;

using System.ComponentModel.DataAnnotations;

using System.Text.Json.Serialization;


namespace StockApp.Data.Models
{
    public class StokHareketi
    {
        public int Id { get; set; }
        public DateTime HareketTarihi { get; set; } = DateTime.UtcNow; 
        public string HareketTipi { get; set; } 
        public int Miktar { get; set; }
        public string Aciklama { get; set; }



        [Required]
        public int UrunId { get; set; }
      
    }
}
