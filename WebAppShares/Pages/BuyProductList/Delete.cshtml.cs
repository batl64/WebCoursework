using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebAppShares.Data;
using WebAppShares.Data.Identity;

namespace WebAppShares.Pages.BuyProductList
{
    [Authorize(Policy = "AdminPolicy")]
    public class DeleteModel : PageModel
    {
        private readonly WebAppShares.Data.ApplicationDbContext _context;

        public DeleteModel(WebAppShares.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BuyProduct BuyProduct { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buyproduct = await _context.BuyProducts.FirstOrDefaultAsync(m => m.Id == id);

            if (buyproduct == null)
            {
                return NotFound();
            }
            else
            {
                BuyProduct = buyproduct;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buyproduct = await _context.BuyProducts.FindAsync(id);
            if (buyproduct != null)
            {
                BuyProduct = buyproduct;
                _context.BuyProducts.Remove(BuyProduct);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
