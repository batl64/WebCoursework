using WebAppShares.Data;
using WebAppShares.Interface;
using Microsoft.EntityFrameworkCore;
using WebAppShares.Data.Identity;
using WebAppShares.Data.Migrations;

namespace WebAppShares.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IList<ProductsModel>> GetAllProductsAsync()
        {
            return await _context.Products.Include(x => x.Image).ToListAsync();
        }

        public async Task<ProductsModel> GetProductByIdAsync(int id)
        {
            return await _context.Products.Include(x => x.Image)
                .FirstOrDefaultAsync(m => m.Id == id); ;
        }

        public IEnumerable<ProductsModel> GetProducts()
        {
            return _context.Products.Include(x => x.Image);
        }
    }
}
