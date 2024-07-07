using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web2.Models;

namespace Web2.Pages.Admin
{
    public class EditModel : PageModel
    {
	private readonly Web2.Data.AppDbContext _context;
	UserManager<User> _userManager;
        // выбор роли пользователя
	public List<SelectListItem>? GetRole()
	{
	    List<SelectListItem> role = new List<SelectListItem>();
	    role.Add(new SelectListItem() { Text = "Доктор", Value = "Doctor" });
	    role.Add(new SelectListItem() { Text = "Админ", Value = "Admin" });
	    return role;
	}
	public EditModel(Web2.Data.AppDbContext context, UserManager<User> userManager)
	{
	    _context = context;
	    _userManager = userManager;
        }			
	[BindProperty]
	public User user { get; set; }
        public IList<string> UserRoles { get; set; }
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
            UserRoles = await _userManager.GetRolesAsync(user);
	    return Page();
	}
        // изменение данных пользователя
        public async Task<IActionResult> OnPostEditAsync(IList<string> UserRoles)
        {
            var user2 = await _userManager.FindByIdAsync(user.Id);
            var userRoles = await _userManager.GetRolesAsync(user);
            var addedRoles = UserRoles.Except(userRoles);
            var removedRoles = userRoles.Except(UserRoles);

            await _userManager.AddToRolesAsync(user, addedRoles);

            await _userManager.RemoveFromRolesAsync(user, removedRoles);

            if (user != null)
            {
                user2.Name = user.Name;
                user2.Surname = user.Surname;
                user2.Email = user.Email;
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
                return Redirect("/Admin/AdminPanel");
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
