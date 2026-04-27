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
    public class IndexModel : PageModel
    {
        private readonly DwebAulas.Data.ApplicationDbContext _context;

        public IndexModel(DwebAulas.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Degree> Degree { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Degree = await _context.Degrees.ToListAsync();
        }
    }
}
