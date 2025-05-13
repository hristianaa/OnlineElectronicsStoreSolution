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

        // GET: /AdminProducts/Create
        public async Task<IActionResult> Create()
        {
            await PopulateCategoriesDropDownList();
            return View(new Product());
        }

        // POST: /AdminProducts/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Product product,
            IFormFile? mainImageFile,
            IFormFileCollection? photoFiles)
        {
            if (!ModelState.IsValid)
            {
                await PopulateCategoriesDropDownList(product.CategoryId);
                return View(product);
            }

            if (mainImageFile?.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(mainImageFile.FileName)}";
                var dir = Path.Combine(_env.WebRootPath, "images", "products");
                Directory.CreateDirectory(dir);
                var path = Path.Combine(dir, fileName);
                await using var stream = new FileStream(path, FileMode.Create);
                await mainImageFile.CopyToAsync(stream);
                product.MainImageUrl = $"/images/products/{fileName}";
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            if (photoFiles?.Any() == true)
            {
                foreach (var file in photoFiles.Where(f => f.Length > 0))
                {
                    var extraFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var extraDir = Path.Combine(_env.WebRootPath, "images", "products");
                    Directory.CreateDirectory(extraDir);
                    var extraPath = Path.Combine(extraDir, extraFileName);
                    await using var fs = new FileStream(extraPath, FileMode.Create);
                    await file.CopyToAsync(fs);
                    await _photoService.AddPhotoAsync(product.Id, $"/images/products/{extraFileName}");
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /AdminProducts/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products
                                        .Include(p => p.Photos)
                                        .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            await PopulateCategoriesDropDownList(product.CategoryId);
            return View(product);
        }

        // POST: /AdminProducts/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Product product,
            IFormFile? mainImageFile,
            IFormFileCollection? photoFiles)
        {
            if (id != product.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                await PopulateCategoriesDropDownList(product.CategoryId);
                return View(product);
            }

            if (mainImageFile?.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(mainImageFile.FileName)}";
                var dir = Path.Combine(_env.WebRootPath, "images", "products");
                Directory.CreateDirectory(dir);
                var path = Path.Combine(dir, fileName);
                await using var stream = new FileStream(path, FileMode.Create);
                await mainImageFile.CopyToAsync(stream);
                product.MainImageUrl = $"/images/products/{fileName}";
            }

            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            if (photoFiles?.Any() == true)
            {
                foreach (var file in photoFiles.Where(f => f.Length > 0))
                {
                    var extraFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var extraDir = Path.Combine(_env.WebRootPath, "images", "products");
                    Directory.CreateDirectory(extraDir);
                    var extraPath = Path.Combine(extraDir, extraFileName);
                    await using var fs = new FileStream(extraPath, FileMode.Create);
                    await file.CopyToAsync(fs);
                    await _photoService.AddPhotoAsync(product.Id, $"/images/products/{extraFileName}");
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: /AdminProducts/DeletePhoto
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePhoto(int photoId, int productId)
        {
            await _photoService.DeletePhotoAsync(photoId);
            return RedirectToAction(nameof(Edit), new { id = productId });
        }

        // POST: /AdminProducts/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateCategoriesDropDownList(object? selected = null)
        {
            var cats = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            ViewBag.CategoryId = new SelectList(cats, "Id", "Name", selected);
        }
    }
}
