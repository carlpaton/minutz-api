
namespace tzatziki.minutz.interfaces.Repositories
{
  public interface IPersonRepository
  {
    bool IsPerson(string email);

    RoleEnum GetRole(string identifier);
  }
}
