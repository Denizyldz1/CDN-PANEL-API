using System.ComponentModel.DataAnnotations;

namespace AppouseProject.Core.Dtos.AppUserModels
{
    public class AppUserWithRoleModel
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? UserType { get; set; }
    }
}
