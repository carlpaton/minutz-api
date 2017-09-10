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

    public string CreateNewUser(string authUserId, string schema, string connectionString)
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
        var user = dbConnection.Execute(sql, new {
          Identityid = authUserId,
          FirstName = string.Empty,
          LastName = string.Empty,
          FullName = string.Empty,
          ProfilePicture = string.Empty,
          Email = string.Empty,
          Role = "Attendee",
          Active = true,
          InstanceId = string.Empty
        });
        if(user == 1)
          return "Attendee";
        return "error";
      }
    }
  }
}
