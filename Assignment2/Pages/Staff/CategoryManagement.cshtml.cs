using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Assignment2.Pages.Staff
{
    [Authorize(Policy = "StaffOnly")]
    public class CategoryManagementModel : PageModel
    {
        private readonly FunewsManagementContext _context;

        public CategoryManagementModel(FunewsManagementContext context)
        {
            _context = context;
        }

        public List<Category> Categories { get; set; }

        [BindProperty]
        public Category Category { get; set; }

        public void OnGet()
        {
            Categories = _context.Categories
                .Include(c => c.ParentCategory)
                .ToList();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            Category.IsActive = true;
            _context.Categories.Add(Category);
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            if (!ModelState.IsValid)
            {
                Categories = _context.Categories.ToList();
                return Page();
            }

            var existingCategory = await _context.Categories.FindAsync(Category.CategoryId);
            if (existingCategory == null) return NotFound();

            existingCategory.CategoryName = Category.CategoryName;
            existingCategory.CategoryDesciption = Category.CategoryDesciption;
            existingCategory.ParentCategoryId = Category.ParentCategoryId;
            existingCategory.IsActive = Category.IsActive;

            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}
