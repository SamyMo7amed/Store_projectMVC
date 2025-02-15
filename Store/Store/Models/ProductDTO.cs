using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Store.Models
    // this class to collect data from user to create new product 
{
    public class ProductDTO
    {
        [Required,MaxLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }
        [Required,MaxLength(100)]
        public string Brand { get; set; }
        [Required,MaxLength(100)]
        public string Category { get; set; }
        [Required]
        public int Price { get; set; }
  
        public IFormFile? ImageFile { get; set; }

    }
}
