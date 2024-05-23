using System.ComponentModel.DataAnnotations.Schema;
using WebAppShares.Models;

namespace WebAppShares.Data.Identity
{
    public class ProductsModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public string Description { get; set; }

        public ImageModel Image { get; set; }

        public decimal Price { get; set; }

        public int Discount { get; set; }
    }
}
