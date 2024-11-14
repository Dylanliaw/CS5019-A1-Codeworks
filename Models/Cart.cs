namespace CS5019_A1_Codeworks.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public string? UserId { get; set; }
        public List<CartItem>? CartItems { get; set; }
    }
}
