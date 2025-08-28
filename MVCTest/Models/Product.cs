using System.ComponentModel.DataAnnotations;

namespace MVCTest.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be zero or greater.")]
        public int Quantity { get; set; }
        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "Price must be zero or greater.")]
        public decimal Price { get; set; }
    }
}
