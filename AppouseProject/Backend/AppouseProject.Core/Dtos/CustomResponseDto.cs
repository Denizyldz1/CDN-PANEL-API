using System.Text.Json.Serialization;

namespace AppouseProject.Core.Dtos
{
    public class CustomResponseDto<T>
    {
        public T? Data { get; set; }
        [JsonIgnore]
        public int StatusCode { get; set; }
        public List<String>? Errors { get; set; }
        public string? Message { get; set; }

        // İşlem başarılı ve geriye data döndürmek için
        public static CustomResponseDto<T> Success(int statusCode, T data)
        {
            return new CustomResponseDto<T> { Data = data, StatusCode = statusCode };
        }
        // İşlem başarılı ve geriye sadece durum kodu döndürmek için
        public static CustomResponseDto<T> Success(int statusCode)
        {
            return new CustomResponseDto<T> { StatusCode = statusCode };
        }
        public static CustomResponseDto<T> Success(int statusCode,string message)
        {
            return new CustomResponseDto<T> { StatusCode = statusCode,Message = message };
        }
        // Birde fazla error mesaj için
        public static CustomResponseDto<T> Failure(int statusCode, List<String> errors)
        {
            return new CustomResponseDto<T> { StatusCode = statusCode, Errors = errors };
        }
        public static CustomResponseDto<T> Failure(int statusCode, string errors)
        {
            return new CustomResponseDto<T> { StatusCode = statusCode, Errors = new List<string> { errors } };
        }
    }
}
