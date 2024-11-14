using System.ComponentModel.DataAnnotations;

namespace CS5019_A1_Codeworks.Models
{
    public class Checkout
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        [Phone(ErrorMessage = "Invalid Phone Number.")]
        public string Phone { get; set; }

        [Required]
        public DateTime OrderTime { get; set; }

        // Shipping Information
        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Postal Code is required.")]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        public string Country { get; set; }

        // Payment Information (No restrictions)
        [Required(ErrorMessage = "Card Number is required.")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Expiry Date is required.")]
        public string ExpiryDate { get; set; }

        [Required(ErrorMessage = "CVV is required.")]
        public string CVV { get; set; }
    }
}
