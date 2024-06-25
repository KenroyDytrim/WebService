using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Web2.Models;

namespace Web2.Pages
{
    public class EditModel : PageModel
    {
		private readonly Web2.Data.AppDbContext _context;
		UserManager<User> _userManager;
		public EditModel(Web2.Data.AppDbContext context, UserManager<User> userManager)
		{
			_context = context;
			_userManager = userManager;
        }			

		[BindProperty]
		public User user { get; set; }
        // получение данных пользователя
        public async Task<IActionResult> OnGetAsync(string id)
		{
			if (id == null)
			{
				return NotFound();
			}
            user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
			return Page();
		}
        // изменение данных пользователя
        public async Task<IActionResult> OnPostEditAsync(IList<string> UserRoles)
        {
            var user2 = await _userManager.FindByIdAsync(user.Id);

            if (user != null)
            {
                user2.Name = user.Name;
                user2.Surname = user.Surname;
                user2.Phone = user.Phone;
            }

            IdentityResult result = await _userManager.UpdateAsync(user2);

            if (result.Succeeded)
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect("/Account/Log");
            }
            else
            {
                ModelState.AddModelError("", "Что-то пошло не так");
            }
            return Page();
        }

        private bool UserExists(string id)
        {
            return _context.user.Any(e => user.Id == id);
        }
    }
}
