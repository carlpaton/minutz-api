using Minutz.Models.Entities;

namespace Interface.Services
{
  public interface IApplicationManagerService
  {
    (bool condition, string message) StartFullVersion(AuthRestModel user);

    (bool condition, string message) ResetAcccount(AuthRestModel user);
  }
}
