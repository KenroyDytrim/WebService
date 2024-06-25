using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web2.Models;

namespace Web2.Pages.Database.Analyzes
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
        public Models.Analyzes Analyzes { get; set; }
        [BindProperty]
        public Patient_Analyzes P_A { get; set; }
        public IActionResult OnGet(int? id, int? var)
        {
            if (id != null)
            {
                idP = (int)id;
                varR = (int)var;
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int idP, int varR)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.analyzes.Add(Analyzes);
            await _context.SaveChangesAsync();

            P_A.id_analysis = Analyzes.id_analysis;
            _context.patient_analyzes.Add(P_A);
            await _context.SaveChangesAsync();

            string str="";
            switch (varR) 
            {
                case 0:
                    str="./Analyzes";
                    break;
                case 1:
                    str=$"/Database/Patient_Analyzes?id={idP}";
                    break;
            }
            return Redirect(str);
        }

    }
}