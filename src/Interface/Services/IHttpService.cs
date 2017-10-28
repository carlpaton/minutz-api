namespace Interface.Services
{
  public interface IHttpService
  {
    string Get(string endpoint, string token);
  }
}
