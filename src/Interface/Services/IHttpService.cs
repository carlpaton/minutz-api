using System.Net.Http;

namespace Interface.Services
{
  public interface IHttpService
  {
    (bool condition, string result) Post(string endpoint, StringContent body);
    string Get(string endpoint, string token);
  }
}
