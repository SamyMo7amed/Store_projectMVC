using Microsoft.EntityFrameworkCore;
using Store.Models;

namespace Store.Services
{
    public class StoreContextService :DbContext

    {
        public StoreContextService(DbContextOptions options):base(options) {
        
        
        }

        public DbSet<Product> Products { get; set; }    
    }
}
