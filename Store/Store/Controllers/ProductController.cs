using Microsoft.AspNetCore.Mvc;
using Store.Models;
using Store.Services;

namespace Store.Controllers
{
    public class ProductController : Controller
    {
        public readonly StoreContextService storeContextService;
        public readonly IWebHostEnvironment webHostEnvironment;
        public ProductController(StoreContextService _context,IWebHostEnvironment  _webHostEnvironment)
        {
            storeContextService = _context;
            webHostEnvironment = _webHostEnvironment;
        }

        public IActionResult Index()
        {
            var Products = storeContextService.Products.OrderByDescending(x => x.Id).ToList();
            return View(Products);
        }


        public IActionResult Create()
        {



            return View();

        }

        [HttpPost]
        public IActionResult Create(ProductDTO productDTO)
        {
            if (productDTO.ImageFile == null)
            {
                ModelState.AddModelError("ImageFille", "Image Fille is Required");


            }
            else if (!ModelState.IsValid)
            {

                return View(productDTO);

            }
            
                // save Image


                
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");// to create uniqe name  by the date
                fileName += Path.GetExtension(productDTO.ImageFile.FileName);

                string imageFullPath = webHostEnvironment.WebRootPath/*to git the wwwroot path*/ + "/images/" + fileName;// to get the path the the image was saved in it 

                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    productDTO.ImageFile.CopyTo(stream);

                }


                // save the product in database

                Product product=new Product();
                product.Name = productDTO.Name; 
                product.Description = productDTO.Description;   
                product.Category = productDTO.Category;
                product.Price = productDTO.Price;
            product.Brand = productDTO.Brand;
                product.ImageFileName = fileName;
                product.CreatedAt = DateTime.Now;   
                
                storeContextService.Products.Add(product);  
                storeContextService.SaveChanges();  
                return RedirectToAction("Index");
            
            

            

        }

    }
}