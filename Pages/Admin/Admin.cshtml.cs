using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CS5019_A1_Codeworks.Models;
using CS5019_A1_Codeworks.Services;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace CS5019_A1_Codeworks.Pages.Admin
{
    [Authorize(Roles = "admin")]
    public class AdminModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        [BindProperty]
        public Product Product { get; set; }

        [BindProperty]
        public IFormFile Image { get; set; }

        public List<Product> Products { get; set; }

        public AdminModel(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // Method to handle displaying the products with search functionality
        public void OnGet()
        {
            // Get the search term from the query string
            var searchTerm = Request.Query["searchTerm"].ToString();

            // If there's a search term, filter the products
            if (!string.IsNullOrEmpty(searchTerm))
            {
                Products = _context.Products
                    .Where(p => p.ItemName.Contains(searchTerm) ||
                                p.Description.Contains(searchTerm) ||
                                p.Brand.Contains(searchTerm) ||
                                p.Category.Contains(searchTerm))
                    .ToList();
            }
            else
            {
                // If no search term, display all products
                Products = _context.Products.ToList();
            }
        }

        // Method to handle adding a new product
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Image == null || Image.Length == 0)
            {
                ModelState.AddModelError("Image", "Please upload an image.");
                return Page();
            }

            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }

            var filePath = Path.Combine(uploads, Image.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await Image.CopyToAsync(stream);
            }

            // Save the product data including the image URL
            Product.ImageUrl = "/uploads/" + Image.FileName;
            _context.Products.Add(Product);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Admin/Admin");
        }

        // Method to handle deleting a product
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("/Admin/Admin");
        }
    }
}
