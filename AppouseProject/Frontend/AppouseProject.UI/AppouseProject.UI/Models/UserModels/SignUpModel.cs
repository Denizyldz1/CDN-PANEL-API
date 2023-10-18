using System.ComponentModel.DataAnnotations;

namespace AppouseProject.UI.Models.UserModels
{
    public class SignUpModel
    {
        [Required(ErrorMessage = "UserName alanı zorunludur.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email alanı zorunludur.")]
        [EmailAddress(ErrorMessage ="Email formatı yanlış")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Şifre alanı zorunludur.")]
        [MinLength(8, ErrorMessage = "Şifre en az 8 karakter uzunluğunda olmalıdır.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\.)[a-zA-Z0-9.]{8,}$", ErrorMessage = "Şifre en az bir büyük harf ve bir nokta içermelidir.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "ConfirmPassword alanı zorunludur.")]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "UserType alanı zorunludur.")]
        public string UserType { get; set; }
    }
}
