using Interface.Services;
using System.Net.Http;

namespace AuthenticationRepository
{
    public class HttpService : IHttpService
    {
        public (bool condition, string result) Get (
            string endpoint, string token)
        {
            var httpClient = new HttpClient ();
            httpClient.DefaultRequestHeaders.Add ("Authorization", $"Bearer {token}");
            HttpResponseMessage response = httpClient.GetAsync (endpoint).Result;
            if (response.IsSuccessStatusCode)
            {
                return (true, response.Content.ReadAsStringAsync ().Result);
            }
            return (false, response.ReasonPhrase);
        }

        public (bool condition, string result) Post (
            string endpoint, StringContent body)
        {
            var client = new HttpClient ();
            var result = client.PostAsync (endpoint, body).Result;
            return (result.IsSuccessStatusCode, result.Content.ReadAsStringAsync ().Result);
        }

        public (bool condition, string result) Post (
            string endpoint, StringContent body, string token)
        {
            var httpClient = new HttpClient ();
            httpClient.DefaultRequestHeaders.Add ("Authorization", token);
            var result = httpClient.PostAsync (endpoint, body).Result;
            return (result.IsSuccessStatusCode, result.Content.ReadAsStringAsync ().Result);
        }
    }
}