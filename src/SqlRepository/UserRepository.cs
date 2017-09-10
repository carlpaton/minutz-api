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
                                                    ,@Active,
                                                    @InstanceId)";
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
  }
}
