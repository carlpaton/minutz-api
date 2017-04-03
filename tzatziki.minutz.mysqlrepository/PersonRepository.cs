using System.Linq;
using tzatziki.minutz.interfaces.Repositories;
using tzatziki.minutz.models.Auth;

namespace tzatziki.minutz.mysqlrepository
{
  public class PersonRepository : IPersonRepository
  {
    public bool IsPerson(string email)
    {
      return false;
    }

    public RoleEnum GetRole(string identifier, string connectionString, UserProfile profile)
    {
      using (var context = new DBConnectorContext(connectionString))
      {
        context.Database.EnsureCreated();
        var user = context.person.FirstOrDefault(i => i.Identityid == identifier);
        if (user != null)
        {
          return RoleEnum.Attentee;
        }

        context.person.Add(new models.Entities.Person
        {
          FirstName = profile.Name,
          Email = profile.EmailAddress,
          Identityid = profile.UserId,
          //Id = profile.ClientID,
          Role = RoleEnum.Attentee.ToString()
        });
        context.SaveChanges();
        return RoleEnum.Attentee;
      }
    
    }
  }
}
