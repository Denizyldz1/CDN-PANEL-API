using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AppouseProject.Core.Entities
{
    public class UserRefreshToken
    {
        public UserRefreshToken()
        {
            CreatedDate = DateTime.Now;
        }
        public int Id { get; set; }
        [ForeignKey(nameof(AppUser))]
        public int UserId { get; set; }
        public string RefreshToken { get; set; } = null!;
        public DateTime Expiration { get; set; }
        public DateTime CreatedDate { get; set; }

        public AppUser? AppUser { get; set; }

    }
}
