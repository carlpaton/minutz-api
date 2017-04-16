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
        }
      }

      return users.FirstOrDefault(i => i.Identity == userProfile.UserId);
    }

    internal string SelectUserStatement(string schema, string identifier)
    {
      return $"SELECT * FROM [{schema}].[User] WHERE [Identity] = '{identifier}' ";
    }

    internal string InsertUserStatement(string schema, UserProfile user)
    {
      return $"INSERT INTO [{schema}].[User]([Identity],[FirstName],[LastName],[FullName],[Email],[Role],[Active])VALUES('{user.UserId}','','','{user.Name}','{user.EmailAddress}','{user.Role}',1)";
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