using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Web2.Models;

namespace Web2.Pages
{
    public class ResetPasswordModel : PageModel
    {
        public UserManager<User> _userManager;
        public ResetPasswordModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        // ����� ��� ������ ������
        public class ResetPasswordMod
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "������ ������ ��������� ��� ������� 6 ��������", MinimumLength = 6)]
            public string Password { get; set; }

            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "������ �� ���������")]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }
        }
        [BindProperty]
        public ResetPasswordMod mod { get; set; }
        // ����� ������ ��� ������������������� �������
        public IActionResult OnGet(string code)
        {
            mod = new ResetPasswordMod { Code = code };
            return code == null ? Redirect("./Error") : Page();
        }
        // ����� ������
        public async Task<IActionResult> OnPostResetPassword(ResetPasswordMod model)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Redirect("/Account/ResetPasswordConfirmation");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return Redirect("/Account/ResetPasswordConfirmation");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }
    }
}