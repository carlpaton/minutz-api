
using tzatziki.minutz.models.Auth;

namespace tzatziki.minutz.interfaces.Repositories
{
  public interface IPersonRepository
  {
    bool IsPerson(string email);

    UserProfile Get(string identifier, string email, string name, string connectionString);

    RoleEnum GetRole(string identifier, string connectionString, UserProfile profile);
  }
}
