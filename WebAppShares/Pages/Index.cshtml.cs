using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebAppShares.Data;
using WebAppShares.Data.Identity;

namespace WebAppShares.Pages
{

    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _context;

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IList<ProductsModel> Products { get; set; }

        public async Task OnGetAsync()
        {
            Products = await _context.Products
                .Include(x => x.Image)
                .Take(8)
                .ToListAsync();
        }
    }
}
