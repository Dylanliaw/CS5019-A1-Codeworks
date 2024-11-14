using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CS5019_A1_Codeworks.Models;
using CS5019_A1_Codeworks.Services; // Replace with your actual namespace

namespace CS5019_A1_Codeworks.Pages.Search
{
    public class SearchModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public SearchModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Product> Products { get; set; }

        public void OnGet(string query)
        {
            if (!string.IsNullOrEmpty(query))
            {
                // Search for products in the database
                Products = _context.Products
                    .Where(p => p.ItemName.Contains(query) || p.Description.Contains(query))
                    .ToList();
            }
            else
            {
                // If no query, show all products
                Products = _context.Products.ToList();
            }
        }
    }
}
