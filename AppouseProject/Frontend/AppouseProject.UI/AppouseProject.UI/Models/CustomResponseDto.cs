namespace AppouseProject.UI.Models
{
    public class CustomResponseDto<T>
    {
        public T? Data { get; set; }
        public string  Error { get; set; }
        public List<string> Errors { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }

}
