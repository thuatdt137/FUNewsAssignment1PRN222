using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Domain.Models;

namespace Assignment2.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly Domain.Models.FunewsManagementContext _context;

        public CreateModel(Domain.Models.FunewsManagementContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryDesciption");
            return Page();
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Categories.Add(Category);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
