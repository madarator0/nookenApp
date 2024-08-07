using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace nookenApp.Helper
{
    public class ApiHelper
    {
        private readonly HttpClient _client;
        private readonly string _url;

        public ApiHelper()
        {
            _client = new HttpClient();
            _url = Settings.ApiUrl;
        }

        public async Task<string> SendJsonAsync(object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(_url, content);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception($"Error: ----- {response.StatusCode}");
            }
        }
    }
}

