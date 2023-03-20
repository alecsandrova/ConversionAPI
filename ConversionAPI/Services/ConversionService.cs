using System.Net.Http;
using Newtonsoft.Json.Linq;
namespace ConversionAPI.Services
{
    public class ConversionService : IConversionService
    {
        private readonly HttpClient _httpClient;

        public ConversionService()
        {
            _httpClient = new HttpClient();
        }
        /// <summary>
        /// Gets all objects from selected API
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <returns></returns>
        public async Task<JObject> GetAll(string apiUrl)
        {
            var response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            var jsonLdData = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(jsonLdData);
            return data;
        }
    }
}
