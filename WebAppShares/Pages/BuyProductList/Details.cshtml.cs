using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebAppShares.Data;
using WebAppShares.Data.Identity;
using WebAppShares.Models;

namespace WebAppShares.Pages.BuyProductList
{
    [Authorize(Policy = "AdminPolicy")]
    public class DetailsModel : PageModel
    {
        private readonly WebAppShares.Data.ApplicationDbContext _context;

        public DetailsModel(WebAppShares.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public BuyProduct BuyProduct { get; set; } = default!;

        public List<BuyProductPurchasedGoods> BuyProductList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buyproduct = await _context.BuyProducts
                .Include(x=>x.User)
                .Include(x => x.PurchasedGoods)
                .FirstOrDefaultAsync(m => m.Id == id);

            var buyproductList = await _context.BuyProductPurchasedGoods
                .Include(x=>x.BuyProductValue)
                .Include(x=>x.ProductsModelValue)
                .Where(x=>x.BuyProductId == id)
                .ToListAsync();

            if (buyproduct == null)
            {
                return NotFound();
            }
            else
            {
                BuyProduct = buyproduct;
                BuyProductList = buyproductList;
            }
            return Page();
        }
    }
}
