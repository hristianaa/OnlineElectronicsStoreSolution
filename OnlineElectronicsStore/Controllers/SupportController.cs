// Controllers/SupportController.cs
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Models.ViewModels;

namespace OnlineElectronicsStore.Controllers
{
    public class SupportController : Controller
    {
        private readonly AppDbContext _db;
        public SupportController(AppDbContext db) => _db = db;

        // GET: /Support/Contact
        [HttpGet]
        public IActionResult Contact()
        {
            ViewBag.Success = TempData["SupportSuccess"];
            return View(new ContactFormModel());
        }

        // POST: /Support/Contact
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactFormModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            // map & save
            var msg = new SupportMessage
            {
                Name = vm.Name,
                Email = vm.Email,
                Message = vm.Message
            };
            _db.SupportMessages.Add(msg);
            await _db.SaveChangesAsync();

            TempData["SupportSuccess"] = "Your message has been sent, thank you!";
            return RedirectToAction(nameof(Contact));
        }
    }
}
