using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web2.Models;

namespace Web2.Pages.Database.Patients
{
    public class EditModel : PageModel
    {
        private readonly Web2.Data.AppDbContext _context;
        // выбор группы
        public List<SelectListItem>? GetGroup()
        {
            List<SelectListItem> group = new List<SelectListItem>();
            group.Add(new SelectListItem() { Text = "Контрольная группа", Value = "1" });
            group.Add(new SelectListItem() { Text = "Основная группа", Value = "2" });
            group.Add(new SelectListItem() { Text = "Группа сравнения", Value = "3" });
            return group;
        }
        public EditModel(Web2.Data.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public patient_archive Patient { get; set; }
		[BindProperty]
		public Archive_Group Archive { get; set; }
        [BindProperty]
        public patient_group patient_groups { get; set; }
        // получение данных пациента
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Patient = await _context.patient_archive.FirstOrDefaultAsync(m => m.id_patient == id);
			Archive = await _context.archive_group.FirstOrDefaultAsync(m => m.id_patient == id);

            if (Patient == null)
            {
                return NotFound();
            }
            return Page();
        }
        // изменение данных пациента
        public async Task<IActionResult> OnPostEditAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var id_g = await _context.archive_group.FromSqlRaw($"SELECT * FROM archive_group WHERE id_patient={Patient.id_patient}").ToListAsync();
            var b = await _context.archive_group.FindAsync(Patient.id_patient, id_g[0].id_group);
            if (b != null)
            {
                _context.archive_group.Remove(b);
                await _context.SaveChangesAsync();
            }

            Archive.patient_archive = Patient;
            _context.Attach(Patient).State = EntityState.Modified;
            _context.archive_group.Add(Archive);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(Patient.id_patient))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            string? url = Url.Page("Patients", new { group = 0 });
            return Redirect(url);
        }

        private bool PatientExists(int id)
        {
            return _context.patient_archive.Any(e => e.id_patient == id);
        }
    }
}