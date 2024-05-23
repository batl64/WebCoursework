using Microsoft.AspNetCore.Mvc;

namespace WebAppShares.Models
{
    public class ProductFilter
    {
        public string Name { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
