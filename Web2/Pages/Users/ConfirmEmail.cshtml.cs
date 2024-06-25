using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web2.Models;

namespace Web2.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
		UserManager<User> _userManager;
        public string Uid;
        public string codeT;

        public ConfirmEmailModel(UserManager<User> userManager)
		{
			_userManager = userManager;
		}
		[BindProperty]
		public User user { get; set; } = default!;
        // получение данных добавленного пользователя
		public async Task OnGetAsync(string userId, string code) 
		{
			user = await _userManager.FindByIdAsync(userId);
            Uid = userId;
            codeT = code;
		}
        // подтверждение пользователем почты после регистрации
        public async Task<IActionResult> OnPostAsync(string Uid, string codeT)
        {
            if (Uid == null || codeT == null)
            {
                return Redirect("/Error");
            }
            var user = await _userManager.FindByIdAsync(Uid);
            if (user == null)
            {
                return Redirect("/Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, codeT);
            if (result.Succeeded)
                return Redirect("/Index");
            else
                return Redirect("/Error");
        }

    }
}