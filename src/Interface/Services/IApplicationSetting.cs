namespace Interface.Services
{
  public interface IApplicationSetting
  {
    string Catalogue { get;  }
    string Schema { get; }
    string Username { get; }
    string Password { get; }
    string Server { get; }
    string Authority { get; }
    string CreateConnectionString(string server,
                                  string catalogue,
                                  string username,
                                  string password);
  }
}
