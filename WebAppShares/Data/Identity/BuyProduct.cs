using Microsoft.AspNetCore.Identity;
using WebAppShares.Models;

namespace WebAppShares.Data.Identity
{
    public class BuyProduct
    {
        public int Id { get; set; }

        public IdentityUser User { get; set; }

        public List<BuyProductPurchasedGoods> PurchasedGoods { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Adress { get; set; }

        public string phoneNumber { get; set; }

        public decimal totalSum { get; set; }
    }
}
