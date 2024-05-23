using Microsoft.AspNetCore.Identity;

namespace WebAppShares.Data.Identity
{
    public class Basket
    {
        public int Id { get; set; }

        public int ProductId { get; set; } 
        public ProductsModel Product { get; set; }

        public string UserId { get; set; } 
        public IdentityUser User { get; set; }

        public int Quantity { get; set; }
    }
}
