using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.Models;
using Microsoft.AspNetCore.Authorization;

namespace OnlineElectronicsStore.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WishlistsController(AppDbContext context)
        {
            _context = context;
        }

        // 📦 GET: api/wishlists
        [HttpGet]
        public async Task<IActionResult> GetWishlists()
        {
            var wishlists = await _context.Wishlists.ToListAsync();
            return Ok(wishlists);
        }

        // 🔍 GET: api/wishlists/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWishlist(int id)
        {
            var wishlist = await _context.Wishlists.FindAsync(id);
            return wishlist != null
                ? Ok(wishlist)
                : NotFound(new { Message = "Wishlist not found." });
        }

        // ➕ POST: api/wishlists
        [HttpPost]
        public async Task<IActionResult> PostWishlist([FromBody] Wishlist wishlist)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Wishlists.Add(wishlist);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetWishlist), new { id = wishlist.Id }, wishlist);
        }

        // ✏️ PUT: api/wishlists/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWishlist(int id, [FromBody] Wishlist wishlist)
        {
            if (id != wishlist.Id)
                return BadRequest(new { Message = "Wishlist ID mismatch." });

            _context.Entry(wishlist).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Wishlist updated successfully." });
        }

        // ❌ DELETE: api/wishlists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWishlist(int id)
        {
            var wishlist = await _context.Wishlists.FindAsync(id);
            if (wishlist == null)
                return NotFound(new { Message = "Wishlist not found." });

            _context.Wishlists.Remove(wishlist);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Wishlist deleted successfully." });
        }
    }
}
