using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineElectronicsStore.Models
{
    public class ProductPhoto
    {
        public int Id { get; set; }

        [Required, Url]
        public string Url { get; set; } = string.Empty;

        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;
    }
}
