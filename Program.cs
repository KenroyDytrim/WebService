using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web2.Data;
using Web2.Models;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Добавление сервисов в контейнер.
        builder.Services.AddRazorPages(options =>
        {
            options.Conventions.AuthorizeFolder("/Database");
			options.Conventions.AuthorizeFolder("/Admin");
			options.Conventions.AuthorizeFolder("/Processing");
		});
        // Подключение базы данных.
        builder.Services.AddDbContext<AppDbContext>(
                        options => options.UseNpgsql(builder.Configuration.GetConnectionString("AppDbContext")));
        // Добавление аутентификации.
        builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        // Настройка аутентификации.
        builder.Services.Configure<IdentityOptions>(options =>
		{
			// Настройка блокировки по умолчанию.
			options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
			options.Lockout.MaxFailedAccessAttempts = 5;
			options.Lockout.AllowedForNewUsers = true;
			// Настройка пароля по умолчанию.
			options.Password.RequireDigit = false;
			options.Password.RequireLowercase = false;
			options.Password.RequireNonAlphanumeric = false;
			options.Password.RequireUppercase = false;
			options.Password.RequiredLength = 8;
			options.Password.RequiredUniqueChars = 1;
			// Настройка входа по умолчанию.
			options.SignIn.RequireConfirmedEmail = false;
			options.SignIn.RequireConfirmedPhoneNumber = false;
			// Настройка пользователя по умолчанию.
			options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
			options.User.RequireUniqueEmail = true;
		});
		// Настройка cookie файлов.
		builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => //Параметры аутентификации cookie
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Log");
                    options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Index");
				});

		var app = builder.Build();

        // Запуск автоматической миграции
        using var score = app.Services.CreateScope();
        score
            .ServiceProvider
            .GetRequiredService<AppDbContext>()
            .Database
            .Migrate();

        // Настройка конвейер HTTP-запросов.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        // Подключение аутентификации.
        app.UseAuthentication();
        app.UseAuthorization();
        // Настройка маршрутизации конечных точек для Razor Pages.
		app.MapRazorPages();
        // Добавление ролей для пользователей.
        using (var scope = app.Services.CreateScope())
        {
            var roleP = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roles = new[] { "Admin", "Doctor" };

            foreach (var role in roles)
            {
                if (!await roleP.RoleExistsAsync(role))
                    await roleP.CreateAsync(new IdentityRole(role));
            }
        }
        // Добавление групп для пациентов.
        using (var scope = app.Services.CreateScope())
        {
            var groupP = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (! groupP.patient_groups.Any())
            {

                groupP.patient_groups.AddRange(
                    new patient_group
                    {
                        id_group = 1,
                        title = "Контрольная группа"
                    },
                    new patient_group
                    {
                        id_group = 2,
                        title = "Основная группа"
                    },
                    new patient_group
                    {
                        id_group = 3,
                        title = "Группа сравнения"
                    }
                );
                groupP.SaveChanges();
            }
        }
        // Добавление администратора.
        using (var scope = app.Services.CreateScope())
        {
            var userP = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            string Login = "Kenroy";
            string Password = "14040205Dd!";
            string Surname = "Корней";
            string Name = "Дмитрий";
            string Phone = "89039192952";
            string Email = "dkorney@inbox.ru";

            if (await userP.FindByEmailAsync(Email) == null)
            {
                var user = new User();
                user.NormalizedEmail = Email;
                user.UserName = Email;
                user.Login = Login;
                user.Phone = Phone;
                user.Name = Name;
                user.Surname = Surname;
                user.Email = Email;

                await userP.CreateAsync(user, Password);
                await userP.AddToRoleAsync(user, "Admin");
            }
        }
        // Запуск веб-приложения.
        app.Run();
    }
}
