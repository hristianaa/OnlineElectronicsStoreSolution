using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Helpers;
using OnlineElectronicsStore.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace OnlineElectronicsStore.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        /// <summary>
        /// Get all invoices.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetInvoices()
        {
            var invoices = await _invoiceService.GetAllAsync();
            return Ok(invoices);
        }

        /// <summary>
        /// Get a specific invoice by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoice(int id)
        {
            var invoice = await _invoiceService.GetByIdAsync(id);
            if (invoice == null)
                return NotFound(new { Message = $"Invoice with ID {id} not found." });

            return Ok(invoice);
        }

        /// <summary>
        /// Create a new invoice.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] Invoice invoice)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            invoice.InvoiceDate = DateTime.UtcNow;

            var created = await _invoiceService.CreateAsync(invoice);
            return CreatedAtAction(nameof(GetInvoice), new { id = created.Id }, created);
        }

        /// <summary>
        /// Delete an invoice by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            var deleted = await _invoiceService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Message = $"Invoice with ID {id} not found or already deleted." });

            return Ok(new { Message = $"Invoice {id} deleted successfully." });
        }

        /// <summary>
        /// Download invoice as PDF.
        /// </summary>
        [HttpGet("{id}/pdf")]
        public async Task<IActionResult> DownloadPdf(int id)
        {
            var invoice = await _invoiceService.GetByIdAsync(id);
            if (invoice == null)
                return NotFound("Invoice not found");

            var pdfBytes = InvoicePdfGenerator.GeneratePdf(invoice);
            return File(pdfBytes, "application/pdf", $"Invoice_{invoice.Id}.pdf");
        }
    }
}
