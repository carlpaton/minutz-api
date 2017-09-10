using Interface.Repositories;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using Models.Entities;
using System.Linq;

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
          Identityid = authUser.sub,
          FirstName = string.Empty,
          LastName = string.Empty,
          FullName = authUser.name,
          ProfilePicture = authUser.picture,
          Email = authUser.email,
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
        var sql = $"select * FROM [{schema}].[Person]  WHERE Identityid = @Identityid";
        dbConnection.Open();
        var query = dbConnection.Query<Person>(sql, new { Identityid = (string)authUserId });
        if (query.Any())
        {
          var user = query.FirstOrDefault();
          return new AuthRestModel
          {
            email = user.Email,
            name = user.FullName,
            nickname = user.FirstName,
            picture = user.ProfilePicture,
            role = user.Role,
            sub = user.Identityid
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
        var id = authUser.sub.Split('|')[1];
        var password = "password12345$";
        var username = $"A_{id}";
        var loginSql = "CREATE LOGIN A_{id} WITH PASSWORD = @Password";
        var loginresult = dbConnection.Execute(loginSql, new { Password = (string)password});
        if (loginresult == 1)
        {
          var createUserSql = $"CREATE USER A_{id} FOR LOGIN A_{id} WITH DEFAULT_SCHEMA = A_{id};";
          var createUserResult = dbConnection.Execute(createUserSql);
          if (createUserResult == 1)
          {
            var createSchema = $"CREATE schema A_{id} authorization A_{id};";
            var createSchemaResult = dbConnection.Execute(createSchema);
            if (createSchemaResult == 1)
            {
              var insertSql = $@"insert into [{schema}].[Person](
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
                Name = authUser.name,
                Username = username,
                Password = password,
                Active = true,
                Type = 1
              });
              if (instance == 1)
              {
                var updateUserSql = $"UPDATE [{schema}].[Instance] SET InstanceId = '{username}' WHERE Identityid = '{authUser.sub}' ";
                var updateUserResult = dbConnection.Execute(updateUserSql);
                if (updateUserResult == 1)
                  return username;
              }
            }
          }
        }
        throw new System.Exception("Error creating schema and user with authorization.");
      }
    }
  }
}
