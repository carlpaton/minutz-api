namespace Interface.Repositories
{
  public interface IUserRepository
  {
    bool CheckIfNewUser(string authUserId, string schema, string connectionString);
    string CreateNewUser(string authUserId, string schema, string connectionString);
  }
}
