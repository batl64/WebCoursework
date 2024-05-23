using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using WebAppShares.Data.Identity;
using WebAppShares.Data.Migrations;

namespace WebAppShares.Pages.BuyProductList
{
    [Authorize(Policy = "AdminPolicy")]
    public class IndexModel : PageModel
    {
        private readonly WebAppShares.Data.ApplicationDbContext _context;

        public IndexModel(WebAppShares.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<BuyProduct> BuyProduct { get;set; } = default!;

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; } = 10;

        public int TotalItems { get; set; }

        public int StartPage { get; set; }

        public int EndPage { get; set; }

        public async Task OnGetAsync(string SearchText = "", int page=1)
        {
            BuyProduct = await _context.BuyProducts.ToListAsync();

            IList<BuyProduct> prod = BuyProduct;
            
            IList<BuyProduct> product;
            /*
            if (SearchText != "" && SearchText != null)
            {
                prod = await _context.BuyProducts.OrderByDescending(x => x.Id).Where(p => p.FirstName.Contains(SearchText) ||  p.LastName.Contains(SearchText) || p.totalSum.ToString().Contains(SearchText) || p.phoneNumber.Contains(SearchText) || p.Email.Contains(SearchText)).ToListAsync();

            }
            */
            product = prod;
          
            int skip = (page - 1) * PageSize;
            prod = prod.Skip(skip).Take(PageSize).ToList();

            int totalPages = (int)Math.Ceiling((decimal)product.Count / (decimal)10);
            int currentPage = page;

            int startPage = currentPage - 5;
            int endPage = currentPage + 4;

            if (startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }
            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }
            TotalPages = totalPages;
            CurrentPage = currentPage;
            TotalItems = product.Count;
            StartPage = startPage;
            EndPage = endPage;
            ViewData["TotalPages"] = TotalPages;
            ViewData["PageSize"] = 10;
            ViewData["TotalItems"] = TotalItems;
            ViewData["StartPage"] = StartPage;
            ViewData["CurrentPage"] = CurrentPage;
            ViewData["EndPage"] = endPage;

            BuyProduct = prod;
        }
    }
}
