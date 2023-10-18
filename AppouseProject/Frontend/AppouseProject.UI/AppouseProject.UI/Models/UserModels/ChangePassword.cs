using System.ComponentModel.DataAnnotations;

namespace AppouseProject.UI.Models.UserModels
{
    public class ChangePassword
    {
        [Required(ErrorMessage = "Şifre alanı zorunludur.")]
        [MinLength(8, ErrorMessage = "Şifre en az 8 karakter uzunluğunda olmalıdır.")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Şifre alanı zorunludur.")]
        [MinLength(8, ErrorMessage = "Şifre en az 8 karakter uzunluğunda olmalıdır.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\.)[a-zA-Z0-9.]{8,}$", ErrorMessage = "Şifre en az bir büyük harf ve bir nokta içermelidir.")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "ConfirmPassword alanı zorunludur.")]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor")]
        public string ConfirmNewPassword { get; set; }

    }
}
