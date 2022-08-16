using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTest.DataAccess
{
    public class HttpDataAccess : IHttpDataAccess
    {
        public async Task<OutputType> GetAsync<OutputType>(string url)
            where OutputType : class
        {
            try { ApiHelper.Client.Timeout = Timeout.InfiniteTimeSpan; } catch { }

            using (HttpResponseMessage response = await ApiHelper.Client.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var output = await response.Content.ReadAsAsync<OutputType>();

                    return output;
                }
                else
                    throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<OutputType> GetAsync<OutputType>(string url, string token)
            where OutputType : class
        {
            if (!ApiHelper.Client.DefaultRequestHeaders.Contains("Authorization"))
                ApiHelper.Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            else ApiHelper.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using (HttpResponseMessage response = await ApiHelper.Client.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var output = await response.Content.ReadAsAsync<OutputType>();

                    return output;
                }
                else
                    throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<OutputType> PostAsync<OutputType>(string url, OutputType input)
            where OutputType : class
        {
            try { ApiHelper.Client.Timeout = Timeout.InfiniteTimeSpan; } catch { }

            using (HttpResponseMessage response = await ApiHelper.Client.PostAsJsonAsync(url, input))
            {
                if (response.IsSuccessStatusCode)
                {
                    var output = await response.Content.ReadAsAsync<OutputType>();

                    return output;
                }
                else
                    throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<OutputType> PostAsync<InputType, OutputType>(string url, InputType input)
            where OutputType : class
            where InputType : class
        {
            try { ApiHelper.Client.Timeout = Timeout.InfiniteTimeSpan; } catch { }

            using (HttpResponseMessage response = await ApiHelper.Client.PostAsJsonAsync(url, input))
            {
                if (response.IsSuccessStatusCode)
                {
                    var output = await response.Content.ReadAsAsync<OutputType>();

                    return output;
                }
                else
                    throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<OutputType> PostAsync<InputType, OutputType>(string url, string token, InputType input)
            where OutputType : class
            where InputType : class
        {
            try { ApiHelper.Client.Timeout = Timeout.InfiniteTimeSpan; } catch { }

            if (!ApiHelper.Client.DefaultRequestHeaders.Contains("Authorization"))
                ApiHelper.Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            else ApiHelper.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using (HttpResponseMessage response = await ApiHelper.Client.PostAsJsonAsync(url, input))
            {
                if (response.IsSuccessStatusCode)
                {
                    var output = await response.Content.ReadAsAsync<OutputType>();

                    return output;
                }
                else
                    throw new Exception(response.ReasonPhrase);
            }
        }
    }
}