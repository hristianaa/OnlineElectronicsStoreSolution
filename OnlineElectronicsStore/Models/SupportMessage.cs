// Models/SupportMessage.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineElectronicsStore.Models
{
    public class SupportMessage
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required, StringLength(2000)]
        public string Message { get; set; } = "";

        [Display(Name = "Submitted On")]
        public DateTime SubmittedOn { get; set; } = DateTime.UtcNow;
    }
}

