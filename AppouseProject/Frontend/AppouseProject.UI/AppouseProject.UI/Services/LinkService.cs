using AppouseProject.UI.Models;
using AppouseProject.UI.Models.LinkModels;

namespace AppouseProject.UI.Services
{
    public class LinkService
    {
        private readonly HttpClient _httpClient;

        public LinkService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<LinkModel>> GetAllAsync()
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Options, "link");
            var response = await _httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var linkModels = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomResponseDto<List<LinkModel>>>(content);
                return linkModels.Data;


            }
            else
            {

                return Enumerable.Empty<LinkModel>();
            }

        }
    }
}
