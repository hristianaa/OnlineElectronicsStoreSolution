using Microsoft.AspNetCore.Mvc;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Interfaces;

namespace OnlineElectronicsStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public IActionResult GetPayments()
        {
            return Ok(_paymentService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetPayment(int id)
        {
            var payment = _paymentService.GetById(id);
            if (payment == null)
                return NotFound(new { Message = "Payment not found." });

            return Ok(payment);
        }

        [HttpPost]
        public IActionResult CreatePayment([FromBody] Payment payment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _paymentService.Create(payment);
            return Ok(new { Message = "Payment created." });
        }

        [HttpPut("mark-paid/{id}")]
        public IActionResult MarkAsPaid(int id)
        {
            _paymentService.MarkAsPaid(id);
            return Ok(new { Message = "Payment marked as paid." });
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePayment(int id)
        {
            _paymentService.Delete(id);
            return Ok(new { Message = "Payment deleted." });
        }
    }
}
