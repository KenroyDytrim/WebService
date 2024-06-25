using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Web2.Models;

namespace Web2.Pages.Database.Patients
{
    public class DetailsModel : PageModel
    {
        private readonly Web2.Data.AppDbContext _context;

        public DetailsModel(Web2.Data.AppDbContext context)
        {
            _context = context;
        }
		public patient_archive patient_archive { get; set; } = default!;
	    public Archive_Group archive_group { get; set; } = default!;
        public patient_group patient_group { get; set; } = default!;
        // получение данных пациента
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.patient_archive == null)
            {
                return NotFound();
            }

            var patient_archive = await _context.patient_archive.FirstOrDefaultAsync(m => m.id_patient == id);
			var archive_group = await _context.archive_group.FirstOrDefaultAsync(m => m.id_patient == id);
            var patient_group = await _context.patient_groups.FirstOrDefaultAsync(m => m.id_group == archive_group.id_group);
            if (patient_archive == null)
            {
                return NotFound();
            }
            else 
            {
                this.patient_archive = patient_archive;
				this.patient_group = patient_group;
			}
            return Page();
        }
    }
}