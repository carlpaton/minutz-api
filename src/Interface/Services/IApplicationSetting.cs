namespace Interface.Services
{
  public interface IApplicationSetting
  {
    string Catalogue { get;  }

    string Schema { get; }

    string Username { get; }

    string Password { get; }
  }
}
