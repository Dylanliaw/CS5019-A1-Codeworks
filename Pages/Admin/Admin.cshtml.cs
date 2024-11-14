using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CS5019_A1_Codeworks.Models;
using CS5019_A1_Codeworks.Services;

namespace CS5019_A1_Codeworks.Pages.Admin
{
    [Authorize(Roles = "admin")]
    public class AdminModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        [BindProperty]
        public Product NewProduct { get; set; } = new Product();

        [BindProperty]
        public IFormFile Image { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();

        // Search Query to retain the search value in the input
        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; }

        public AdminModel(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public void OnGet()
        {
            // Retrieve all products
            var query = _context.Products.AsQueryable();

            // Filter products based on the search query
            if (!string.IsNullOrEmpty(SearchQuery))
            {
                var lowerSearchQuery = SearchQuery.ToLower();

                // Match any part of the product's details (name, description, brand, category)
                query = query.Where(p =>
                    p.ItemName.ToLower().Contains(lowerSearchQuery) ||
                    p.Description.ToLower().Contains(lowerSearchQuery) ||
                    p.Brand.ToLower().Contains(lowerSearchQuery) ||
                    p.Category.ToLower().Contains(lowerSearchQuery)
                );
            }

            // Assign the filtered list to the Products property
            Products = query.ToList();
        }

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

            NewProduct.ImageUrl = "/uploads/" + Image.FileName;

            if (string.IsNullOrWhiteSpace(NewProduct.Category))
            {
                ModelState.AddModelError("Category", "Please provide a category.");
                return Page();
            }

            _context.Products.Add(NewProduct);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
