using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineElectronicsStore.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;

        public string Role { get; set; } = "User";

        // ✅ Parameterless constructor for EF Core (required for seeding)
        public User() { }

        // ✅ Custom constructor
        public User(string email, string password, string fullName, string role)
        {
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
            Role = role ?? throw new ArgumentNullException(nameof(role));
        }
    }
}
