using AppouseProject.UI.Models;
using AppouseProject.UI.Models.UserModels;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace AppouseProject.UI.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<CustomResponseDto<SignUpModel>> SaveAsync(SignUpModel model,string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PostAsJsonAsync("User",model);
            if(response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<SignUpModel>>();
                responseBody.IsSuccess = true;
                return responseBody;
            }
            else
            {
                var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<NoContentDto>>();
                return new CustomResponseDto<SignUpModel>() { Error = responseBody.Errors.FirstOrDefault() };
            }
        }
        public async Task<CustomResponseDto<List<UserModel>>> GetAllAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            try
            {
                var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<UserModel>>>("User");
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                return new CustomResponseDto<List<UserModel>>() { IsSuccess = false ,Error = errorMessage};
            }
        }
        public async Task<CustomResponseDto<UserModel>> GetByIdAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            try
            {

                var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<UserModel>>($"User/{id}");
                return response;
            }
            catch (Exception ex)
            {

                string errorMessage = ex.Message;
                return new CustomResponseDto<UserModel>() { IsSuccess = false, Error = errorMessage };
            }
        }
        public async Task<CustomResponseDto<NoContentDto>> ChangeRoleAsync(string token , UserRoleModel model)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PatchAsJsonAsync($"User/ChangeRole", model);
            if(response.IsSuccessStatusCode)
            {
                return new CustomResponseDto<NoContentDto>() { IsSuccess = true };
            }
            return new CustomResponseDto<NoContentDto>() { IsSuccess = false };


        }
    }
}
