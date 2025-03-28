using System.ComponentModel.DataAnnotations;

namespace OnlineElectronicsStore.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } = "User";

        // Navigation Property
        public ICollection<Order> Orders { get; set; }
    }
}
