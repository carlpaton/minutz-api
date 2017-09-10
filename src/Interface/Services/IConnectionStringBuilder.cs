namespace Interface.Services
{
  public interface IConnectionStringBuilder
  {
    string Build(string server, string catalogue, string username, string password);
  }
}
