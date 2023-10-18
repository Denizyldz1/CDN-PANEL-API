using System.ComponentModel.DataAnnotations;

namespace AppouseProject.UI.Models.UserModels
{
    public class ChangeEmailModel
    {
        [Required(ErrorMessage = "Email alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Email formatı yanlış")]
        public string Email { get; set; }
        [Compare("Email", ErrorMessage = "Emailler uyuşmuyor")]
        public string ConfirmEmail { get; set; }
    }
}
