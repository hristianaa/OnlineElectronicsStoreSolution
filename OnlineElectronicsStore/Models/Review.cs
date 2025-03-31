using System.ComponentModel.DataAnnotations;

namespace OnlineElectronicsStore.Models
{
    public class Review
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public string? Comment { get; set; }

        // ✅ Required for EF Core
        public Review() { }

        // ✅ Optional helper constructor for clean creation
        public Review(int productId, int userId, string? comment)
        {
            ProductId = productId;
            UserId = userId;
            Comment = comment;
        }
    }
}
