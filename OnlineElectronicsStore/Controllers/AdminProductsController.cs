using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Models.ViewModels;
using OnlineElectronicsStore.Services.Interfaces;

namespace OnlineElectronicsStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminProductsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IProductPhotoService _photoService;
        private readonly IWebHostEnvironment _env;

        public AdminProductsController(
            AppDbContext context,
            IProductPhotoService photoService,
            IWebHostEnvironment env)
        {
            _context = context;
            _photoService = photoService;
            _env = env;
        }

        // GET: /AdminProducts
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                                         .Include(p => p.Category)
                                         .Include(p => p.Photos)
                                         .ToListAsync();
            return View(products);
        }

        // GET: /AdminProducts/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products
                                        .Include(p => p.Category)
                                        .Include(p => p.Photos)
                                        .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            return View(product);
        }

        // GET: /AdminProducts/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products
                                        .Include(p => p.Photos)
                                        .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();

            await PopulateCategoriesDropDownList(product.CategoryId);

            var vm = new EditProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                ShortDescription = product.ShortDescription,
                LongDescription = product.LongDescription,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                MainImageUrl = product.MainImageUrl,
                Photos = product.Photos.ToList()
            };

            return View(vm);
        }

        // POST: /AdminProducts/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProductViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await PopulateCategoriesDropDownList(vm.CategoryId);
                return View(vm);
            }

            var product = await _context.Products
                                        .Include(p => p.Photos)
                                        .FirstOrDefaultAsync(p => p.Id == vm.Id);
            if (product == null) return NotFound();

            // Map scalar properties
            product.Name = vm.Name;
            product.ShortDescription = vm.ShortDescription;
            product.LongDescription = vm.LongDescription;
            product.Price = vm.Price;
            product.Stock = vm.Stock;
            product.CategoryId = vm.CategoryId;

            // Handle main image upload
            if (vm.MainImageFile?.Length > 0)
            {
                var fn = $"{Guid.NewGuid()}{Path.GetExtension(vm.MainImageFile.FileName)}";
                var dir = Path.Combine(_env.WebRootPath, "images", "products");
                Directory.CreateDirectory(dir);
                var path = Path.Combine(dir, fn);
                await using var fs = new FileStream(path, FileMode.Create);
                await vm.MainImageFile.CopyToAsync(fs);
                product.MainImageUrl = $"/images/products/{fn}";
            }

            // Handle extra photos
            if (vm.PhotoFiles?.Any() == true)
            {
                var dir = Path.Combine(_env.WebRootPath, "images", "products");
                Directory.CreateDirectory(dir);

                foreach (var file in vm.PhotoFiles.Where(f => f.Length > 0))
                {
                    var fn = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var path = Path.Combine(dir, fn);
                    await using var fs = new FileStream(path, FileMode.Create);
                    await file.CopyToAsync(fs);

                    await _photoService.AddPhotoAsync(product.Id, $"/images/products/{fn}");
                }
            }

            // Persist
            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: /AdminProducts/DeletePhoto
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePhoto(int photoId, int productId)
        {
            await _photoService.DeletePhotoAsync(photoId);
            return RedirectToAction(nameof(Edit), new { id = productId });
        }

        // Helper: category dropdown
        private async Task PopulateCategoriesDropDownList(object? selected = null)
        {
            var cats = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            ViewBag.CategoryId = new SelectList(cats, "Id", "Name", selected);
        }
    }
}
