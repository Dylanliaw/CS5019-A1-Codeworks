using System.Collections.Generic;
using System.Linq;
using CS5019_A1_Codeworks.Models;  // Correct namespace for Product
using CS5019_A1_Codeworks.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CS5019_A1_Codeworks.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _context;

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public List<Product> Products { get; set; }

        public void OnGet()
        {
            // Fetch all products from the database
            Products = _context.Products
                .Where(p => p.Stock > 0)  // Optional: filter for products that are in stock
                .Take(6) // Limit to 6 products (or adjust as needed)
                .ToList();
        }
    }
}
