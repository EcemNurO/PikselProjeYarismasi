using System.ComponentModel.DataAnnotations;

namespace Yarışma.Areas.Management.Models
{
	public class LoginViewModel
	{
			[Required(ErrorMessage = "Mail Alanı Boş Olamaz")]
			public string Email { get; set; }
			[Required(ErrorMessage = "Şifre Alanı Boş Olamaz")]
			public string Password { get; set; }
		
	}
}
