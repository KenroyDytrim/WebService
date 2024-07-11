using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Web2.Models;

namespace Web2.Pages.Database
{
    public class Patient_ExaminationsModel : PageModel
    {
		private readonly Web2.Data.AppDbContext _context;

		public Patient_ExaminationsModel(Web2.Data.AppDbContext context)
		{
			_context = context;
		}
        public IList<Examination> examinations { get; set; } = default!;
        public int id_p;
        // получение всех результатов диспансеризации определённого пациента
        public async Task<IActionResult> OnGetAsync(int id)
        {
            id_p = id;
            examinations = await _context.examination.FromSqlRaw($"SELECT * FROM examination WHERE id_examination IN(SELECT id_examination FROM patient_examinations WHERE id_patient={id})").ToListAsync();
            return Page();
        }
        // удаление определённых результатов диспансеризации из БД
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var b = await _context.examination.FindAsync(id);
            if (b != null)
            {
                _context.examination.Remove(b);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("./Patient_Examinations");
        }
    }
}