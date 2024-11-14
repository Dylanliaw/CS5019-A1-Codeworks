using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using CS5019_A1_Codeworks.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CS5019_A1_Codeworks.Services;

namespace CS5019_A1_Codeworks.Pages.Customer
{
    public class CartModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public Cart Cart { get; set; }
        public List<CartItem> CartItemsWithProducts { get; set; }

        public CartModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);

            // Find the user's cart, including cart items
            Cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (Cart == null)
            {
                Cart = new Cart { CartItems = new List<CartItem>() };
            }

            // Get the products for each cart item
            CartItemsWithProducts = new List<CartItem>();
            foreach (var cartItem in Cart.CartItems)
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(p => p.ProductId == cartItem.ProductId);

                if (product != null)
                {
                    cartItem.Name = product.ItemName;
                    cartItem.Price = product.SellingPrice;
                    cartItem.ImageUrl = product.ImageUrl;
                }

                CartItemsWithProducts.Add(cartItem);
            }

            return Page();
        }

        // POST method to remove an item from the cart
        [HttpPost]
        public async Task<IActionResult> OnPostRemoveItemAsync(int productId)
        {
            var userId = _userManager.GetUserId(User);
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart != null)
            {
                var cartItem = cart.CartItems.FirstOrDefault(c => c.ProductId == productId);
                if (cartItem != null)
                {
                    cart.CartItems.Remove(cartItem);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage();
        }

        // POST method to change the quantity of an item in the cart
        [HttpPost]
        public async Task<IActionResult> OnPostChangeQuantityAsync(int productId, string action, int quantity)
        {
            var userId = _userManager.GetUserId(User);
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart != null)
            {
                var cartItem = cart.CartItems.FirstOrDefault(c => c.ProductId == productId);
                if (cartItem != null)
                {
                    if (action == "increase")
                    {
                        cartItem.Quantity++;
                    }
                    else if (action == "decrease" && cartItem.Quantity > 1)
                    {
                        cartItem.Quantity--;
                    }
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage();
        }

        // POST method to clear the cart
        [HttpPost]
        public async Task<IActionResult> OnPostClearCartAsync()
        {
            var userId = _userManager.GetUserId(User);
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart != null)
            {
                cart.CartItems.Clear();
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
