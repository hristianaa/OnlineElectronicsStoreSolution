using OnlineElectronicsStore.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineElectronicsStore.Models.ViewModels
{
    public class CheckoutViewModel
    {
        // your existing cart lines and totals
        public List<CartLineDto> Items { get; set; } = new();
        [Display(Name = "Subtotal")]
        public decimal Subtotal { get; set; }
        [Display(Name = "Shipping Fee")]
        public decimal ShippingFee { get; set; }
        [Display(Name = "Total")]
        public decimal Total { get; set; }

        // new: discount code entry
        [Display(Name = "Discount Code")]
        public string? DiscountCode { get; set; }

        // new: how much was actually subtracted
        public decimal DiscountApplied { get; set; }

        // new: user-facing status (invalid, expired, not applicable)
        public string? DiscountMessage { get; set; }

        // new: computed total after discount
        public decimal TotalAfterDiscount
            => Math.Max(0, Total - DiscountApplied);

        // your existing checkout fields
        [Required, Display(Name = "Shipping Address")]
        public string ShippingAddress { get; set; } = "";
        [Required, Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; } = "";
    }
}
