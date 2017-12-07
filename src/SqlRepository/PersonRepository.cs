using Dapper;
using Interface.Repositories;
using Minutz.Models.Entities;
using Minutz.Models.ViewModels;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SqlRepository
{
  public class PersonRepository : IPersonRepository
  {
    public UserProfile Get(string identifier, string email, string name, string picture, string connectionString)
    {
      var user = GetUser(identifier, connectionString);
      if (user != null)
      {
        return new UserProfile
        {
          InstanceId = string.IsNullOrEmpty(user.InstanceId) ? Guid.Empty : Guid.Parse(user.InstanceId),
          UserId = user.Identityid,
          Name = $"{user.FirstName} {user.LastName}",
          FirstName = user.FirstName,
          LastName = user.LastName,
          ProfileImage = user.ProfilePicture,
          EmailAddress = user.Email
        };
      }
      var newUserObject = new UserProfile
      {
        EmailAddress = email,
        UserId = identifier,
        ProfileImage = picture,
        Name = name
      };

      var split = name.Split(' ');
      if (split.Length > 1)
      {
        newUserObject.FirstName = split[0];
        newUserObject.LastName = split[1];

      }
      if (split.Length == 1)
      {
        newUserObject.FirstName = name;
        newUserObject.LastName = string.Empty;
      }

      CreateUser(connectionString, newUserObject);
      return newUserObject;
    }

    public Interface.RoleEnum GetRole(string identifier, string connectionString, UserProfile profile)
    {
      var user = GetUser(identifier, connectionString);
      if (user != null)
        return (Interface.RoleEnum)Enum.Parse(typeof(Interface.RoleEnum), user.Role);

      CreateUser(connectionString, profile);
      return Interface.RoleEnum.Attendee;
    }

    public Interface.RoleEnum GetRole(string identifier, string connectionString, UserProfile profile, string schema)
    {
      var user = GetUser(identifier, connectionString);
      if (user != null)
        return (Interface.RoleEnum)Enum.Parse(typeof(Interface.RoleEnum), user.Role);

      CreateUser(connectionString, profile, schema);
      return Interface.RoleEnum.Attendee;
    }

    internal Person GetUser(string identifier, string connectionString, string schema = "app")
    {
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        try
        {
          dbConnection.Open();
          var data = dbConnection.Query<Person>($"select * FROM [{schema}].[Person] WHERE Identityid = '{identifier}' ");
          return data.FirstOrDefault();
        }
        catch (Exception ex)
        {
          throw (ex);
        }
      }
    }

    internal Person CreateUser(string connectionString, UserProfile profile, string schema = "app")
    {
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var userObject = new Person
        {
          FirstName = profile.FirstName,
          LastName = profile.LastName,
          FullName = $"{profile.FirstName} {profile.LastName}",
          Email = profile.EmailAddress,
          ProfilePicture = profile.ProfileImage,
          Identityid = profile.UserId,
          Role = Interface.RoleEnum.Attendee.ToString(),
          Active = true
        };
        var insertQuery = $@"INSERT INTO [{schema}].[Person]([Identityid], [FirstName], [LastName], [FullName], [ProfilePicture], [Email], [Role], [Active], [InstanceId])
														 VALUES(@Identityid, @FirstName, @LastName,@FullName, @ProfilePicture, @Email, @Role, @Active, @InstanceId)";
        try
        {
          dbConnection.Execute(insertQuery, userObject);
          return userObject;
        }
        catch (Exception ex)
        {
          throw (ex);
        }
      }
    }

  }
}