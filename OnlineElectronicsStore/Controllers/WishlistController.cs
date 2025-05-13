using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineElectronicsStore.Services.Interfaces;

namespace OnlineElectronicsStore.Controllers
{
    [Authorize]
    public class WishlistController : Controller   // singular
    {
        private readonly IWishlistService _wish;
        public WishlistController(IWishlistService wishService)
            => _wish = wishService;

        // GET: /Wishlist
        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var items = await _wish.GetByUserIdAsync(userId);
            return View(items);
        }

        // POST: /Wishlist/Add
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int productId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _wish.AddAsync(userId, productId);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Wishlist/Remove
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int productId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _wish.RemoveAsync(userId, productId);
            return RedirectToAction(nameof(Index));
        }
    }
}
