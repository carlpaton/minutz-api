
using tzatziki.minutz.models.Auth;

namespace tzatziki.minutz.interfaces.Repositories
{
  public interface IPersonRepository
  {
    bool IsPerson(string email);

    RoleEnum GetRole(string identifier, string connectionString, UserProfile profile);
  }
}
