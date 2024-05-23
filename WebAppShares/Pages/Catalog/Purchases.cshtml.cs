using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebAppShares.Data;
using WebAppShares.Data.Identity;
using WebAppShares.Data.Migrations;
using WebAppShares.Models;

namespace WebAppShares.Pages.Catalog
{
    public class PurchasesModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public PurchasesModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Basket> Basket { get; set; }

        public decimal totalPrice { get; set; }

        public decimal tax { get; set; }

        public decimal total { get; set; }

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Basket = await _context.Baskets.Include(x => x.Product).Include(x => x.Product.Image).Where(x => x.UserId == userId).ToListAsync();
            totalPrice = await _context.Baskets.Include(b => b.Product).Where(b => b.UserId == userId).SumAsync(b => b.Product.Price * (1 - ((decimal)b.Product.Discount / 100)) * b.Quantity);
            totalPrice = Math.Round(totalPrice);
            tax = totalPrice * 0.2m;
            total = tax + totalPrice;
        }

        [BindProperty]
        public string FirstName { get; set; }

        [BindProperty]
        public string LastName { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string PhoneNumber { get; set; }

        [BindProperty]
        public string Adress { get; set; }
        

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
          
            var basket = await _context.Baskets.Include(x => x.Product).Include(x => x.Product.Image).Where(x => x.UserId == user.Id).ToListAsync();
            totalPrice = await _context.Baskets.Include(b => b.Product).Where(b => b.UserId == user.Id).SumAsync(b => b.Product.Price * (1 - ((decimal)b.Product.Discount / 100)) * b.Quantity);
            totalPrice = Math.Round(totalPrice);
            tax = totalPrice * 0.2m;
            total = tax + totalPrice;

            if (user == null || basket == null)
            {
                return NotFound();
            }

            var buy = new BuyProduct
            {
                User = user,
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                phoneNumber = PhoneNumber,
                totalSum = total,
                Adress= Adress

            };
         
            List < BuyProductPurchasedGoods > listBuyProduct = new();
            List<Basket> BasketsStop = new();
            foreach (var b in basket) {
                if (b.Product.Quantity >= b.Quantity)
                {

                    b.Product.Quantity = b.Product.Quantity - b.Quantity;
                    listBuyProduct.Add(new BuyProductPurchasedGoods
                    {
                        ProductsModelValue = b.Product,
                        ProductsModelId = buy.Id,
                        BuyProductValue = buy,
                        BuyProductId = b.ProductId,
                        Quantity = b.Quantity,
                    });

                }
                else
                {
                    BasketsStop.Add(b);
                    
                    
                }
            }
            if (BasketsStop.Count > 0)
            {
                _context.Baskets.RemoveRange(BasketsStop);
                await _context.SaveChangesAsync();
                ModelState.AddModelError(string.Empty, "Помилка, товар який ви хотіли купити вже закінчився або хотіли купиити більше чи його в наявності");
                return Page();
            }
            buy.PurchasedGoods = listBuyProduct;
            _context.BuyProducts.Add(buy);

           
            _context.Baskets.RemoveRange(basket);
            await _context.SaveChangesAsync();

            return RedirectToPage("Basket");
        }
    }
}
