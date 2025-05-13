// Controllers/AdminSupportController.cs
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineElectronicsStore.Data;
using Microsoft.EntityFrameworkCore;

namespace OnlineElectronicsStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminSupportController : Controller
    {
        private readonly AppDbContext _db;
        public AdminSupportController(AppDbContext db) => _db = db;

        // GET /AdminSupport
        public async Task<IActionResult> Index()
        {
            var messages = await _db.SupportMessages
                                    .AsNoTracking()
                                    .OrderByDescending(m => m.SubmittedOn)
                                    .ToListAsync();
            return View(messages);
        }

        // GET /AdminSupport/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var msg = await _db.SupportMessages
                               .AsNoTracking()
                               .FirstOrDefaultAsync(m => m.Id == id);
            if (msg == null) return NotFound();
            return View(msg);
        }
    }
}
