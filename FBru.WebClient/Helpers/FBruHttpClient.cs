using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace FBru.WebClient.Helpers
{
    public class FBruHttpClient
    {
        public static HttpClient GetClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(Constants.FBruApi);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }
}