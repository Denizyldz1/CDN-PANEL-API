using Microsoft.AspNetCore.Http;

namespace AppouseProject.Core.Dtos.FileDtos
{
    public class FileDto :BaseDto
    {
        public string FileName { get; set; } = null!;
        public string FileType { get; set; } = null!;
        public int FileSize { get; set; }
        public string FilePatchUrl { get; set; } = null!;
        public int UserId { get; set; }
        public string? UserName { get; set; }

    }
}
