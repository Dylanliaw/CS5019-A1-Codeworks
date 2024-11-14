using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CS5019_A1_Codeworks.Models;
using CS5019_A1_Codeworks.Services;

namespace CS5019_A1_Codeworks.Pages.Customer
{
    public class CheckoutModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CheckoutModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Checkout Checkout { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Checkout.OrderTime = DateTime.Now;

            // Save the checkout information to the database
            _context.Checkouts.Add(Checkout);
            await _context.SaveChangesAsync();

            // Redirect to a confirmation page or show success message
            return RedirectToPage("OrderConfirmation");
        }
    }
}
