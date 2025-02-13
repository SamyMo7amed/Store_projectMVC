using Microsoft.EntityFrameworkCore;

namespace Store.Services
{
    public class StoreContextService :DbContext

    {
        public StoreContextService(DbContextOptions options):base(options) {
        
        
        }
    }
}
