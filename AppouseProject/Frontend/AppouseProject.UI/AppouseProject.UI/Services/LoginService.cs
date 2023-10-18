using AppouseProject.UI.Models;
using AppouseProject.UI.Models.TokenModels;

namespace AppouseProject.UI.Services
{
    public class LoginService
    {
        private readonly HttpClient _httpClient;

        public LoginService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<CustomResponseDto<TokenModel>> CreateTokenAsync(LoginModel login)
        {
            var response = await _httpClient.PostAsJsonAsync("auth/createtoken", login);
            if (!response.IsSuccessStatusCode) 
            {
                var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<TokenModel>>();
                responseBody.IsSuccess = false;
                return responseBody;
            }
            else
            {
                var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<TokenModel>>();
                responseBody.IsSuccess = true;
                return responseBody;
            }

        }
    }
}
