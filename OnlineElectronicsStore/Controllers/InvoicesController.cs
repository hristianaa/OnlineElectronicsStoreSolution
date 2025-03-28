using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Interfaces;

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

        [HttpGet]
        public IActionResult GetInvoices()
        {
            var invoices = _invoiceService.GetAll();
            return Ok(invoices);
        }

        [HttpGet("{id}")]
        public IActionResult GetInvoice(int id)
        {
            var invoice = _invoiceService.GetById(id);
            if (invoice == null)
                return NotFound(new { Message = "Invoice not found." });

            return Ok(invoice);
        }

        [HttpPost]
        public IActionResult CreateInvoice([FromBody] Invoice invoice)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _invoiceService.Create(invoice);
            return Ok(new { Message = "Invoice created." });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteInvoice(int id)
        {
            _invoiceService.Delete(id);
            return Ok(new { Message = "Invoice deleted." });
        }
    }
}
