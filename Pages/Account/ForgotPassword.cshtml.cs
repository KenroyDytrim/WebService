using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web2.Models;

namespace Web2.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        public UserManager<User> _userManager;
        public ForgotPasswordModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        // отправка сообщения о сбросе пароля
        public async Task<IActionResult> OnPostForgotPassword(string Email)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Email);
                if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
                {
                    ModelState.AddModelError("", "Email �� ������");
                    return Page();
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Page(
                        "/Account/ResetPassword",
                        null,
                    new { userId = user.Id, code },
                    protocol: HttpContext.Request.Scheme);
                EmailService emailService = new EmailService();
                await emailService.SendEmailAsync(Email, "Reset Password",
                    $"Для сброса пароля пройдите по ссылке: <a href='{callbackUrl}'>link</a>");
            }
            return RedirectToPage("/Account/ForgotPasswordConfirmation");
        }
    }
}