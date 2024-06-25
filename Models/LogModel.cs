using System.ComponentModel.DataAnnotations;

namespace Web2.Models
{
    public class LogMod
	{
		[Required(ErrorMessage = "Не указан логин")]
		public string Login { get; set; }
		[Required(ErrorMessage = "Не указан пароль")]
		public string Password { get; set; }
	}
}