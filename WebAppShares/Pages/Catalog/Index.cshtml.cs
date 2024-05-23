using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using WebAppShares.Data;
using WebAppShares.Data.Identity;
using WebAppShares.Data.Migrations;
using WebAppShares.Interface;
using WebAppShares.Models;
using WebAppShares.Services;
using WebAppShares.Views.Pagination;

namespace WebAppShares.Pages.Catalog
{
    public class IndexModel : PageModel
    {

        private readonly ApplicationDbContext _context;

        private readonly IProductService _productService;

        public IndexModel(ApplicationDbContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        public IList<ProductsModel> Products { get; set; }

        public ProductFilter filter { get; set; }


        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; } = 9;

        public int TotalItems { get; set; }

        public int StartPage { get; set; }

        public int EndPage { get; set; }


        public async Task OnGetAsync([FromQuery] ProductFilter filter, [FromQuery] int page = 1)
        {
            if (page == null || page < 1)
            {
                page = 1;
            }

            var products = _productService.GetProducts();


    
          
            if (!string.IsNullOrEmpty(filter.Name))
            {
                products = products.Where(p => p.Name.Contains(filter.Name));
            }

            if (filter.MinPrice.HasValue)
            {
                products = products
                    .Where(p => (Math.Round(p.Price * (1 - ((decimal)p.Discount / 100)), 2)) >= filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                products = products
                    .Where(p => (Math.Round(p.Price * (1 - ((decimal)p.Discount / 100)), 2)) <= filter.MaxPrice.Value);
            }

            if (products == null)
            {
                // Обробка випадку, коли products є null
                Products = new List<ProductsModel>();
                TotalPages = 0;
                CurrentPage = 1;
                StartPage = 1;
                EndPage = 1;
                return;
            }
            Products = products.ToList();

            int skip = (page - 1) * PageSize;
            var productSubset = Products.Skip(skip).Take(PageSize).ToList();

            int totalPages = (int)Math.Ceiling((decimal)Products.Count / PageSize);
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
            TotalItems = Products.Count;
            StartPage = startPage;
            EndPage = endPage;
            ViewData["TotalPages"] = TotalPages;
            ViewData["PageSize"] = PageSize;
            ViewData["TotalItems"] = TotalItems;
            ViewData["StartPage"] = StartPage;
            ViewData["CurrentPage"] = CurrentPage;
            ViewData["EndPage"] = EndPage;

            Products = productSubset;
        }




    }
}

