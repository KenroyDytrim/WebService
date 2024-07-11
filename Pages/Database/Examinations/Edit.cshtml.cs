using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Web2.Models;

namespace Web2.Pages.Database.Examinations
{
    public class EditModel : PageModel
    {
        private readonly Web2.Data.AppDbContext _context;

        public EditModel(Web2.Data.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Examination Examination { get; set; }
        [BindProperty]
        public patient_archive Patient { get; set; }
        [BindProperty]
        public Patient_Examination P_E { get; set; }
        // получение результатов диспансеризации
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Examination = await _context.examination.FirstOrDefaultAsync(m => m.id_examination == id);
            P_E = await _context.patient_examinations.FirstOrDefaultAsync(m => m.id_examination == id);

            if (Examination == null)
            {
                return NotFound();
            }
            return Page();
        }
        // изменение результатов диспансеризации
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var id_e = await _context.patient_examinations.FromSqlRaw($"SELECT * FROM patient_examinations WHERE id_examination={Examination.id_examination}").ToListAsync();
            var b = await _context.patient_examinations.FindAsync(id_e[0].id_patient, Examination.id_examination);
            if (b != null)
            {
                _context.patient_examinations.Remove(b);
                await _context.SaveChangesAsync();
            }

            P_E.examination = Examination;
            _context.Attach(Examination).State = EntityState.Modified;
            _context.patient_examinations.Add(P_E);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(Examination.id_examination))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Examinations");
        }

        private bool PatientExists(int id)
        {
            return _context.examination.Any(e => e.id_examination == id);
        }
    }
}