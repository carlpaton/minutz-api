using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using Interface.Repositories;
using Interface.Services;
using Minutz.Models.Entities;
using Minutz.Models.Message;
using SqlRepository.Extensions;

namespace SqlRepository
{
  public class UserRepository : IUserRepository
  {
    private readonly ILogService _logService;
    public UserRepository (ILogService logService)
    {
      this._logService = logService;
    }

    public PersonResponse MinutzPersonCheckIfUserExistsByEmail
      (string email, string minutzAppConnectionString)
    {
      var result = new PersonResponse { Condition = false, Message = string.Empty, People = new List<Person>()};
      try
      {
        using (IDbConnection dbConnection = new SqlConnection (minutzAppConnectionString))
        {
          var sql = $"SELECT * FROM [app].[Person] WHERE [Email] = '{email}' ";
          var userQueryResult = dbConnection.Query<Person> (sql).ToList ();
          if (!userQueryResult.Any())
          {
            result.Message = "No User with that email exists.";
            return result;
          }
          result.Condition = true;
          result.Person = userQueryResult.FirstOrDefault();
          result.People = userQueryResult;
        }
      }
      catch (Exception exception)
      {
        result.Message = exception.InnerException.Message;
        Console.WriteLine(exception);
      }
      return result;
    }

    public bool CheckIfNewUser
      ((string key, string reference) reference, string authUserId, string schema, string connectionString)
    {
      using (IDbConnection dbConnection = new SqlConnection (connectionString))
      {
        var sql = $"select Identityid FROM [{schema}].[Person]  WHERE Identityid = '{authUserId}' ";
        //dbConnection.Open ();
        var user = dbConnection.Query<Person> (sql, new { Identityid = (string) authUserId });
        return user.Any ();
      }
    }

    public MessageBase CreateNewUser
      (AuthRestModel authUser, string connectionString)
    {
      var result = new MessageBase{ Condition = false, Message =  string.Empty };
      using (IDbConnection dbConnection = new SqlConnection (connectionString))
      {
        var sql = $@"insert into [app].[Person]( [Identityid]
                                                ,[FirstName]
                                                ,[LastName]
                                                ,[FullName]
                                                ,[ProfilePicture]
                                                ,[Email]
                                                ,[Role]
                                                ,[Active]
                                                ,[InstanceId]
                                                ,[Related]) 
                                            values( @Identityid
                                                    ,@FirstName
                                                    ,@LastName
                                                    ,@FullName
                                                    ,@ProfilePicture
                                                    ,@Email
                                                    ,@Role
                                                    ,@Active
                                                    ,@InstanceId
                                                    ,@Related)";
        dbConnection.Open ();
        var user = dbConnection.Execute (sql, new
        {
          Identityid = authUser.Sub,
            FirstName = string.Empty,
            LastName = string.Empty,
            FullName = authUser.Name,
            ProfilePicture = authUser.Picture,
            Email = authUser.Email,
            Role = authUser.Role,
            Active = true,
            InstanceId = string.Empty,
            Related = authUser.Related
        });
        if (user == 1)
        {
          result.Condition = true;
          return result;
        }
        _logService.Log (Minutz.Models.LogLevel.Error, "There was a issue inserting the new user");
        result.Message = "There was a issue inserting the new user";
        return result;
      }
    }

