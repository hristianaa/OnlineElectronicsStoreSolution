using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Interfaces;

namespace OnlineElectronicsStore.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IProductService _products;
        private readonly ICategoryService _categories;
        private readonly IWebHostEnvironment _env;
        private readonly IProductPhotoService _photoService;

        public ProductsController(
            IProductService products,
            ICategoryService categories,
            IWebHostEnvironment env,
            IProductPhotoService photoService)
        {
            _products = products;
            _categories = categories;
            _env = env;
            _photoService = photoService;
        }

        // GET /Products
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var model = await _products.GetAllAsync();
            if (!model.Any())
                return View("Empty");
            return View(model);
        }

        // GET /Products/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _products.GetByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        // GET /Products/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(
                await _categories.GetAllAsync(), "Id", "Name");
            return View(new Product());
        }

        // POST /Products/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Product product,
            List<IFormFile>? Photos)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(
                    await _categories.GetAllAsync(), "Id", "Name", product.CategoryId);
                return View(product);
            }

            // 1) Save product to get ID
            var created = await _products.AddAsync(product);

            // 2) Save uploaded photos
            if (Photos?.Any() == true)
                await SavePhotosAsync(created.Id, Photos);

            return RedirectToAction(nameof(Index));
        }

        // GET /Products/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _products.GetByIdAsync(id);
            if (product == null) return NotFound();

            ViewBag.Categories = new SelectList(
                await _categories.GetAllAsync(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST /Products/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Product product,
            List<IFormFile>? Photos)
        {
            if (id != product.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(
                    await _categories.GetAllAsync(), "Id", "Name", product.CategoryId);
                return View(product);
            }

            var updated = await _products.UpdateAsync(product);
            if (!updated) return NotFound();

            if (Photos?.Any() == true)
                await SavePhotosAsync(product.Id, Photos);

            return RedirectToAction(nameof(Index));
        }

        // GET /Products/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _products.GetByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        // POST /Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _products.GetByIdAsync(id);
            if (product == null) return NotFound();

            // 1) Delete photo files & records
            foreach (var photo in product.Photos)
            {
                var physical = Path.Combine(_env.WebRootPath, photo.Url.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(physical))
                    System.IO.File.Delete(physical);

                await _photoService.DeletePhotoAsync(photo.Id);
            }

            // 2) Delete main image file if it points to wwwroot
            if (!string.IsNullOrEmpty(product.MainImageUrl) && product.MainImageUrl.StartsWith("/images/"))
            {
                var mainPhys = Path.Combine(_env.WebRootPath,
                    product.MainImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(mainPhys))
                    System.IO.File.Delete(mainPhys);
            }

            // 3) Delete the product entity
            await _products.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

      
        private async Task SavePhotosAsync(int productId, List<IFormFile> photos)
        {
            var uploadDir = Path.Combine(_env.WebRootPath, "images", "products");
            Directory.CreateDirectory(uploadDir);

            foreach (var file in photos)
            {
                if (file.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var filePath = Path.Combine(uploadDir, fileName);
                    using var stream = System.IO.File.Create(filePath);
                    await file.CopyToAsync(stream);

                    await _photoService.AddPhotoAsync(
                        productId,
                        $"/images/products/{fileName}");
                }
            }
        }
    }
}
