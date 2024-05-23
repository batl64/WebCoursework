using WebAppShares.Data.Identity;
using WebAppShares.Data.Migrations;

namespace WebAppShares.Interface
{
    public interface IProductService
    {
        Task<IList<ProductsModel>> GetAllProductsAsync();
        Task<ProductsModel> GetProductByIdAsync(int id);
        IEnumerable<ProductsModel> GetProducts();
    }
}
