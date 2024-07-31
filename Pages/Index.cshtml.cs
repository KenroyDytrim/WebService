using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web2.Models;

namespace Web2.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
         // письмо от пользователя
        public async Task<IActionResult> OnPostAsync(string name, string email, string phone, string message)
        {
            EmailService emailService = new EmailService();
            await emailService.SendEmailAsync("korney1404@gmail.com", "Письмо от пользователя",
                $"{message} <div><p>Имя: {name}</p> <p> Email: {email}</p> <p> Телефон: {phone}</p> </div>");
            return Page();
        }
    }
}
