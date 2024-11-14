using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using CS5019_A1_Codeworks.Models;
using CS5019_A1_Codeworks.Services;
using System.IO;
using System.Threading.Tasks;

namespace CS5019_A1_Codeworks.Pages.Admin
{
    [Authorize(Roles = "admin")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product NewProduct { get; set; } = new Product();

        public IActionResult OnGet(int id)
        {
            NewProduct = _context.Products.Find(id);
            if (NewProduct == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile ImageFile)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // If an image file is provided, handle the file upload and update ImageUrl
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }

                var filePath = Path.Combine(uploads, ImageFile.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                // Update the ImageUrl only if a new image is provided
                NewProduct.ImageUrl = "/uploads/" + ImageFile.FileName;
            }

            // Update the product in the database (including any other fields)
            _context.Products.Update(NewProduct);
            await _context.SaveChangesAsync();

            return RedirectToPage("Admin");
        }
    }
}
