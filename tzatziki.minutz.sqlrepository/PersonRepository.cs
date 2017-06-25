using System;
using System.Linq;
using tzatziki.minutz.interfaces.Repositories;
using tzatziki.minutz.models.Auth;
using tzatziki.minutz.models.Entities;

namespace tzatziki.minutz.sqlrepository
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
    /// <param name="identifier">      Auth0 identifier</param>
    /// <param name="connectionString">database connection string</param>
    /// <param name="profile">         Auth0 populated UserProfile instance</param>
    /// <returns>Enum.RoleEnum</returns>
    public RoleEnum GetRole(string identifier, string connectionString, UserProfile profile)
    {
      var user = GetUser(identifier, connectionString);
      if (user != null)
        return (RoleEnum)Enum.Parse(typeof(RoleEnum), user.Role);

      CreateUser(connectionString, profile);
      return RoleEnum.Attendee;
    }

    public UserProfile InsertInstanceIdForUser(UserProfile user, string connectionString)
    {
      using (var context = new DBConnectorContext(connectionString, "app"))
      {
        context.Database.EnsureCreated();

        var dbUser = context.Person.FirstOrDefault(i => i.Identityid == user.UserId);
        dbUser.InstanceId = user.InstanceId.ToString();
        dbUser.Role = RoleEnum.Admin.ToString();
        user.Role = RoleEnum.Admin.ToString();

        try
        {
          context.SaveChanges();
        }
        catch (Exception ex)
        {
          throw (ex);
        }
        return user;
      }
    }

    public void CreateInstanceUser(UserProfile userProfile, string connectionString, string schema)
    {
      using (var context = new DBConnectorContext(connectionString, schema))
      {
        try
        {
          context.Database.EnsureCreated();
          var user = CreateUser(connectionString, userProfile, schema);
        }
        catch (Exception ex)
        {
          throw (ex);
        }
      }
    }

    internal models.Entities.Person GetUser(string identifier, string connectionString, string schema = "app")
    {
      using (var context = new DBConnectorContext(connectionString, schema))
      {
        try
        {
          context.Database.EnsureCreated();
          return context.Person.FirstOrDefault(i => i.Identityid == identifier);
        }
        catch (Exception ex)
        {
          throw (ex);
        }
      }
    }

    internal Person CreateUser(string connectionString, UserProfile profile, string schema = "app")
    {
      using (var context = new DBConnectorContext(connectionString, schema))
      {
        context.Database.EnsureCreated();
        var userObject = new Person
        {
          FirstName = profile.Name,
          Email = profile.EmailAddress,
          Identityid = profile.UserId,
          Role = RoleEnum.Attendee.ToString(),
          Active = true
        };
        context.Person.Add(userObject);
        try
        {
          context.SaveChanges();
          return userObject;
        }
        catch (Exception ex)
        {
          throw (ex);
        }
      }
    }

		public Guid GetInstanceIdForUser(string userIdentifier, string connectionString, string schema = "app")
		{
			using (var context = new DBConnectorContext(connectionString, schema))
			{
				context.Database.EnsureCreated();
				var user = context.Person.FirstOrDefault(i => i.Identityid == userIdentifier);
				if (user == null)
					return Guid.Empty;
				if (user.InstanceId == null)
					return Guid.Empty;
				
				return Guid.Parse(user.InstanceId);
			}
		}
	}
}