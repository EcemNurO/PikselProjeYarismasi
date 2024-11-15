using System.ComponentModel.DataAnnotations;

namespace Yarışma.Models
{
    public class LoginUserViewModel
    {
        [Required(ErrorMessage = "E-posta adresi gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir.")]
        [DataType(DataType.Password)]  
        public string Password { get; set; }
    }
}
