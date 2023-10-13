using System.ComponentModel.DataAnnotations;

namespace AppouseProject.Core.Dtos.AppUserModels
{
    public class ChangePasswordModel
    {
        [Required]

        public string OldPassword { get; set; } = null!;

        [Required]
        public string NewPassword { get; set; } = null!;
        [Required]
        [Compare("NewPassword", ErrorMessage = "Şifreler uyuşmuyor")]
        public string ConfirmNewPassword { get; set; } = null!;
    }
}
