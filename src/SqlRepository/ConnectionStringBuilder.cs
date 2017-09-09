
using Interface.Services;

namespace SqlRepository
{
  public class ConnectionStringBuilder : IConnectionStringBuilder
  {
    public string Build(string server, 
                        string catalogue, 
                        string username, 
                        string password)
    {
      return $"Server=tcp:{server},1433;User ID={username};pwd={password};database={catalogue};";
    }
  }
}
