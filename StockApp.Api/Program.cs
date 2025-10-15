using Microsoft.EntityFrameworkCore;
using StockApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext - PostgreSQL'e geçiþ için UseNpgsql kullanýldý
builder.Services.AddDbContext<StockDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        // CRITICAL FIX: Baðlantý esnekliði (retry) özelliði doðru þekilde eklendi
        o => o.EnableRetryOnFailure()
    )
);

var app = builder.Build();

// Apply migrations automatically
// Uygulama Geliþtirme (Development) ortamýnda deðilse (yani terminalden çalýþýyorsa) migration'larý uygula.
if (!app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<StockDbContext>();
        // Debug modunda hata almamak için bu satýr Development ortamýnda atlanacak.
        dbContext.Database.Migrate();
    }
}


app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StockApp v1"));
app.MapGet("/", () => Results.Redirect("/swagger")); // Kök dizin yönlendirmesi



app.UseAuthorization();

app.MapControllers();

app.Run();