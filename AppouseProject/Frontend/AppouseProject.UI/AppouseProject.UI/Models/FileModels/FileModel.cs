namespace AppouseProject.UI.Models.FileModels
{
    public class FileModel
    {
        public int Id { get; set; }
        public string FileName { get; set; } = null!;
        public string FileType { get; set; } = null!;
        public int FileSize { get; set; }
        public string FilePatchUrl { get; set; } = null!;
        public int UserId { get; set; }
        public string? UserName { get; set; }
    }
}
