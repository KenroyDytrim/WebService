using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Web2.Pages.Database
{
    public class Patient_AnalyzesModel : PageModel
    {
		private readonly Web2.Data.AppDbContext _context;

		public Patient_AnalyzesModel(Web2.Data.AppDbContext context)
		{
			_context = context;
		}
		public IList<Models.Analyzes> analyzes { get; set; } = default!;
        public int id_p;
        // получение всех анализов определенного пациента
		public async Task<IActionResult> OnGetAsync(int id)
        {
            id_p = id;
			analyzes = await _context.analyzes.FromSqlRaw($"SELECT * FROM analyzes WHERE id_analysis IN(SELECT id_analysis FROM patient_analyzes WHERE id_patient={id})").ToListAsync();
			return Page();
        }
        // удаление определённых анализов из БД
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var b = await _context.analyzes.FindAsync(id);
            if (b != null)
            {
                _context.analyzes.Remove(b);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("./Patient_Analyzes");
        }
    }
}