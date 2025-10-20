using System.ComponentModel.DataAnnotations;

namespace StockApp.Data
{
    public class StokHareketleriDto
    {
        public int Id { get; set; }
        public DateTime HareketTarihi { get; set; } = DateTime.UtcNow;
        public string HareketTipi { get; set; }
        public int Miktar { get; set; }
        public string Aciklama { get; set; }
        public int UrunId { get; set; }
    }
}
