using System.ComponentModel.DataAnnotations;

namespace AppouseProject.Core.Entities
{
    public class ImageFile : BaseEntity
    {
        public string FileName { get; set; } = null!;
        public string FileType { get; set; } = null!;
        public int FileSize { get; set; }
        public string FilePatchUrl { get; set; } = null!;
        public int UserId { get; set; }
        public AppUser? AppUser { get; set; }
    }
}
