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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Urun>>> GetUrunler()
    {

        return await _context.Urunler.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Urun>> GetUrun(int id)
    {
        var urun = await _context.Urunler.FindAsync(id);

        if (urun == null)
        {
            return NotFound();
        }

        return urun;
    }

    [HttpPost]
    public async Task<ActionResult<Urun>> PostUrun(Urun urun)
    {
        _context.Urunler.Add(urun);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUrun), new { id = urun.Id }, urun);
    }
}
