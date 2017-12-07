using Interface.Repositories;
using System.Data.SqlClient;
using Minutz.Models.Entities;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using System;

namespace SqlRepository
{
  public class UserRepository : IUserRepository
  {
    public bool CheckIfNewUser(string authUserId, string schema, string connectionString)
    {
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        var sql = $"select Identityid FROM [{schema}].[Person]  WHERE Identityid = @Identityid";
        dbConnection.Open();
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

    public AuthRestModel GetUser(string authUserId, string schema, string connectionString)
    {
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        var sql = $"select * FROM [{schema}].[Person] WHERE Identityid = @Identityid";
        dbConnection.Open();
        var query = dbConnection.Query<Person>(sql, new { Identityid = (string)authUserId });
        if (query.Any())
        {
          var user = query.FirstOrDefault();
          if (user != null)
            return new AuthRestModel
            {
              Email = user.Email,
              Name = user.FullName,
              Nickname = user.FirstName,
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
    public string CreateNewSchema(AuthRestModel authUser, string schema, string connectionString)
    {
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var id = authUser.Sub.Split('|')[1];
        var Password = CreatePassword(10);
        var Username = $"A_{id}";
        var Name = authUser.Name;
        var Active = true;
        var Type = 1;
        var loginSql = $"CREATE LOGIN A_{id} WITH PASSWORD = '{Password}'";
        var loginresult = dbConnection.Execute(loginSql);
        if (loginresult == -1)
        {
          var createUserSql = $"CREATE USER A_{id} FOR LOGIN A_{id} WITH DEFAULT_SCHEMA = A_{id};";
          var createUserResult = dbConnection.Execute(createUserSql);
          if (createUserResult == -1)
          {
            var createSchema = $"CREATE schema A_{id} authorization A_{id};";
            var createSchemaResult = dbConnection.Execute(createSchema);
            if (createSchemaResult == -1)
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
              if (instance == 1)
              {
                var updateUserSql = $"UPDATE [{schema}].[Person] SET InstanceId = '{Username}' WHERE Identityid = '{authUser.Sub}' ";
                var updateUserResult = dbConnection.Execute(updateUserSql);
                if (updateUserResult == 1)
                  return Username;
              }
            }
          }
        }
        throw new System.Exception("Error creating schema and user with authorization.");
      }
    }

    public string CreatePassword(int length)
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
  }
}
