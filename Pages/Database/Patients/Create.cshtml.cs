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
        // ����� ������
        public List<SelectListItem>? GetGroup()
        {
            List<SelectListItem> group = new List<SelectListItem>();
            group.Add(new SelectListItem() { Text = "����������� ������", Value = "1" });
            group.Add(new SelectListItem() { Text = "�������� ������", Value = "2" });
            group.Add(new SelectListItem() { Text = "������ ���������", Value = "3" });
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
        // ���������� ������ �������� � ��
        public async Task<IActionResult> OnPostAsync()
        {
            UP= new User_Patients();
            string email = User.Identity.Name.ToString();
            var userP = await _context.user.FromSqlRaw($"SELECT * FROM \"AspNetUsers\" WHERE \"UserName\" = \'{email}\' ").ToListAsync();
            string id = userP[0].Id.ToString();

            Archive.id_patient= Patient.id_patient;

            UP.id_patient = Patient.id_patient;
            UP.id_user = id;

            _context.patient_archive.Add(Patient);
            _context.archive_group.Add(Archive);
            _context.user_patients.Add(UP);
            await _context.SaveChangesAsync();

            string? url = Url.Page("Database", new { group = 0 });
            return Redirect(url);
        }
    }
}