using AppouseProject.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace AppouseProject.Core.Dtos.AppUserModels
{
    public class SignInModel
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        [EmailAddress(ErrorMessage = "Email formatı doğru görünmüyor")]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor")]
        public string ConfirmPassword { get; set; } = null!;
        public string? UserType { get; set; }
    }
}
