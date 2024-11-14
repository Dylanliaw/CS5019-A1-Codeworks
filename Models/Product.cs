using System.ComponentModel.DataAnnotations;

namespace CS5019_A1_Codeworks.Models
{
    public class Product
    {
        [Key]
        public int ProductId{ get; set; }
        public string? ItemName { get; set; }
        public string? Description { get; set; }
        public string? Brand { get; set; }
        public float CostPrice { get; set; }
        public float SellingPrice { get; set; }
        public float DiscountPrice { get; set; }
        public int Stock { get; set; } 
        public float Price { get; set; }
        public string? ImageUrl { get; set; }
        public string? Category { get; set; } // Add category property
    }
}
