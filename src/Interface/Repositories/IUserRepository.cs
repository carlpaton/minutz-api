using Minutz.Models.Entities;

namespace Interface.Repositories
{
  public interface IUserRepository
  {
    bool CheckIfNewUser(
                        (string key, string reference) reference,
                        string authUserId,
                        string schema,
                        string connectionString);
    string CreateNewUser(
                        AuthRestModel authUser,
                        string schema,
                        string connectionString);
    AuthRestModel GetUser(
                          string authUserId,
                          string schema,
                          string connectionString);
    string CreateNewSchema(
                          AuthRestModel authUser,
                          string schema,
                          string connectionString,
                          string masterConnectionString);
  }
}
