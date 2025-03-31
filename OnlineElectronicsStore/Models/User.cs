using System;
using System.ComponentModel.DataAnnotations; // Add this line for Required attribute

namespace OnlineElectronicsStore.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]  // Use RequiredAttribute to ensure that email cannot be null
        [EmailAddress] // Optionally, validate the email format
        public string Email { get; set; }

        [Required]  // Use RequiredAttribute to ensure that the password cannot be null
        public string Password { get; set; }

        [Required]  // Use RequiredAttribute for full name
        public string FullName { get; set; }

        public string Role { get; set; }
    



// Constructor for User class
public User(string email, string password, string fullName, string role)
        {
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
            Role = role ?? throw new ArgumentNullException(nameof(role)); // Make sure Role is provided
        }
    }
}
