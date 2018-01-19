using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using Interface.Repositories;
using Minutz.Models.Entities;

namespace SqlRepository
{
  public class UserRepository : IUserRepository
  {
    public bool CheckIfNewUser((string key, string reference) reference,
                               string authUserId,
                               string schema,
                               string connectionString)
    {
      if (string.IsNullOrEmpty(reference.key) || string.IsNullOrEmpty(reference.reference))
      {
        using (IDbConnection dbConnection = new SqlConnection(connectionString))
        {
          var sql = $"select Identityid FROM [{schema}].[Person]  WHERE Identityid = '{authUserId}' ";
          //dbConnection.Open ();
          var user = dbConnection.Query<Person>(sql, new { Identityid = (string)authUserId });
          return user.Any();
        }
      }

      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        var sql = $"select Identityid FROM [{schema}].[Person]  WHERE Identityid = '{authUserId}' ";
        //dbConnection.Open ();
        var user = dbConnection.Query<Person>(sql, new { Identityid = (string)authUserId });
        return user.Any();
      }
    }

    public string CreateNewUser(AuthRestModel authUser, string schema, string connectionString)
    {
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        var sql = $@"insert into [{schema}].[Person](
                                                    [Identityid]
                                                    ,[FirstName]
                                                    ,[LastName]
                                                    ,[FullName]
                                                    ,[ProfilePicture]
                                                    ,[Email]
                                                    ,[Role]
                                                    ,[Active]
                                                    ,[InstanceId]) 
                                            values(
                                                    @Identityid
                                                    ,@FirstName
                                                    ,@LastName
                                                    ,@FullName
                                                    ,@ProfilePicture
                                                    ,@Email
                                                    ,@Role
                                                    ,@Active
                                                    ,@InstanceId)";
        dbConnection.Open();
        var user = dbConnection.Execute(sql, new
        {
          Identityid = authUser.Sub,
          FirstName = string.Empty,
          LastName = string.Empty,
          FullName = authUser.Name,
          ProfilePicture = authUser.Picture,
          Email = authUser.Email,
          Role = "Attendee",
          Active = true,
          InstanceId = string.Empty
        });
        if (user == 1)
          return "Attendee";
        throw new System.Exception("There was a issue inserting the new user");
      }
    }

    public AuthRestModel GetUser(
      string authUserId,
      string schema,
      string connectionString)
    {
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        var sql = $"select * FROM [{schema}].[Person] WHERE Identityid ='{authUserId}'; ";
        //dbConnection.Open ();
        var query = dbConnection.Query<Person>(sql);
        if (query.Any())
        {
          var user = query.FirstOrDefault();
          if (user != null)
            return new AuthRestModel
            {
              Email = user.Email,
              Name = user.FullName,
              Nickname = user.FirstName,
              FirstName = user.FullName,
              LastName = user.LastName,
              Picture = user.ProfilePicture,
              Role = user.Role,
              Sub = user.Identityid,
              InstanceId = user.InstanceId
            };
        }
        throw new System.Exception("User does not exist in the datastore.");
      }
    }

    /// <summary>
    /// Creates the new schema and returns the schema that can be used
    /// </summary>
    /// <param name="authUser"></param>
    /// <param name="schema"></param>
    /// <param name="connectionString"></param>
    /// <returns>Schema value</returns>
    public string CreateNewSchema(
      AuthRestModel authUser,
      string schema,
      string connectionString,
      string masterConnectionString)
    {
      var id = authUser.Sub.Split('|')[1];
      var username = $"A_{id}";
      var password = CreatePassword(10);
      createSecurityUser(masterConnectionString, username, password);
      createLoginSchemaUser(connectionString, username);
      createSchema(connectionString, username);
      createInstanceRecord(connectionString, "app", authUser.Name, username, password, true, 1);
      updatePersonRecord(connectionString, "app", username, authUser.Sub);
      return username;
    }

    internal string CreatePassword(int length)
    {
      const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890123456789@#";
      StringBuilder res = new StringBuilder();
      Random rnd = new Random();
      while (0 < length--)
      {
        res.Append(valid[rnd.Next(valid.Length)]);
      }
      return res.ToString();
    }

    internal bool createSecurityUser(string connectionString, string user, string password)
    {
      try
      {
        using (IDbConnection dbConnection = new SqlConnection(connectionString))
        {
          var sql = $@"CREATE LOGIN {user}   
                      WITH PASSWORD = '{password}' ";

          return dbConnection.Execute(sql) == -1;
        }
      }
      catch (Exception ex)
      {

        return false;
      }
    }

    internal bool createSchema(string connectionString, string user)
    {
      try
      {
        using (IDbConnection dbConnection = new SqlConnection(connectionString))
        {
          var createSchema = $"CREATE schema {user} authorization {user};";
          var createSchemaResult = dbConnection.Execute(createSchema);
          return createSchemaResult == -1;
        }
      }
      catch (Exception)
      {
        return false;
      }
    }

    internal bool createLoginSchemaUser(string connectionString, string user)
    {
      try
      {
        using (IDbConnection dbConnection = new SqlConnection(connectionString))
        {
          var createSchemaLogin = $"CREATE USER {user} FOR LOGIN {user};";
          var createSchemaLoginResult = dbConnection.Execute(createSchemaLogin);
          return createSchemaLoginResult == -1;
        }
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    internal bool createInstanceRecord(
      string connectionString,
      string schema,
      string Name,
      string Username,
      string Password,
      bool Active,
      int Type)
    {
      try
      {
        using (IDbConnection dbConnection = new SqlConnection(connectionString))
        {
          var insertSql = $@"insert into [{schema}].[Instance](
                                                                 [Name]
                                                                ,[Username]
                                                                ,[Password]
                                                                ,[Active]
                                                                ,[Type]) 
                                                         values(
                                                                 @Name
                                                                ,@Username
                                                                ,@Password
                                                                ,@Active
                                                                ,@Type)";
          var instance = dbConnection.Execute(insertSql, new
          {
            Name,
            Username,
            Password,
            Active,
            Type
          });
          return instance == 1;
        }
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    internal bool updatePersonRecord(
      string connectionString,
      string schema,
      string username,
      string identity)
    {
      try
      {
        using (IDbConnection dbConnection = new SqlConnection(connectionString))
        {
          var updateUserSql = $"UPDATE [{schema}].[Person] SET InstanceId = '{username}' WHERE Identityid = '{identity}' ";
          var updateUserResult = dbConnection.Execute(updateUserSql);
          return updateUserResult == 1;
        }
      }
      catch (Exception)
      {
        return false;
      }
    }
  }
}