using System.ComponentModel.DataAnnotations;

namespace OnlineElectronicsStore.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required, StringLength(100)]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6), DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password"), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
