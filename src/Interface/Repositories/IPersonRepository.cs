using Interface.ViewModels;
using Minutz.Models.ViewModels;

namespace Interface.Repositories
{
  public interface IPersonRepository
  {
    UserProfile Get(string identifier, string email, string name, string picture, string connectionString);

    RoleEnum GetRole(string identifier, string connectionString, UserProfile profile);

    RoleEnum GetRole(string identifier, string connectionString, UserProfile profile, string schema);
  }
}