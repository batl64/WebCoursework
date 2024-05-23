using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Moq;
using WebAppShares.Data;
using WebAppShares.Data.Identity;
using WebAppShares.Pages.Catalog;
using Xunit;
using WebAppShares.Models;

namespace TestWeb
{
    public class ProductModelTests
    {
        private readonly ApplicationDbContext _context;
        private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
        private readonly ProductModel _pageModel;

        public ProductModelTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _mockUserManager = new Mock<UserManager<IdentityUser>>(
                new Mock<IUserStore<IdentityUser>>().Object, null, null, null, null, null, null, null, null);

            _pageModel = new ProductModel(_context, _mockUserManager.Object);

    
            SeedData();
        }

        private void SeedData()
        {
            var products = new List<ProductsModel>
        {
            new ProductsModel { Name = "Test Product", Quantity = 10, Description = "Test Description", Price = 100, Discount = 10,Image=new ImageModel() },
            new ProductsModel { Name = "Test Product", Quantity = 10, Description = "Test Description", Price = 100, Discount = 10,Image=new ImageModel() }
        };

            _context.Products.AddRange(products);
            _context.SaveChanges();
        }

        [Fact]
        public async Task OnGetAsync_ReturnsNotFound_WhenIdIsNull()
        {
       
            var result = await _pageModel.OnGetAsync(null);

    
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task OnGetAsync_ReturnsNotFound_WhenProductIsNull()
        {
        
            int productId = 999; 

            
            var result = await _pageModel.OnGetAsync(productId);

           
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task OnGetAsync_ReturnsPage_WhenProductIsFound()
        {
           
            int productId = 1; 

          
            var result = await _pageModel.OnGetAsync(productId);

         
            Assert.IsType<PageResult>(result);
            Assert.Equal(productId, _pageModel.Product.Id);
        }

        [Fact]
        public async Task OnPostAsync_ReturnsPage_WhenModelStateIsInvalid()
        {
           
            _pageModel.ModelState.AddModelError("Quantity", "Required");

          
            
            _pageModel.Quantity = 1;
            var result = await _pageModel.OnPostAsync(1);

          
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task OnPostAsync_ReturnsNotFound_WhenUserIsNull()
        {
          
            int productId = 1;
            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                            .ReturnsAsync((IdentityUser)null);

            
            var result = await _pageModel.OnPostAsync(productId);

          
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task OnPostAsync_ReturnsNotFound_WhenProductIsNull()
        {
           
            int productId = 99000; 
            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                            .ReturnsAsync(new IdentityUser());

            
            var result = await _pageModel.OnPostAsync(productId);

          
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task OnPostAsync_ReturnsRedirectToPage_WhenSuccess()
        {
           
            int productId = 1;
            var user = new IdentityUser();
            var product = await _context.Products.FindAsync(productId);
            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                            .ReturnsAsync(user);
            _pageModel.Quantity = 1;

           
            var result = await _pageModel.OnPostAsync(productId);

         
            Assert.IsType<RedirectToPageResult>(result);
        }
    }
}