using tzatziki.minutz.models.Auth;
using tzatziki.minutz.models.Entities;

namespace tzatziki.minutz.interfaces
{
  public interface IInstanceService
  {
    Instance Get(UserProfile user, string connectionString);
  }
}