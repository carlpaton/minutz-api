using System.Net.Http;

namespace Interface.Services
{
  public interface IHttpService
  {
    (bool condition, string result) Post(
      string endpoint, StringContent body);

    (bool condition, string result) Post (
      string endpoint, StringContent body, string token);
    (bool condition, string result) Get(
      string endpoint, string token);
  }
}
