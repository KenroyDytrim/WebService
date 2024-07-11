using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Web2.Pages.Database.Analyzes
{
    public class AnalyzesModel : PageModel
    {
        [BindProperty]
        public Models.Analyzes Analyzes { get; set; }
        // выбор группы
        public List<SelectListItem>? GetGroup()
        {
            List<SelectListItem> group = new List<SelectListItem>();
            group.Add(new SelectListItem() { Text = "Контрольная группа", Value = "1" });
            group.Add(new SelectListItem() { Text = "Основная группа", Value = "2" });
            group.Add(new SelectListItem() { Text = "Группа сравнения", Value = "3" });
            return group;
        }
        private readonly Web2.Data.AppDbContext _context;

        public AnalyzesModel(Web2.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<Models.Analyzes> analyzes { get;set; } = default!;
        // получение анализов пациентов
        public async Task OnGetAsync(int? group)
        {
			string email = User.Identity.Name.ToString();
			string role = "";
			var userP = await _context.user.FromSqlRaw($"SELECT * FROM \"AspNetUsers\" WHERE \"UserName\" = \'{email}\' ").ToListAsync();
			string id = userP[0].Id.ToString();

			if (User.IsInRole("Admin"))
			{
				role = "Admin";
			}
			else
			{
				role = "Doctor";
			}

			if (_context.analyzes != null)
            {
                switch (role)
                {
                    case "Admin":
						if (group == 0 || group == null)
							analyzes = await _context.analyzes.FromSqlRaw($"SELECT * FROM analyzes ORDER BY id_analysis").ToListAsync();
						else
							analyzes = await _context.analyzes.FromSqlRaw($"SELECT * FROM analyzes WHERE id_analysis IN(SELECT id_analysis FROM patient_analyzes WHERE id_patient IN(SELECT id_patient FROM patient_archive WHERE id_patient IN(SELECT id_patient FROM archive_group WHERE id_group={group}) ORDER BY id_patient)) ORDER BY id_analysis").ToListAsync();
						break;
                    case "Doctor":
						if (group == 0 || group == null)
							analyzes = await _context.analyzes.FromSqlRaw($"SELECT * FROM analyzes WHERE id_analysis IN(SELECT id_analysis FROM patient_analyzes WHERE id_patient IN(SELECT id_patient FROM patient_archive WHERE id_patient IN(SELECT id_patient FROM user_patients WHERE id_user=\'{id}\'))) ORDER BY id_analysis").ToListAsync();
						else
							analyzes = await _context.analyzes.FromSqlRaw($"SELECT * FROM analyzes WHERE id_analysis IN(SELECT id_analysis FROM patient_analyzes WHERE id_patient IN(SELECT id_patient FROM patient_archive WHERE id_patient IN(SELECT id_patient FROM user_patients WHERE id_user=\'{id}\') INTERSECT (SELECT id_patient FROM archive_group WHERE id_group={group}))) ORDER BY id_analysis").ToListAsync();
						break;
                } 
            }
        }
        // выбор анализов для пациентов определённой группы
        public IActionResult OnPost(string G1)
        {
            int G = Convert.ToInt32(G1);
            string? url = Url.Page("Analyzes", new { group = G });

            return Redirect(url);
        }
        // удаление анализа из БД
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var b = await _context.analyzes.FindAsync(id);
            if (b != null)
            {
                _context.analyzes.Remove(b);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("./Analyzes");
        }
    }
}