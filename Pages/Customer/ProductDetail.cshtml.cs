using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using CS5019_A1_Codeworks.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using CS5019_A1_Codeworks.Services;

namespace CS5019_A1_Codeworks.Pages.Customer
{
    public class ProductDetailModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public Product Product { get; set; }

        public ProductDetailModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // OnGetAsync: Retrieves product details based on the product ID
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve product details from the database
            Product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);

            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }

        // OnPostAsync: Adds product to the cart or updates the quantity if the product is already in the cart
        public async Task<IActionResult> OnPostAsync(int productId, int quantity)
        {
            // Ensure the user is authenticated
            var userId = _userManager.GetUserId(User);

            // Ensure the cart exists for the user, or create a new one if it doesn't exist
            var cart = await _context.Carts
                .Include(c => c.CartItems) // Ensure CartItems are loaded
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                // If no cart exists, create one with an empty CartItems collection
                cart = new Cart { UserId = userId, CartItems = new List<CartItem>() };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            // Retrieve the product details to ensure it exists
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            // Check if the product is already in the cart
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (cartItem == null)
            {
                // Add the product to the cart if it's not already there
                cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = productId,
                    Name = product.ItemName,
                    Price = product.SellingPrice,
                    Quantity = quantity,
                    ImageUrl = product.ImageUrl
                };
                cart.CartItems.Add(cartItem);
            }
            else
            {
                // If the product is already in the cart, update the quantity
                cartItem.Quantity += quantity;
            }

            await _context.SaveChangesAsync();

            // Redirect the user to the Cart page
            return RedirectToPage("/Customer/Cart");
        }

        // OnPostRemoveAsync: Removes product from the cart
        public async Task<IActionResult> OnPostRemoveAsync(int productId)
        {
            // Get the current user's ID
            var userId = _userManager.GetUserId(User);

            // Retrieve the cart for the user
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return NotFound();
            }

            // Find the cart item to remove
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem != null)
            {
                cart.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Customer/Cart");
        }

        // This method is for handling AJAX requests related to product search (if needed)
        public async Task<IActionResult> OnGetSearchAsync(string searchQuery)
        {
            var products = await _context.Products
                .Where(p => p.ItemName.Contains(searchQuery) || p.Description.Contains(searchQuery))
                .ToListAsync();

            return new JsonResult(products);
        }
    }
}
