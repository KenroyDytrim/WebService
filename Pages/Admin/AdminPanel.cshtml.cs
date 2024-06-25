using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Web2.Models;

namespace Web2.Pages
{
    [Authorize(Roles = "Admin")]
	public class AdminPanelModel : PageModel
    {
		private readonly Web2.Data.AppDbContext _context;
        UserManager<User> _userManager;
        public AdminPanelModel(Web2.Data.AppDbContext context, UserManager<User> userManager)
		{
			_context = context;
            _userManager = userManager;
		}
		[BindProperty]
		public IList<User> user { get; set; } = default!;
		// получение данных пользователей
		public async Task OnGetAsync()
		{
			user = await _context.user.FromSqlRaw($"SELECT * FROM \"AspNetUsers\" ").ToListAsync();
		}
		// удаление пользователей из БД
		public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            var b = await _userManager.FindByIdAsync(id);

            if (b != null)
            {
                _userManager.DeleteAsync(b);
            }
			return RedirectToPage("./AdminPanel");
		}
    }
}