using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppShares.Data.Identity;
using WebAppShares.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace WebAppShares.Pages.Catalog
{
    public class BasketModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public BasketModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Basket> Basket { get; set; }

        public decimal totalPrice { get; set; }

        public decimal tax { get; set; }

        public decimal total { get; set; }

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Basket = await _context.Baskets.Include(x => x.Product).Include(x=> x.Product.Image).Where(x=>x.UserId == userId).ToListAsync();
            totalPrice = await _context.Baskets.Include(b => b.Product).Where(b => b.UserId == userId).SumAsync(b => b.Product.Price * (1 - ((decimal)b.Product.Discount / 100)) * b.Quantity);
            totalPrice= Math.Round(totalPrice);
            tax = totalPrice *0.2m;
            total= tax + totalPrice;
        }
    }
}
