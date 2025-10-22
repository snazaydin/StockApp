using Microsoft.AspNetCore.Mvc;

namespace StockApp.Api.Controllers;

public class HomeController : Controller 
{
   
    public IActionResult Index()
    {
        
        return View();
    }

    
}