using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebAppShares.Controllers;
using WebAppShares.Data;
using WebAppShares.Data.Identity;
using WebAppShares.Models;
using Xunit;

namespace WebAppShares.Tests.Controllers
{
    public class ProductsControllerTests
    {
        private ProductsController _controller;
        private ApplicationDbContext _context;
        private Mock<IWebHostEnvironment> _webHostEnvironmentMock;

        public ProductsControllerTests()
        {
          
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();

            _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();

            _controller = new ProductsController(_context, _webHostEnvironmentMock.Object);
        }

        public void Dispose()
        {
         
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task Index_ReturnsViewWithProducts()
        {
            
            var result = await _controller.Index();

            Assert.IsType<ViewResult>(result);

            var viewResult = (ViewResult)result;
            var model = (List<ProductsModel>)viewResult.Model;
            Assert.Equal(0, model.Count);
        }

        [Fact]
        public async Task Details_WithValidId_ReturnsViewWithProduct()
        {
            
            var product = new ProductsModel { Name = "Test Product", Quantity = 10, Description = "Test Description", Price = 100, Discount = 10,Image=new ImageModel() };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

           
            var result = await _controller.Details(1);

       
            Assert.IsType<ViewResult>(result);

            var viewResult = (ViewResult)result;
            var model = (ProductsModel)viewResult.Model;
            Assert.Equal(product.Id, 3);
        }

        [Fact]
        public async Task Create_WithValidModel_ReturnsRedirectToActionResult()
        {
           
            var productModel = new ProductsModel { Name = "Test Product", Quantity = 10, Description = "Test Description", Price = 100, Discount = 10 };
            var imageFileMock = new Mock<IFormFile>();
            imageFileMock.SetupGet(x => x.FileName).Returns("test_image.png");
            productModel.Image = new ImageModel { ImageFile = imageFileMock.Object };

           
            var result = await _controller.Create(productModel);

         
            Assert.IsType<RedirectToActionResult>(result);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task Edit_WithValidModel_ReturnsRedirectToActionResult()
        {

            var product = new ProductsModel { Name = "Test Product", Quantity = 10, Description = "Test Description", Price = 100, Discount = 10, Image = new ImageModel() };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

         
            var result = await _controller.Edit(1, product);

          
            Assert.IsType<RedirectToActionResult>(result);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task DeleteConfirmed_WithValidId_ReturnsRedirectToActionResult()
        {
       
            var productModel = new ProductsModel { Name = "Test Product", Quantity = 10, Description = "Test Description", Price = 100, Discount = 10 };
            var imageModel = new ImageModel { ImageTitle = "test_image.png" };
            productModel.Image = imageModel;
            _context.Products.Add(productModel);
            await _context.SaveChangesAsync();

        
            var result = await _controller.DeleteConfirmed(2);

       
            Assert.IsType<RedirectToActionResult>(result);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
