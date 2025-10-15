using Microsoft.EntityFrameworkCore;
using StockApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext - PostgreSQL'e ge�i� i�in UseNpgsql kullan�ld�
builder.Services.AddDbContext<StockDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        // CRITICAL FIX: Ba�lant� esnekli�i (retry) �zelli�i do�ru �ekilde eklendi
        o => o.EnableRetryOnFailure()
    )
);

var app = builder.Build();

// Apply migrations automatically
// Uygulama Geli�tirme (Development) ortam�nda de�ilse (yani terminalden �al���yorsa) migration'lar� uygula.
if (!app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<StockDbContext>();
        // Debug modunda hata almamak i�in bu sat�r Development ortam�nda atlanacak.
        dbContext.Database.Migrate();
    }
}


app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StockApp v1"));
app.MapGet("/", () => Results.Redirect("/swagger")); // K�k dizin y�nlendirmesi



app.UseAuthorization();

app.MapControllers();

app.Run();