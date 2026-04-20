using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Aulas.Data.Model;
using DwebAulas.Data;

namespace DwebAulas.Pages.Degrees
{
    public class DeleteModel : PageModel
    {
        private readonly DwebAulas.Data.ApplicationDbContext _context;

        public DeleteModel(DwebAulas.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Degree Degree { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var degree = await _context.Degrees.FirstOrDefaultAsync(m => m.Id == id);

            if (degree is not null)
            {
                Degree = degree;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var degree = await _context.Degrees.FindAsync(id);
            if (degree != null)
            {
                Degree = degree;
                _context.Degrees.Remove(Degree);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
