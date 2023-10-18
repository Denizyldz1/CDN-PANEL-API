using AppouseProject.UI.Models;
using AppouseProject.UI.Models.FileModels;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AppouseProject.UI.Services
{
    public class FileService
    {
        private readonly HttpClient _httpClient;

        public FileService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<CustomResponseDto<List<FileModel>>> GetAllAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<FileModel>>>("file");
            return response;
        }


        public async Task<CustomResponseDto<List<FileModel>>> GetAllByUserIdAsync(string token,string userId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<FileModel>>>("file/" + userId);
            return response;
        }

        public async Task<CustomResponseDto<NoContentDto>> RemoveAsync(string token, int id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync("file/" + id);
            if (response.IsSuccessStatusCode)
            {
                return new CustomResponseDto<NoContentDto>() { IsSuccess = true };
            }
            else
            {
                return new CustomResponseDto<NoContentDto>() { IsSuccess = false };

            }
        }

        public async Task<CustomResponseDto<NoContentDto>> SaveAsync(ImageUploadModel model, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StreamContent(model.File.OpenReadStream())
                {
                    Headers =
            {
                ContentLength = model.File.Length,
                ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(model.File.ContentType)
            }
                }, "file", model.File.FileName);

                var response = await _httpClient.PostAsync("File", content);

                if (response.IsSuccessStatusCode)
                {
                    return new CustomResponseDto<NoContentDto>() { IsSuccess = true };
                }
                else
                {
                    var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<NoContentDto>>();
                    return new CustomResponseDto<NoContentDto>() { IsSuccess = false, Error = responseBody.Errors.FirstOrDefault() };
                }
            }
        }

    }
}

