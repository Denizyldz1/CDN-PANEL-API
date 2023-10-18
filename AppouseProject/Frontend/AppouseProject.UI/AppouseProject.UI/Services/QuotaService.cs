using AppouseProject.UI.Models;
using AppouseProject.UI.Models.QuotaModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace AppouseProject.UI.Services
{
    public class QuotaService : Controller
    {
        private readonly HttpClient _httpClient;

        public QuotaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CustomResponseDto<QuotaModel>> GetByUserNameAsync(string userName, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            try
            {

                var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<QuotaModel>>($"Quota/{userName}");
                return response;
            }
            catch (Exception ex)
            {

                string errorMessage = ex.Message;
                return new CustomResponseDto<QuotaModel>() { IsSuccess = false, Error = errorMessage };
            }
        }
    }
}
