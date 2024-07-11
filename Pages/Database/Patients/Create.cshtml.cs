using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web2.Models;

namespace Web2.Pages.Database.Patients
{
    public class CreateModel : PageModel
    {
        private readonly Data.AppDbContext _context;
        // выбор группы
        public List<SelectListItem>? GetGroup()
        {
            List<SelectListItem> group = new List<SelectListItem>();
            group.Add(new SelectListItem() { Text = "Контрольная группа", Value = "1" });
            group.Add(new SelectListItem() { Text = "Основная группа", Value = "2" });
            group.Add(new SelectListItem() { Text = "Группа сравнения", Value = "3" });
            return group;
        }
        public CreateModel(Data.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public patient_archive Patient { get; set; }
		[BindProperty]
        public Archive_Group Archive { get; set; }
        public User_Patients UP { get; set; }
        // добавление нового пациента в БД
        public async Task<IActionResult> OnPostAsync()
        {
            UP= new User_Patients();
            string email = User.Identity.Name.ToString();
            var userP = await _context.user.FromSqlRaw($"SELECT * FROM \"AspNetUsers\" WHERE \"UserName\" = \'{email}\' ").ToListAsync();
            
            string id = userP[0].Id.ToString();
            UP.id_user = id;

            _context.patient_archive.Add(Patient);
            await _context.SaveChangesAsync();

            var newP = await _context.patient_archive.FromSqlRaw("SELECT * FROM patient_archive ORDER BY id_patient DESC LIMIT 1").ToListAsync();
            int idP = newP[0].id_patient;

            Archive.id_patient= idP;
            UP.id_patient = idP;
            
            _context.archive_group.Add(Archive);
            _context.user_patients.Add(UP);
            await _context.SaveChangesAsync();

            string? url = Url.Page("./Patients", new { group = 0 });
            return Redirect(url);
        }
    }
}
