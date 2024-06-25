using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web2.Models;

namespace Web2.Pages.Database.Examinations
{
    public class CreateModel : PageModel
    {
        private readonly Data.AppDbContext _context;

        public CreateModel(Data.AppDbContext context)
        {
            _context = context;
        }
        public int idP = 1;
        public int varR = 0;
        [BindProperty]
        public Examination Examination { get; set; }
        [BindProperty]
        public Patient_Examination P_E { get; set; }
        // вывод формы для добавления анализов
        public IActionResult OnGet(int? id, int? var)
        {
            if (id != null)
            {
                idP = (int)id;
                varR = (int)var;
            }
            return Page();
        }
        // добавление новых анализов в БД
        public async Task<IActionResult> OnPostAsync(int idP, int varR)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.examination.Add(Examination);
            await _context.SaveChangesAsync();

            P_E.id_examination = Examination.id_examination;
            _context.patient_examinations.Add(P_E);
            await _context.SaveChangesAsync();

            string str = "";
            switch (varR)
            {
                case 0:
                    str = "./Examinations";
                    break;
                case 1:
                    str = $"/Database/Patient_Examinations?id={idP}";
                    break;
            }
            return Redirect(str);
        }
    }
}