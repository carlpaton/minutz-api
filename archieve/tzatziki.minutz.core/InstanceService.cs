using tzatziki.minutz.interfaces;
using tzatziki.minutz.interfaces.Repositories;
using tzatziki.minutz.models.Auth;
using tzatziki.minutz.models.Entities;

namespace tzatziki.minutz.core
{
  public class InstanceService : IInstanceService
  {
    private readonly IInstanceRepository _instanceRepository;
    private readonly IPersonRepository _personRepository;

    public InstanceService(IInstanceRepository instanceRepository, IPersonRepository personRepository)
    {
      _instanceRepository = instanceRepository;
      _personRepository = personRepository;
    }

    public Instance Get(UserProfile user, string connectionString)
    {
      return _instanceRepository.CreateInstance(connectionString, user);
    }
  }
}