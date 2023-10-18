using System.Text.Json.Serialization;

namespace AppouseProject.UI.Models
{
    public class NoContentDto
    {
        public List<String>? Errors { get; set; }
        public string? Message { get; set; }
  

    }

}
