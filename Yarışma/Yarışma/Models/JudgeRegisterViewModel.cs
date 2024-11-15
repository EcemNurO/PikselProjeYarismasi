using System.ComponentModel.DataAnnotations;

namespace Yarışma.Models
{
    public class JudgeRegisterViewModel
    {
        [Required]
        [Display(Name = "İsim")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre Onay")]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor.")]
        public string ConfirmPassword { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }    
        public int? JudgeCategoryId { get; set; } 
        public IFormFile image { get; set; }
        public string? Univercity { get; set; }
        public int ProjectCategoryId { get; set; }
        
     
    }
}

