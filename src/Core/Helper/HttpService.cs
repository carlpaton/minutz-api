
using System.Net.Http;

namespace Core.Helper
{
  public static class HttpService
  {
    public static string Get(string endpoint, string token)
    {
      var httpClient = new HttpClient();
      httpClient.DefaultRequestHeaders.Add("Authorization",  $"Bearer {token}");
      HttpResponseMessage response = httpClient.GetAsync(endpoint).Result;
      if(response.IsSuccessStatusCode)
        return response.Content.ReadAsStringAsync().Result;
      return response.ReasonPhrase;
    }
    
    public static string Get(string endpoint, string token, string bearer)
    {
      var httpClient = new HttpClient();
      httpClient.DefaultRequestHeaders.Add("Authorization", $"{bearer} {token}");
      HttpResponseMessage response = httpClient.GetAsync(endpoint).Result;
      if(response.IsSuccessStatusCode)
        return response.Content.ReadAsStringAsync().Result;
      return response.ReasonPhrase;
    }
  }
}
