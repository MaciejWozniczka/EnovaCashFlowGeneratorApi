using System.Net.Http.Headers;

namespace ConsoleAppTest.DataAccess
{
    public class ApiHelper : HttpClient
    {
        private static HttpClient _httpClient;

        public static HttpClient Client
        {
            get
            {
                if (_httpClient == null)
                {
                    _httpClient = new HttpClient();
                    _httpClient.DefaultRequestHeaders.Accept.Clear();
                    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }

                return _httpClient;
            }
        }
    }
}