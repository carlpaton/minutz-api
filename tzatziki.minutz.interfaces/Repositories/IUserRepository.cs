using System.Collections.Generic;
using tzatziki.minutz.models.Auth;
using tzatziki.minutz.models.Entities;

namespace tzatziki.minutz.interfaces.Repositories
{
  public interface IUserRepository
  {
    bool ResetAccount(UserProfile userProfile, string connectionString, string schema);

    User Get(UserProfile userProfile, string connectionString, string schema);

    IEnumerable<User> GetUsers(string connectionString, string schema);

    User Update(string connectionString, string schema, UserProfile user);
  }
}