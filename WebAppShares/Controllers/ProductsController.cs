using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using WebAppShares.Data;
using WebAppShares.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using WebAppShares.Models;
using Microsoft.AspNetCore.Authorization;


using WebAppShares.Views.Shared.Component.SearchBar;
namespace WebAppShares.Controllers
{
    [Authorize(Policy = "AdminPolicy")]
    public class ProductsController : AProductsController
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _environment = webHostEnvironment;
        }

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; } = 10;

        public int TotalItems { get; set; }

        public int StartPage { get; set; }

        public int EndPage { get; set; }


        public override async Task<IActionResult> Index(string SearchText="",int page=1)
        {
            List<ProductsModel> prod = await _context.Products.Include(x => x.Image).OrderByDescending(x => x.Id).ToListAsync(); ;
            List<ProductsModel> product;
            if (SearchText != "" && SearchText != null)
            {
                prod = await _context.Products.Include(x => x.Image).OrderByDescending(x => x.Id).Where(p=>p.Name.Contains(SearchText) || p.Price.ToString().Contains(SearchText) || p.Description.Contains(SearchText)).ToListAsync();
            
            }
            int skip = (page - 1) * PageSize;
            product = prod;
            prod = prod.Skip(skip).Take(PageSize).ToList();

            int totalPages = (int)Math.Ceiling((decimal)product.Count/(decimal)10);
            int currentPage = page;

            int startPage = currentPage - 5;
            int endPage = currentPage + 4;

            if(startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }
            if(endPage > totalPages) {
            endPage = totalPages;
                if(endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }
            TotalPages = totalPages;
            CurrentPage = currentPage;
            TotalItems = product.Count;
            StartPage = startPage;
            EndPage = endPage;
            ViewData["TotalPages"]= TotalPages;
            ViewData["PageSize"] =10;
            ViewData["TotalItems"] = TotalItems;
            ViewData["StartPage"]= StartPage;
            ViewData["CurrentPage"] = CurrentPage;
            ViewData["EndPage"] = endPage;

            try
            {
                return View(prod);
            }
            catch (Exception ex)
            {
                return NotFound();
            }

        }

        // GET: Products/Details/5
        public override async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include(x => x.Image)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Quantity,Description,Price,Discount,Image")] ProductsModel productModel)
        {
            if (ModelState.IsValid)
            {
                var prod = _context.Add(productModel);
                await _context.SaveChangesAsync();


                int Id = prod.Entity.Id;

                if (Id > 0 && productModel.Image != null)
                {
                    string wwwrootpath = _environment.WebRootPath;

                    string subDirPath = $"Product{Id}";

                    DirectoryInfo dir = new DirectoryInfo(wwwrootpath + "/Products");
                    if (dir.Exists)
                    {
                        dir.Create();
                    }
                    dir.CreateSubdirectory(subDirPath);

                    string filename = Path.GetFileNameWithoutExtension(productModel.Image.ImageFile.FileName);
                    string extension = Path.GetExtension(productModel.Image.ImageFile.FileName);


                    productModel.Image.ImageTitle = filename + Id + extension;

                    string path = Path.Combine(wwwrootpath + "/Products/" + subDirPath + "/", productModel.Image.ImageTitle);

                    using (var FileStream = new FileStream(path, FileMode.Create))
                    {
                        await productModel.Image.ImageFile.CopyToAsync(FileStream);
                    }

                    _context.Products.Update(productModel);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(productModel);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Quantity,Description,Price,Discount,Image")] ProductsModel productModel)
        {
            if (id != productModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingProduct = await _context.Products.Include(o => o.Image).FirstOrDefaultAsync(o => o.Id == id);

                    if (existingProduct == null)
                    {
                        return NotFound();
                    }

                    existingProduct.Name = productModel.Name;
                    existingProduct.Quantity = productModel.Quantity;
                    existingProduct.Description = productModel.Description;
                    existingProduct.Price = productModel.Price;
                    existingProduct.Discount = productModel.Discount;

                    if (productModel.Image != null && productModel.Image.ImageFile != null)
                    {
                        string wwwrootPath = _environment.WebRootPath;
                        string productDirPath = Path.Combine(wwwrootPath, "Products", $"Product{id}");

                        if (!Directory.Exists(productDirPath))
                        {
                            Directory.CreateDirectory(productDirPath);
                        }

                        string oldImagePath = Path.Combine(productDirPath, wwwrootPath + "/Products/" + $"Product{id}");

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }

                        string filename = Path.GetFileNameWithoutExtension(productModel.Image.ImageFile.FileName);
                        string extension = Path.GetExtension(productModel.Image.ImageFile.FileName);
                        productModel.Image.ImageTitle = $"{filename}{id}{extension}";

                        string newImagePath = Path.Combine(productDirPath, productModel.Image.ImageTitle);

                        using (var fileStream = new FileStream(newImagePath, FileMode.Create))
                        {
                            await productModel.Image.ImageFile.CopyToAsync(fileStream);
                        }

                        existingProduct.Image.ImageTitle = productModel.Image.ImageTitle;
                    }

                    _context.Update(existingProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(productModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(productModel);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productModel = await _context.Products.Include(o => o.Image).FirstOrDefaultAsync(o => o.Id == id);
            var countf = await _context.BuyProductPurchasedGoods.FirstOrDefaultAsync(o => o.ProductsModelId == id);
            if (countf != null)
            {
                ModelState.AddModelError(string.Empty, "Помилка, даний продукт уже купили його видалення на даний момент не можливе");
                return View(productModel);
            }

      

            string rootPath = _environment.WebRootPath;

            DirectoryInfo df = new DirectoryInfo(rootPath + "/Products/" + $"Product{id}");
            df.Delete(true);


            if (productModel != null)
            {
                _context.Products.Remove(productModel);
                _context.Images.Remove(productModel.Image);
            }

            await _context.SaveChangesAsync();



            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
