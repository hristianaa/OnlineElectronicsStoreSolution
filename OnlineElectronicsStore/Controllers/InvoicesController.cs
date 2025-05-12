using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Interfaces;
using OnlineElectronicsStore.Services.Helpers;

namespace OnlineElectronicsStore.Controllers
{
    [Authorize]
    public class InvoicesController : Controller
    {
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        // GET: /Invoices
        public async Task<IActionResult> Index()
        {
            var invoices = await _invoiceService.GetAllAsync();
            return View(invoices);
        }

        // GET: /Invoices/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var invoice = await _invoiceService.GetByIdAsync(id);
            if (invoice == null)
                return NotFound();

            return View(invoice);
        }

        // GET: /Invoices/Create
        // (if you want admins to be able to manually create invoices)
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View(new Invoice());
        }

        // POST: /Invoices/Create
        [HttpPost, Authorize(Roles = "Admin"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Invoice model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var created = await _invoiceService.CreateAsync(model);
            return RedirectToAction(nameof(Details), new { id = created.Id });
        }

        // GET: /Invoices/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var invoice = await _invoiceService.GetByIdAsync(id);
            if (invoice == null)
                return NotFound();

            return View(invoice);
        }

        // POST: /Invoices/Delete/5
        [HttpPost, ActionName("Delete"), Authorize(Roles = "Admin"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _invoiceService.DeleteAsync(id);
            if (!success)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Invoices/DownloadPdf/5
        public async Task<IActionResult> DownloadPdf(int id)
        {
            var invoice = await _invoiceService.GetByIdAsync(id);
            if (invoice == null)
                return NotFound();

            var pdfBytes = InvoicePdfGenerator.GeneratePdf(invoice);
            return File(pdfBytes, "application/pdf", $"Invoice_{invoice.Id}.pdf");
        }
    }
}
