using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Web2.Models;

namespace Web2.Pages.Database.Examinations
{
    public class DetailsModel : PageModel
    {
        private readonly Web2.Data.AppDbContext _context;

        public DetailsModel(Web2.Data.AppDbContext context)
        {
            _context = context;
        }

      public Examination Examination { get; set; } = default!;
      public Patient_Examination P_E { get; set; } = default!;
        // получение определённых анализов
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.examination == null)
            {
                return NotFound();
            }

            var Examination = await _context.examination.FirstOrDefaultAsync(m => m.id_examination == id);
            var P_E = await _context.patient_examinations.FirstOrDefaultAsync(m => m.id_examination == id);
            if (Examination == null)
            {
                return NotFound();
            }
            else 
            {
                this.Examination = Examination;
                this.P_E = P_E;
            }
            return Page();
        }
    }
}