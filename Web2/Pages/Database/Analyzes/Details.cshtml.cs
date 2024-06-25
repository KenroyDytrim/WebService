using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Web2.Models;

namespace Web2.Pages.Database.Analyzes
{
    public class DetailsModel : PageModel
    {
        private readonly Web2.Data.AppDbContext _context;

        public DetailsModel(Web2.Data.AppDbContext context)
        {
            _context = context;
        }

      public Models.Analyzes Analyzes { get; set; } = default!;
      public Patient_Analyzes P_A { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.analyzes == null)
            {
                return NotFound();
            }

            var Analyzes = await _context.analyzes.FirstOrDefaultAsync(m => m.id_analysis == id);
            var P_A = await _context.patient_analyzes.FirstOrDefaultAsync(m => m.id_analysis == id);
            if (Analyzes == null)
            {
                return NotFound();
            }
            else 
            {
                this.Analyzes = Analyzes;
                this.P_A = P_A;
            }
            return Page();
        }
    }
}