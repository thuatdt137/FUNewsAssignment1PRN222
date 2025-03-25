using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Domain.Models;

namespace Assignment2.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly Domain.Models.FunewsManagementContext _context;

        public IndexModel(Domain.Models.FunewsManagementContext context)
        {
            _context = context;
        }

        public IList<Category> Category { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Category = await _context.Categories
                .Include(c => c.ParentCategory).ToListAsync();
        }
    }
}
