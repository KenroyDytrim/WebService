using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Web2.Models;
using Web2.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web2.Pages.Database.Examinations
{
    public class ExaminationsModel : PageModel
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

        private readonly AppDbContext _context;

        public ExaminationsModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<Examination> examination { get; set; } = default!;
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

			if (_context.examination != null)
            {
				switch (role)
				{
					case "Admin":
						if (group == 0 || group == null)
							examination = await _context.examination.FromSqlRaw($"SELECT * FROM examination ORDER BY id_examination").ToListAsync();
						else
							examination = await _context.examination.FromSqlRaw($"SELECT * FROM examination WHERE id_examination IN(SELECT id_examination FROM patient_examinations WHERE id_patient IN(SELECT id_patient FROM patient_archive WHERE id_patient IN(SELECT id_patient FROM archive_group WHERE id_group={group}) ORDER BY id_patient)) ORDER BY id_examination").ToListAsync();
						break;
					case "Doctor":
						if (group == 0 || group == null)
							examination = await _context.examination.FromSqlRaw($"SELECT * FROM examination WHERE id_examination IN(SELECT id_examination FROM patient_examinations WHERE id_patient IN(SELECT id_patient FROM patient_archive WHERE id_patient IN(SELECT id_patient FROM user_patients WHERE id_user=\'{id}\'))) ORDER BY id_examination").ToListAsync();
						else
							examination = await _context.examination.FromSqlRaw($"SELECT * FROM examination WHERE id_examination IN(SELECT id_examination FROM patient_examinations WHERE id_patient IN(SELECT id_patient FROM patient_archive WHERE id_patient IN(SELECT id_patient FROM user_patients WHERE id_user=\'{id}\') INTERSECT (SELECT id_patient FROM archive_group WHERE id_group={group}))) ORDER BY id_examination").ToListAsync();
						break;
				}
            }
        }
        // выбор анализов для пациентов определённой группы
        public IActionResult OnPost(string G1)
        {
            int G = Convert.ToInt32(G1);
            string? url = Url.Page("Examinations", new { group = G });

            return Redirect(url);
        }
        // удаление анализа из БД
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var b = await _context.examination.FindAsync(id);
            if (b != null)
            {
                _context.examination.Remove(b);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("./Examinations");
        }
    }
}