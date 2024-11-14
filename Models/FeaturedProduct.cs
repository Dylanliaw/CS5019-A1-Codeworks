using System.ComponentModel.DataAnnotations;

namespace CS5019_A1_Codeworks.Models
{
    public class FeaturedProduct
    {
        [Key]
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public float Price { get; set; }
        public DateTime EndDate { get; set; }
        public string? ImageUrl { get; set; }
    }
}
