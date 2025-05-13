// Models/ViewModels/ContactFormModel.cs
using System.ComponentModel.DataAnnotations;

namespace OnlineElectronicsStore.Models.ViewModels
{
    public class ContactFormModel
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(2000)]
        public string Message { get; set; }
    }
}
