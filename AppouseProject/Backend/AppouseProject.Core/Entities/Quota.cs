using System.ComponentModel.DataAnnotations.Schema;

namespace AppouseProject.Core.Entities
{
    public class Quota
    {
        [ForeignKey("AppUser")]
        public int Id { get; set; }
        public int StorageSpaceByte{ get; set; }
        public int UsedSpaceByte { get; set; }

        public AppUser? AppUser { get; set; }

    }
}