    public string CreateNewUser
      ((string key, string reference) relationship, AuthRestModel authUser, string schema, string connectionString)
    {
      //check if key == guest then write - guest and use the instanceid update availibleattendees, 
      if (!string.IsNullOrEmpty (relationship.key))
      {
        authUser.Role = relationship.key;
        authUser.Related = relationship.reference;
      }
      else
      {
        authUser.Related = string.Empty;
      }

      using (IDbConnection dbConnection = new SqlConnection (connectionString))
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
                                                    ,[InstanceId]
                                                    ,[Related]) 
                                            values(
                                                    @Identityid
                                                    ,@FirstName
                                                    ,@LastName
                                                    ,@FullName
                                                    ,@ProfilePicture
                                                    ,@Email
                                                    ,@Role
                                                    ,@Active
                                                    ,@InstanceId
                                                    ,@Related)";
        dbConnection.Open ();
        var user = dbConnection.Execute (sql, new
        {
          Identityid = authUser.Sub,
            FirstName = string.Empty,
            LastName = string.Empty,
            FullName = authUser.Name,
            ProfilePicture = authUser.Picture,
            Email = authUser.Email,
            Role = authUser.Role,
            Active = true,
            InstanceId = string.Empty,
            Related = authUser.Related
        });
        if (user == 1)
          return "Guest";
        throw new System.Exception ("There was a issue inserting the new user");
      }
    }

    public AuthRestModel GetUser
      (string authUserId, string schema, string connectionString)
    {
      using (IDbConnection dbConnection = new SqlConnection (connectionString))
      {
        var sql = $"select * FROM [{schema}].[Person] WHERE Identityid ='{authUserId}'; ";
        //dbConnection.Open ();
        var query = dbConnection.Query<Person> (sql);
        if (query.Any ())
        {
          var user = query.FirstOrDefault ();
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
              InstanceId = user.InstanceId,
              Related = user.Related
            };
        }
        throw new System.Exception ("User does not exist in the datastore.");
      }
    }

    public string CreateNewSchema
      (AuthRestModel authUser, string connectionString, string masterConnectionString)
    {
      var id = authUser.Sub.Split ('|') [1];
      var username = $"A_{id}";
      var password = CreatePassword (10); //need to salt the password and username
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        var createUserSQL = $@" EXEC [app].[createUser] '{authUser.Sub}','{authUser.Email}', '{id}', '{password}'; ";
        var createUserSQLResult = dbConnection.Execute (createUserSQL);
      }

      //var createsecurityuserresult = createSecurityUser (masterConnectionString, username, password);
      //var createloginresult = createLoginSchemaUser (masterConnectionString, username);
      //var createshcemaresult = createSchema (connectionString, username);
      //var createinstancerecordresult = createInstanceRecord (masterConnectionString, "app", authUser.Name, username, password, true, 1);
      //var updatepersonresult = updatePersonRecord (masterConnectionString, "app", username, authUser.Sub);
      //var updaterolepersonresult = updatePersonRoleRecord (masterConnectionString, "app", authUser.Sub, authUser.Role);
      return username;
    }

    public (bool condition, string message) UpdatePerson
      (string connectionString, string schema, Person person)
    {
      try
      {
        using (IDbConnection dbConnection = new SqlConnection (connectionString))
        {
          var updateUserSql = $@"UPDATE [{schema}].[Person] 
          SET InstanceId = '{person.InstanceId}',
          [FirstName] = '{person.FirstName}' ,
          [LastName] = '{person.LastName}' ,
          [FullName] = '{person.FullName}' ,
          [ProfilePicture] = '{person.ProfilePicture}' ,
          [Role] = '{person.Role}' ,
          [Active] = '{person.Active}' ,
          [Related] = '{person.Related}' 
          WHERE Email = '{person.Email}' 
          ";
          var updateUserResult = dbConnection.Execute (updateUserSql);
          return (updateUserResult == 1, updateUserResult == 1 ? "Success": "There was a issue with the update of person.");
        }
      }
      catch (Exception ex)
      {
        return (false, $"There was a {ex.Message}");
      }
    }

    /// <summary>
    /// Reset tables and account.
    /// </summary>
    /// <returns>The reset.</returns>
    /// <param name="connectionString">Connection string.</param>
    /// <param name="instanceId">Instance identifier.</param>
    /// <param name="instanceName">This is the email address in the instance table i.e. info@docker.durban</param>
    public (bool condition, string message) Reset (
      string connectionString, string instanceId, string instanceName)
    {
      string sql = $@"
        EXECUTE [app].[resetAccount]'{instanceId}','{instanceName}','{instanceId}' 
        ALTER DATABASE [MINUTZ-TEST] set single_user with rollback immediate;  
        DROP SCHEMA {instanceId};
        DROP USER {instanceId};
        DROP LOGIN {instanceId};
        ALTER DATABASE [MINUTZ-TEST] set MULTI_USER;
      ";
      try
      {
        using (IDbConnection dbConnection = new SqlConnection (connectionString))
        {
          return (dbConnection.Execute (sql) == -1, "successful");
        }
      }
      catch (Exception ex)
      {
        return (false, ex.Message);
      }
    }

    internal string CreatePassword (int length)
    {
      const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890123456789@#";
      StringBuilder res = new StringBuilder ();
      Random rnd = new Random ();
      while (0 < length--)
      {
        res.Append (valid[rnd.Next (valid.Length)]);
      }
      return res.ToString ();
    }

    internal bool createSecurityUser (string connectionString, string user, string password)
    {
      try
      {
        using (IDbConnection dbConnection = new SqlConnection (connectionString))
        {
          var sql = $@"CREATE LOGIN {user}   
                      WITH PASSWORD = '{password}' ";
          var sqlresult = dbConnection.Execute(sql);
          return sqlresult == -1;
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine (ex.Message);
        return false;
      }
    }

    internal bool createSchema (string connectionString, string user)
    {
      try
      {
        using (IDbConnection dbConnection = new SqlConnection (connectionString))
        {
          var createSchema = $"CREATE schema {user} authorization {user};";
          var createSchemaResult = dbConnection.Execute (createSchema);
          return createSchemaResult == -1;
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine (ex.Message);
        return false;
      }
    }

    internal bool createLoginSchemaUser (string connectionString, string user)
    {
      try
      {
        using (IDbConnection dbConnection = new SqlConnection (connectionString))
        {
          var createSchemaLogin = $"CREATE USER {user} FOR LOGIN {user};";
          var createSchemaLoginResult = dbConnection.Execute (createSchemaLogin);
          return createSchemaLoginResult == -1;
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine (ex.Message);
        return false;
      }
    }

    internal bool createInstanceRecord (
      string connectionString, string schema, string Name, string Username, string Password, bool Active, int Type)
    {
      try
      {
        using (IDbConnection dbConnection = new SqlConnection (connectionString))
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
          var instance = dbConnection.Execute (insertSql, new
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
        Console.WriteLine (ex.Message);
        return false;
      }
    }

    internal bool updatePersonRecord (
      string connectionString, string schema, string username, string identity)
    {
      try
      {
        using (IDbConnection dbConnection = new SqlConnection (connectionString))
        {
          var updateUserSql = $"UPDATE [{schema}].[Person] SET InstanceId = '{username}' WHERE Identityid = '{identity}' ";
          var updateUserResult = dbConnection.Execute (updateUserSql);
          return updateUserResult == 1;
        }
      }
      catch (Exception)
      {
        return false;
      }
    }

    internal bool updatePersonRoleRecord (
      string connectionString, string schema, string identity, string role)
    {
      try
      {
        using (IDbConnection dbConnection = new SqlConnection (connectionString))
        {
          var updateUserSql = $"UPDATE [{schema}].[Person] SET [Role] = '{role}' WHERE Identityid = '{identity}' ";
          var updateUserResult = dbConnection.Execute (updateUserSql);
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