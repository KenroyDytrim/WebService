using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web2.Models;
using DocumentFormat.OpenXml.Wordprocessing;
using System.ComponentModel.DataAnnotations;

namespace Web2.Pages.Admin
{
    public class CreateModel : PageModel
	{
		UserManager<User> _userManager;
		// ������ ��� ���������� ������������
        public class InputModel
        {
            public string Role { get; set; }

            [Required(ErrorMessage = "�� ������ ������")]
			[DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
			[StringLength(100, ErrorMessage = "����� {0} ������ ���� �� ����� {2} � �� ����� {1} ��������.", MinimumLength = 8)]
            [Display(Name = "Password")]
            public string Password { get; set; }
			[Required(ErrorMessage = "�� ������ Email")]
			public string Email { get; set; }
			[Required(ErrorMessage = "�� ������ �����")]
			public string Login { get; set; }
			[Required(ErrorMessage = "�� ������� �������")]
			public string Surname { get; set; }
			[Required(ErrorMessage = "�� ������� ���")]
			public string Name { get; set; }
			[Required(ErrorMessage = "�� ������ ����� ��������")]
			[Phone]
			public string Phone { get; set; }

		}

        [BindProperty]
        public InputModel Input { get; set; }
		// ����� ���� ������������
        public List<SelectListItem>? GetRole()
        {
            List<SelectListItem> role = new List<SelectListItem>();
            role.Add(new SelectListItem() { Text = "������", Value = "Doctor" });
            role.Add(new SelectListItem() { Text = "�����", Value = "Admin" });
            return role;
        }
        public CreateModel(Web2.Data.AppDbContext context, UserManager<User> userManager)
		{
			_userManager = userManager;
		}
		[BindProperty]
		public User user { get; set; }
		public IActionResult OnGet()
		{
			return Page();
		}
		// ���������� ������������ � �������� ��������� ��� ������������� �����������
		public async Task<IActionResult> OnPostAsync()
		{
			if (await _userManager.FindByEmailAsync(Input.Email) == null)
			{
				user.Login = Input.Login;
				user.Phone = Input.Phone;
				user.Name = Input.Name;
				user.Surname = Input.Surname;
				user.NormalizedEmail = Input.Email;
				user.UserName = Input.Email;
				user.Email = Input.Email;

				await _userManager.CreateAsync(user, Input.Password);
                var result = await _userManager.AddToRoleAsync(user, Input.Role);

                if (result.Succeeded)
				{
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Users/ConfirmEmail",
						null,
                        new { userId = user.Id, code = code },
                        protocol: HttpContext.Request.Scheme);
                    EmailService emailService = new EmailService();
                    await emailService.SendEmailAsync(user.Email, "Confirm your account",
                        $"����������� �����������, ������� �� ������: <a href='{callbackUrl}'>link</a>");

                    return Redirect("/Admin/AdminPanel");
				}
			}
			return Page();
		}
    }
}