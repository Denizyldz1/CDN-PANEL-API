﻿using System.Text.Json.Serialization;

namespace AppouseProject.Core.Dtos
{
    public class NoContentDto
    {
        //CustomResponseDto'da T Data dönerken null dönmek istemez isek bunu oluşturabiliriz. İsteğe bağlı
        [JsonIgnore] // Bu veriyi json dönüştürürken Ignore(Yok say) et demek
        public int StatusCode { get; set; }
        public List<String>? Errors { get; set; }
        public string? Message { get; set; }
        public bool IsSuccess { get; set; }

        public static NoContentDto Success(int statusCode)
        {
            return new NoContentDto { StatusCode = statusCode ,IsSuccess=true };
        }
        public static NoContentDto Success(int statusCode,string message)
        {
            return new NoContentDto { StatusCode = statusCode,Message =message , IsSuccess = true };
        }

        public static NoContentDto Failure(int statusCode, List<String> errors)
        {
            return new NoContentDto { StatusCode = statusCode, Errors = errors , IsSuccess = false };
        }
        public static NoContentDto Failure(int statusCode, string errors)
        {
            return new NoContentDto { StatusCode = statusCode, Errors = new List<string> { errors } , IsSuccess = false };
        }
        
    }
}
