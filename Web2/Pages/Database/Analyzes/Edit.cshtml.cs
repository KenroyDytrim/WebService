using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Web2.Models;

namespace Web2.Pages.Database.Analyzes
{
    public class EditModel : PageModel
    {
        private readonly Web2.Data.AppDbContext _context;

        public EditModel(Web2.Data.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Analyzes Analyzes { get; set; }
        [BindProperty]
        public patient_archive Patient { get; set; }
        [BindProperty]
        public Patient_Analyzes P_A { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Analyzes = await _context.analyzes.FirstOrDefaultAsync(m => m.id_analysis == id);
            P_A = await _context.patient_analyzes.FirstOrDefaultAsync(m => m.id_analysis == id);

            if (Analyzes == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var id_a = await _context.patient_analyzes.FromSqlRaw($"SELECT * FROM patient_analyzes WHERE id_analysis={Analyzes.id_analysis}").ToListAsync();
            var b = await _context.patient_analyzes.FindAsync(id_a[0].id_patient, Analyzes.id_analysis);
            if (b != null)
            {
                _context.patient_analyzes.Remove(b);
                await _context.SaveChangesAsync();
            }

            P_A.analyzes = Analyzes;
            _context.Attach(Analyzes).State = EntityState.Modified;
            _context.patient_analyzes.Add(P_A);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(Analyzes.id_analysis))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Analyzes");
        }

        private bool PatientExists(int id)
        {
            return _context.analyzes.Any(e => e.id_analysis == id);
        }
    }
}