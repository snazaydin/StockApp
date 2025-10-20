using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockApp.Data;
using StockApp.Data.Models;
using System.Linq; 

namespace StockApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StokHareketleriController : ControllerBase
{
    private readonly StockDbContext _context;

    public StokHareketleriController(StockDbContext context)
    {
        _context = context;
    }

    // GET: api/StokHareketleri (Tüm hareketleri listele)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StokHareketi>>> GetStokHareketleri()
    {
        
        return await _context.StokHareketleri.ToListAsync();
    }

    // POST: api/StokHareketleri (Yeni stok hareketini kaydet ve stok miktarını güncelle)
    [HttpPost]
    public async Task<ActionResult<StokHareketi>> PostStokHareketi(StokHareketleriDto hareket)
    {
        StokHareketi yeniHareket = new StokHareketi
        {
            HareketTarihi = hareket.HareketTarihi,
            HareketTipi = hareket.HareketTipi,
            Miktar = hareket.Miktar,
            Aciklama = hareket.Aciklama,
            UrunId = hareket.UrunId
        };










        // Model doğrulama API'nin zorunlu alanlarını kontrol eder (UrunId ve Miktar)

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // 1. Ürünü Veritabanından Bul
        var urun = await _context.Urunler.FindAsync(hareket.UrunId);

        if (urun == null)
        {
            return NotFound(new { Message = $"ID'si {hareket.UrunId} olan ürün bulunamadı." });
        }

        // 2. Stok Miktarını Hesapla ve Güncelle
        if (hareket.HareketTipi == "Giriş")
        {
            urun.StokMiktari += hareket.Miktar;
        }
        else if (hareket.HareketTipi == "Çıkış")
        {
            // Çıkış yaparken stokta yeterli miktar var mı kontrolü
            if (urun.StokMiktari < hareket.Miktar)
            {
                return BadRequest(new { Message = "Stokta yeterli miktar bulunmamaktadır. Mevcut stok: " + urun.StokMiktari });
            }
            urun.StokMiktari -= hareket.Miktar;
        }
        else
        {
            return BadRequest(new { Message = "Geçersiz HareketTipi. 'Giriş' veya 'Çıkış' olmalıdır." });
        }

        // 3. Değişiklikleri Kaydet (Hem Stok Hareketi hem de Ürün Güncellemesi)
        _context.StokHareketleri.Add(yeniHareket);
        _context.Entry(urun).State = EntityState.Modified; // Ürün nesnesinin güncellendiğini belirt

        await _context.SaveChangesAsync();

        // Başarılı olduğunu belirt (HTTP 201 Created)
        return CreatedAtAction(nameof(GetStokHareketleri), new { id = hareket.Id }, hareket);
    }
}