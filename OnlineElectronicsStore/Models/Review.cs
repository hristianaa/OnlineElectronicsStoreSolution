using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineElectronicsStore.Models
{
   public class Review
{
    public int Id { get; set; }
    
    public Product? Product { get; set; }  // Make it nullable
    public User? User { get; set; }  // Make it nullable
    public string? Comment { get; set; }  // Make it nullable
    
    // Constructor
    public Review(Product? product, User? user, string? comment)
    {
        Product = product;
        User = user;
        Comment = comment;
    }
}

}
