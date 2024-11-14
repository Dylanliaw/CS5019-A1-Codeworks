using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CS5019_A1_Codeworks.Pages.Customer
{
    public class OrderConfirmationModel : PageModel
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public void OnGet(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}
