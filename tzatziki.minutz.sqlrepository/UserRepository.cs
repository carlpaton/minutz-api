using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using tzatziki.minutz.interfaces.Repositories;
using tzatziki.minutz.models.Auth;
using tzatziki.minutz.models.Entities;

namespace tzatziki.minutz.sqlrepository
{
  public class UserRepository : IUserRepository
  {
    public User Get(UserProfile userProfile, string connectionString, string schema)
    {
      var users = new List<User>();
      using (SqlConnection con = new SqlConnection(connectionString))
      {
        // Open the SqlConnection.
        con.Open();
        // The following code uses an SqlCommand based on the SqlConnection.

        using (SqlCommand command = new SqlCommand(SelectUserStatement(schema, userProfile.UserId), con))
        {
          using (SqlDataReader reader = command.ExecuteReader())
          {
            while (reader.Read())
            {
              users.Add(ToUser(reader));
            }
          }
        }
        users = ToList(schema, connectionString, userProfile);
        if (!users.Any())
        {
          using (SqlCommand command = new SqlCommand(InsertUserStatement(schema, userProfile), con))
          {
            try
            {
              command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
              throw (ex);
            }
          }

          users = new List<User>();
        }
      }

      return users.FirstOrDefault(i => i.Identity == userProfile.UserId);
    }

    public IEnumerable<User> GetUsers(string connectionString, string schema)
    {
      return ToList(schema, connectionString);
    }

    public User Update(string connectionString, string schema, UserProfile user)
    {
      return ToUser(connectionString, schema, user);
    }

    public User Update(string connectionString, string schema, User user)
    {
      return ToUser(connectionString, schema, user);
    }

    internal string SelectUserStatement(string schema, string identifier)
    {
      return $"SELECT * FROM [{schema}].[User] WHERE [Identity] = '{identifier}' ";
    }

    internal string SelectUsersStatement(string schema)
    {
      return $"SELECT * FROM [{schema}].[User] ";
    }

    internal string InsertUserStatement(string schema, UserProfile user)
    {
      return $@"INSERT INTO [{schema}].[User]
                ([Identity],[FirstName],[LastName],[FullName],[Email],[Role],[Active])
                VALUES
                ('{user.UserId}','','','{user.Name}','{user.EmailAddress}','{user.Role}',1)";
    }

    internal string UpdateUserSatement(string schema, UserProfile user)
    {
      var active = user.Active == true ? 1 : 0;
      return $@"UPDATE [{schema}].[User]
                SET [FirstName] = '{user.FirstName}', [LastName] = '{user.LastName}', [FullName] = '{user.Name}', [Email] = '{user.EmailAddress}', [Role] = '{user.Role}', [Active] = {active}
                WHERE [Identity] = '{user.UserId}' ";
    }

    internal string UpdateUserSatement(string schema, User user)
    {
      var active = user.Active == true ? 1 : 0;
      return $@"UPDATE [{schema}].[User]
                SET [FirstName] = '{user.FirstName}', [LastName] = '{user.LastName}', [FullName] = '{user.FullName}', [Email] = '{user.Email}', [Role] = '{user.Role}', [Active] = {active}
                WHERE [Identity] = '{user.Identity}' ";
    }

    internal List<User> ToList(string schema, string connectionString, UserProfile userProfile)
    {
      var result = new List<User>();
      using (SqlConnection con = new SqlConnection(connectionString))
      {
        using (SqlCommand command = new SqlCommand(SelectUserStatement(schema, userProfile.UserId), con))
        {
          using (SqlDataReader reader = command.ExecuteReader())
          {
            while (reader.Read())
            {
              result.Add(ToUser(reader));
            }
          }
        }
      }
      return result;
    }

    internal List<User> ToList(string schema, string connectionString)
    {
      var result = new List<User>();
      using (SqlConnection con = new SqlConnection(connectionString))
      {
        using (SqlCommand command = new SqlCommand(SelectUsersStatement(schema), con))
        {
          using (SqlDataReader reader = command.ExecuteReader())
          {
            while (reader.Read())
            {
              result.Add(ToUser(reader));
            }
          }
        }
      }
      return result;
    }

    internal User ToUser(string connectionString, string schema, UserProfile user)
    {
      using (SqlConnection con = new SqlConnection(connectionString))
      {
        using (SqlCommand command = new SqlCommand(UpdateUserSatement(schema, user), con))
        {
          try
          {
            command.ExecuteNonQuery();
          }
          catch (Exception ex)
          {
            throw new Exception($"Issue inserting the user record. {ex.Message}", ex.InnerException);
          }
        }
        return ToList(schema, connectionString, user).FirstOrDefault(i => i.Identity == user.UserId);
      }
    }

    internal User ToUser(string connectionString, string schema, User user)
    {
      using (SqlConnection con = new SqlConnection(connectionString))
      {
        using (SqlCommand command = new SqlCommand(UpdateUserSatement(schema, user), con))
        {
          try
          {
            command.ExecuteNonQuery();
          }
          catch (Exception ex)
          {
            throw new Exception($"Issue inserting the user record. {ex.Message}", ex.InnerException);
          }
        }
        return user;
      }
    }

    internal User ToUser(SqlDataReader dataReader)
    {
      return new User
      {
        Id = Int32.Parse(dataReader["Id"].ToString()),
        Active = bool.Parse(dataReader["Active"].ToString()),
        Email = dataReader["Email"].ToString(),
        FirstName = dataReader["FirstName"].ToString(),
        FullName = dataReader["FullName"].ToString(),
        Identity = dataReader["Identity"].ToString(),
        LastName = dataReader["LastName"].ToString(),
        Role = dataReader["Role"].ToString()
      };
    }
  }
}