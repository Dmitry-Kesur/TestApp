using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DefaultNamespace
{
    public static class Api
    {
        private static HttpClient _httpClient;

        public static void InitializeApi()
        {
            _httpClient = new HttpClient();
        }

        public static async Task<T> Get<T>(string url)
        {
            var result = await _httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}