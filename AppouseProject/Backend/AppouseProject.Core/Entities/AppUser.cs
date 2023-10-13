using Microsoft.AspNetCore.Identity;

namespace AppouseProject.Core.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public AppUser()
        {
            IsActive = true;
            CreatedDate = DateTime.Now;
        }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual ICollection<ImageFile>? Files { get; set; }
        public virtual ICollection<UserRefreshToken>? UserRefreshTokens{ get; set; }
        public Quota? Quota { get; set; }
    }
}
