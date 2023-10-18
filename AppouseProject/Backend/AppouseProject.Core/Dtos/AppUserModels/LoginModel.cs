using System.ComponentModel.DataAnnotations;

namespace AppouseProject.Core.Dtos.AppUserModels
{
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
