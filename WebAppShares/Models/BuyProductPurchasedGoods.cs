using WebAppShares.Data.Identity;

namespace WebAppShares.Models
{
    public class BuyProductPurchasedGoods
    {
        public int Id { get; set; }

        public ProductsModel ProductsModelValue { get; set; }
        public int ProductsModelId { get; set; }

        public BuyProduct BuyProductValue { get; set; }
        public int BuyProductId { get; set; }

        public int Quantity { get; set; }
    }
}
