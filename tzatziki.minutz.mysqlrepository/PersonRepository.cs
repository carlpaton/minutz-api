using System;
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

    public UserProfile Get(string identifier, string email, string name, string connectionString)
    {
      var user = GetUser(identifier, connectionString);
      if (user != null)
      {
        return new UserProfile
        {
          InstanceId = string.IsNullOrEmpty(user.InstanceId) ? Guid.Empty : Guid.Parse(user.InstanceId),
          UserId = user.Identityid,
          Name = user.FirstName,
          EmailAddress = user.Email
        };
      }
      var newUserObject = new UserProfile
      {
        EmailAddress = email,
        UserId = identifier,
        Name = name
      };
      CreateUser(connectionString, newUserObject);
      return newUserObject;
    }

    /// <summary>
    /// Get the role for a user from the connector database
    /// </summary>
    /// <param name="identifier">Auth0 identifier</param>
    /// <param name="connectionString">database connection string</param>
    /// <param name="profile">Auth0 populated UserProfile instance</param>
    /// <returns>Enum.RoleEnum</returns>
    public RoleEnum GetRole(string identifier, string connectionString, UserProfile profile)
    {
      var user = GetUser(identifier, connectionString);
      if (user != null)
        return (RoleEnum)Enum.Parse(typeof(RoleEnum), user.Role);

      CreateUser(connectionString, profile);
      return RoleEnum.Attendee;
    }

    internal models.Entities.Person GetUser(string identifier, string connectionString)
    {
      using (var context = new DBConnectorContext(connectionString))
      {
        context.Database.EnsureCreated();
        return context.person.FirstOrDefault(i => i.Identityid == identifier);
      }
    }

    internal void CreateUser(string connectionString, UserProfile profile)
    {
      using (var context = new DBConnectorContext(connectionString))
      {
        context.Database.EnsureCreated();
        context.person.Add(new models.Entities.Person
        {
          FirstName = profile.Name,
          Email = profile.EmailAddress,
          Identityid = profile.UserId,
          //InstanceId = Guid.NewGuid().ToString(),
          Role = RoleEnum.Attendee.ToString()
        });
        context.SaveChanges();
      }
    }
  }
}
