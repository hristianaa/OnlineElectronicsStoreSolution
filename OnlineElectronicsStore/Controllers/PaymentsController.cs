using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Interfaces;
using System;

namespace OnlineElectronicsStore.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        /// <summary>
        /// Get all payments.
        /// </summary>
        [HttpGet]
        public IActionResult GetPayments()
        {
            var payments = _paymentService.GetAll();
            return Ok(payments);
        }

        /// <summary>
        /// Get payment by ID.
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult GetPayment(int id)
        {
            var payment = _paymentService.GetById(id);
            return payment != null ? Ok(payment) : NotFound(new { Message = "Payment not found." });
        }

        /// <summary>
        /// Create a new payment.
        /// </summary>
        [HttpPost]
        public IActionResult CreatePayment([FromBody] Payment payment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            payment.PaymentDate = DateTime.UtcNow;
            _paymentService.Create(payment);

            return Ok(new { Message = "Payment recorded." });
        }

        /// <summary>
        /// Mark payment as paid.
        /// </summary>
        [HttpPut("mark-paid/{id}")]
        public IActionResult MarkAsPaid(int id)
        {
            _paymentService.MarkAsPaid(id);
            return Ok(new { Message = "Payment marked as paid." });
        }

        /// <summary>
        /// Delete a payment by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult DeletePayment(int id)
        {
            _paymentService.Delete(id);
            return Ok(new { Message = "Payment deleted." });
        }
    }
}
