
using System.Net.Http;

namespace Core.Helper
{
  public static class HttpService
  {
    public static string Get(string endpoint, string token)
    {
      var httpClient = new HttpClient();
      httpClient.DefaultRequestHeaders.Add("Authorization", token);
      HttpResponseMessage response = httpClient.GetAsync(endpoint).Result;
      if(response.IsSuccessStatusCode)
        return response.Content.ReadAsStringAsync().Result;
      return response.ReasonPhrase;
    }
  }
}
