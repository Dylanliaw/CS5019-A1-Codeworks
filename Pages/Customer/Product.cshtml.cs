using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CS5019_A1_Codeworks.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using CS5019_A1_Codeworks.Services;

namespace CS5019_A1_Codeworks.Pages.Customer
{
    public class ProductModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ProductModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Product> Products { get; set; }
        public IList<string> Categories { get; set; }

        // The selected category filter (will be populated when a category is selected)
        public string SelectedCategory { get; set; }

        // Fetch products and categories on page load or search query
        public async Task OnGetAsync(string query = null, string sortBy = null)
        {
            // Fetch all unique categories from the database
            Categories = await _context.Products
                .Select(p => p.Category)
                .Distinct()
                .ToListAsync();

            // Set the selected category from the query string
            SelectedCategory = sortBy;

            // Filter products based on the search query or category filter
            var productsQuery = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                // Filter products based on search query (item name or description)
                productsQuery = productsQuery.Where(p => p.ItemName.Contains(query) || p.Description.Contains(query));
            }

            if (!string.IsNullOrEmpty(sortBy) && sortBy != "All Categories")
            {
                // Filter products by selected category if specified
                productsQuery = productsQuery.Where(p => p.Category == sortBy);
            }

            // Execute the query and assign the result to the Products property
            Products = await productsQuery.ToListAsync();
        }
    }
}
