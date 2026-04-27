using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Aulas.Data.Model;
using DwebAulas.Data;

namespace DwebAulas.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly DwebAulas.Data.ApplicationDbContext _context;

        public IndexModel(DwebAulas.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Student> Student { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Student = await _context.Students
                .Include(s => s.Degree).ToListAsync();
        }
    }
}
