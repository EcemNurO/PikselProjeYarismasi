using System.ComponentModel.DataAnnotations;

namespace Yarışma.Areas.Management.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-posta adresi gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; }

        // Şifre için gerekli doğrulama
        [Required(ErrorMessage = "Şifre gereklidir.")]
        [DataType(DataType.Password)] // Şifre formatı için şifreyi gizler
        public string Password { get; set; }
    }
}
