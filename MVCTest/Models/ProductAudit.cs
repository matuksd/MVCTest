using MVCTest.Models.Enums;

namespace MVCTest.Models
{
    public class ProductAudit
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public string ChangedBy { get; set; } = string.Empty;
        public DateTime ChangedAt { get; set; }
        public ProductChangeType ChangeType { get; set; }
        public string Changes { get; set; } = string.Empty;
    }
}
