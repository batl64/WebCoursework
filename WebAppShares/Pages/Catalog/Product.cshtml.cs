using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppShares.Data.Identity;
using WebAppShares.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace WebAppShares.Pages.Catalog
{
    public class ProductModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ProductModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public ProductsModel  Product { get; set; }
        public IList<ProductsModel> ProductsListRec { get; set; }
        public bool stop {  get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await _context.Products.Include(x => x.Image).FirstOrDefaultAsync(m => m.Id == id);
            ProductsListRec = await _context.Products.Include(x => x.Image).Where(x=>x.Id != id).Take(4).ToListAsync();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
          
                return Page();
            }
            var check = await _context.Baskets.FirstOrDefaultAsync(x => x.UserId == user.Id && x.ProductId == id);
            if (check != null)
            {
                stop = true;
                return Page();
            }

            if (Product == null)
            {
                return NotFound();
            }
            stop = false;
            return Page();
        }

        [BindProperty]
        public int Quantity { get; set; }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var product = await _context.Products.FindAsync(id);

            Product = await _context.Products.Include(x => x.Image).FirstOrDefaultAsync(m => m.Id == id);
            ProductsListRec = await _context.Products.Include(x => x.Image).Where(x => x.Id != id).Take(4).ToListAsync();

            if (user == null || product == null)
            {
                return NotFound();
            }
            var check = await _context.Baskets.FirstOrDefaultAsync(x => x.UserId == user.Id && x.ProductId == id);
            if (check != null)
            {
                stop = true;
                ModelState.AddModelError(string.Empty, "Помилка, у вас куплений даний товар");
                return Page();
            }
            stop = false;
            var basket = new Basket
            {
                User = user,
                Product = product,
                Quantity = Quantity
            };

            _context.Baskets.Add(basket);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }

    }
}
