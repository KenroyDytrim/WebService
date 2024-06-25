using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web2.Models;

namespace Web2.Pages.Database.Patients
{
    public class DatabaseModel : PageModel
    {
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

        public DatabaseModel(Web2.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<patient_archive> patient_archive { get;set; } = default!;
		public IList<User> user { get; set; } = default!;
        // получение архива пациентов
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

            if (_context.patient_archive != null)
            {
                switch (role)
                {
                    case "Admin":
						if (group == 0 || group == null)
							patient_archive = await _context.patient_archive.FromSqlRaw($"SELECT * FROM patient_archive ORDER BY id_patient").ToListAsync();
						else
							patient_archive = await _context.patient_archive.FromSqlRaw($"SELECT * FROM patient_archive WHERE id_patient IN(SELECT id_patient FROM archive_group WHERE id_group={group}) ORDER BY id_patient").ToListAsync();
						break;
                    case "Doctor":
						if (group == 0 || group == null)
							patient_archive = await _context.patient_archive.FromSqlRaw($"SELECT * FROM patient_archive WHERE id_patient IN(SELECT id_patient FROM user_patients WHERE id_user=\'{id}\') ORDER BY id_patient").ToListAsync();
						else
							patient_archive = await _context.patient_archive.FromSqlRaw($"SELECT * FROM patient_archive WHERE id_patient IN( (SELECT id_patient FROM user_patients WHERE id_user=\'{id}\') INTERSECT (SELECT id_patient FROM archive_group WHERE id_group={group})) ORDER BY id_patient").ToListAsync();
						break;
                }
            }
        }
        // вывод пациентов определённой группы
        public IActionResult OnPost(string G1)
        {
            int G = Convert.ToInt32(G1);
            string? url = Url.Page("Patients", new { group = G });

            return Redirect(url);
        }
        // удаление пациента из БД
        public async Task<IActionResult> OnPostDeleteAsync(int id)
		{
			var b = await _context.patient_archive.FindAsync(id);

            var A = await _context.analyzes.FromSqlRaw($"SELECT * FROM analyzes WHERE id_analysis IN (SELECT id_analysis FROM patient_analyzes WHERE id_patient={id})").ToListAsync();
			var C = await _context.examination.FromSqlRaw($"SELECT * FROM examination WHERE id_examination IN (SELECT id_examination FROM patient_examinations WHERE id_patient={id})").ToListAsync();

			if (b != null)
			{
				_context.patient_archive.Remove(b);  

				if (A != null)
				{
                    for (int i = 0; i < A.Count; i++)
                    {
                        _context.analyzes.Remove(A[i]);
                    }
				}
				if (C != null)
				{
					for (int i = 0; i < C.Count; i++)
					{
						_context.examination.Remove(C[i]);
					}
				}
				await _context.SaveChangesAsync();

            }
            string? url = Url.Page("Database", new { group = 0 });
            return Redirect(url);
        }
    }
}