using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Web2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Web2.Pages
{
    public class LogModel : PageModel
    {
        private readonly Web2.Data.AppDbContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        public UserManager<User> _userManager;
        public LogModel(Web2.Data.AppDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, ILogger<LoginModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public IList<User> user { get; set; } = default!;
		[BindProperty]
		public LogMod mod { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public string ReturnUrl { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }
        // ����� ��� ����������� ������������
        public class LogMod
        {
            [Required]
            [EmailAddress]
            public string? Email { get; set; }

            [Required]
			public string? Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
        // �����������
        public async Task<IActionResult> OnPostLogin(LogMod mod)
        {
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = await _signInManager.PasswordSignInAsync(mod.Email, mod.Password, mod.RememberMe, lockoutOnFailure: false);
                if (user.Succeeded)
                {
                    await Authenticate(mod.Email); // ��������������

                    _logger.LogInformation("User logged in.");

					return RedirectToPage("./Log");
                }
                ModelState.AddModelError("", "�������� ����� ��� ������");
            }
            return Page();
        }
        // ��������������
        private async Task Authenticate(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var Role= await _userManager.GetRolesAsync(user);
            // ������� ���� claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, Role[0])
            };
            // ������� ������ ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // ��������� ������������������ ����
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        // ����� ������������
		public async Task<IActionResult> OnPostLogout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

			return RedirectToPage("./Log");
        }
        // ��������� ������ ������������
        public async Task OnGetAsync()
		{
            if (User.Identity.IsAuthenticated)
            {
                string Log = User.Identity.Name.ToString();
                if (_context.user != null)
                {
                    user = await _context.user.FromSqlRaw($"SELECT * FROM \"AspNetUsers\" WHERE \"UserName\"=\'{Log}\'").ToListAsync();
                }
            }
		}
	}
}