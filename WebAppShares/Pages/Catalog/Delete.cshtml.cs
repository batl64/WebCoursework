using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebAppShares.Data;
using WebAppShares.Data.Identity;

namespace WebAppShares.Pages.Catalog
{
    public class DeleteModel : PageModel
    {
        private readonly WebAppShares.Data.ApplicationDbContext _context;

        public DeleteModel(WebAppShares.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Basket _Basket { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basket = await _context.Baskets.Include(x=>x.Product).FirstOrDefaultAsync(m => m.Id == id);

            if (basket == null)
            {
                return NotFound();
            }
            else
            {
                _Basket = basket;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basket = await _context.Baskets.FindAsync(id);
            if (basket != null)
            {
                _Basket = basket;
                _context.Baskets.Remove(_Basket);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./basket");
        }
    }
}
