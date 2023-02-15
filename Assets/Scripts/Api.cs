using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DefaultNamespace
{
    public static class Api
    {
        private static WebClient _webClient;

        public static void InitializeApi()
        {
            _webClient = new WebClient();
        }
        
        public static async Task<T> Get<T>(string url)
        {
            var result = await _webClient.DownloadStringTaskAsync(url);
            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}