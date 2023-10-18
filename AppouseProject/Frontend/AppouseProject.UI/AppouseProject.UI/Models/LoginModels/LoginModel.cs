using System.ComponentModel.DataAnnotations;

namespace AppouseProject.UI.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="Zorunlu alan")]
        public string UserName { get; set; } = null!;
        [Required(ErrorMessage = "Zorunlu alan")]
        public string Password { get; set; } = null!;
    }
}
