using Microsoft.AspNetCore.Mvc;
using Store.Services;

namespace Store.Controllers
{
    public class ProductController : Controller
    {
       public readonly StoreContextService storeContextService; 
        public ProductController(StoreContextService _context)
        {
            storeContextService = _context; 
        }

        public IActionResult Index()
        {
            var Products=storeContextService.Products.OrderByDescending(x=>x.Id).ToList();
            return View(Products);
        }


        public IActionResult Create()
        {



            return View();  

        }
    }
}
