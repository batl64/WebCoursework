using Microsoft.AspNetCore.Mvc;

namespace WebAppShares.Models
{
    public class ProductFilter
    {
        public string Name { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
    }
}
