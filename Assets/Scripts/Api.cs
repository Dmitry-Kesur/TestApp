using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DefaultNamespace
{
    public static class Api
    {
        private static HttpClient _webClient;

        public static void InitializeApi()
        {
            _webClient = new HttpClient();
        }
        
        public static async Task<T> Get<T>(string url)
        {
            var result = await _webClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}