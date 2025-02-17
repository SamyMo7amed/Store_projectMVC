using Microsoft.AspNetCore.Mvc;
using Store.Models;
using Store.Services;

namespace Store.Controllers
{
    public class ProductController : Controller
    {
        public readonly StoreContextService storeContextService;
        public readonly IWebHostEnvironment webHostEnvironment;
        public ProductController(StoreContextService _context, IWebHostEnvironment _webHostEnvironment)
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

            Product product = new Product();
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
        public IActionResult Edit(int id)
        {

            var product = storeContextService.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            var productDTO = new ProductDTO()
            {
                Name = product.Name,
                Brand = product.Brand,
                Category = product.Category,
                Price = product.Price,
                Description = product.Description
            };
            ViewData["ProductId"] = product.Id;
            ViewData["Image"] = product.ImageFileName;
            ViewData["CreateAt"] = product.CreatedAt.ToString("yyyy/MM/dd");
            return View(productDTO);


        }
        [HttpPost]
        public IActionResult Edit(int id, ProductDTO productDTO)
        {

            var product = storeContextService.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                ViewData["ProductId"] = product.Id;
                ViewData["Image"] = product.ImageFileName;
                ViewData["CreateAt"] = product.CreatedAt.ToString("yyyyMMdd");

                return View(productDTO);

            }//

            //UPEAT THE IMAGE IF WE HAve a new image

            string newfileName=product.ImageFileName;
            if(productDTO.ImageFile != null)
            {
                newfileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newfileName += Path.GetExtension(productDTO.ImageFile.FileName);

                string ImageFullPath= webHostEnvironment.WebRootPath+"/images/"+newfileName;
                using (var stream = System.IO.File.Create(ImageFullPath))
                {

                    productDTO.ImageFile.CopyTo(stream);
                }

                //delete the old image
                string old = webHostEnvironment.WebRootPath + "/images/" + product.ImageFileName;
                System.IO.File.Delete(old); 

            }
            // update the product in the database
            product.Name = productDTO.Name; 
            product.Description = productDTO.Description;
            product.Price = productDTO.Price;   
            product.Category = productDTO.Category;
            product.ImageFileName = newfileName;
            storeContextService.SaveChanges();
            return RedirectToAction("Index");   
             
        }

        public IActionResult Delete(int id) {
            var product = storeContextService.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                storeContextService.Products.Remove(product);
                storeContextService.SaveChanges();
                return RedirectToAction("Index");
            }


        }
    }

}