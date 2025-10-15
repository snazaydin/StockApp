using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockApp.Data;
using StockApp.Data.Models;

namespace StockApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly StockDbContext _context;

    public ProductsController(StockDbContext context)
    {
        _context = context;
    }

    // GET: api/Products (Tüm ürünleri listele)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Urun>>> GetUrunler()
    {
        return await _context.Urunler.ToListAsync();
    }

    // GET: api/Products/5 (Belirli bir ürünü ID ile getir)
    [HttpGet("{id}")]
    public async Task<ActionResult<Urun>> GetUrun(int id)
    {
        var urun = await _context.Urunler.FindAsync(id);

        if (urun == null)
        {
            return NotFound(); // 404
        }

        return urun;
    }

    // POST: api/Products (Yeni ürün ekle)
    [HttpPost]
    public async Task<ActionResult<Urun>> PostUrun(Urun urun)
    {   
        // ID'yi sıfırla, veritabanı otomatik atayacaktır.
        urun.Id = 0;

        _context.Urunler.Add(urun);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUrun), new { id = urun.Id }, urun);
    }

    // --- YENİ EKLENEN METOTLAR ---

    // PUT: api/Products/5 (Ürün güncelleme)
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUrun(int id, Urun urun)
    {
        // Gelen ID ile gövdedeki ID eşleşmeli
        if (id != urun.Id)
        {
            return BadRequest(new { Message = "ID mismatch: URL'deki ID ile gönderilen ürün ID'si aynı olmalıdır." }); // 400
        }

        // Değişiklikleri takip etmeye başla
        _context.Entry(urun).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            // Eğer ürün veritabanında yoksa (silinmiş olabilir)
            if (!UrunExists(id))
            {
                return NotFound(); // 404
            }
            else
            {
                throw;
            }
        }

        return NoContent(); // 204 (Başarılı, yanıt gövdesi yok)
    }

    // DELETE: api/Products/5 (Ürün silme)
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUrun(int id)
    {
        var urun = await _context.Urunler.FindAsync(id);
        if (urun == null)
        {
            return NotFound(); // 404
        }

        _context.Urunler.Remove(urun);
        await _context.SaveChangesAsync();

        return NoContent(); // 204 (Başarılı, yanıt gövdesi yok)
    }

    // Helper metot: Ürünün var olup olmadığını kontrol eder
    private bool UrunExists(int id)
    {
        return _context.Urunler.Any(e => e.Id == id);
    }
}
