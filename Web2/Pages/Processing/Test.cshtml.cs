using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Web2.Models;

namespace Web2.Pages
{
    public class TestModel : PageModel
    {
        private readonly Web2.Data.AppDbContext _context;

        public TestModel(Web2.Data.AppDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Models.Analyzes Analyzes { get; set; }
        [BindProperty]
        public Examination Examination { get; set; }
        // получение данных для прогноза патологий
        public async Task<IActionResult> OnGetAsync(int? id_a, int? id_e)
        {

            Analyzes = await _context.analyzes.FindAsync(id_a);
            Examination = await _context.examination.FindAsync(id_e);

            return Page();
         }
    }
}