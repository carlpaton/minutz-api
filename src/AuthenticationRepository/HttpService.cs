using Interface.Services;
using System.Net.Http;

namespace AuthenticationRepository
{
    public class HttpService : IHttpService
    {
        public string Get (
            string endpoint, string token)
        {
            throw new System.NotImplementedException ();
        }

        public (bool condition, string result) Post (
            string endpoint, StringContent body)
        {
            var client = new HttpClient ();
            var result = client.PostAsync (endpoint, body).Result;
            return (result.IsSuccessStatusCode, result.Content.ReadAsStringAsync ().Result);
        }
    }
}