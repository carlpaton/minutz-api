using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Interface.Repositories;
using Interface.Services;
using Minutz.Models;
using Minutz.Models.Entities;

namespace SqlRepository {
  public class MeetingAttendeeRepository : IMeetingAttendeeRepository {
    private readonly ILogService _logService;
    public MeetingAttendeeRepository (ILogService logService) {
      this._logService = logService;
    }

    public MeetingAttendee Get
      (Guid id, string schema, string connectionString)
    {
      if (id == Guid.NewGuid () || string.IsNullOrEmpty (schema) || string.IsNullOrEmpty (connectionString))
        throw new ArgumentException ("Please provide a valid meeting attendee identifier, schema or connection string.");
      using (IDbConnection dbConnection = new SqlConnection (connectionString))
      {
        dbConnection.Open ();
        var sql = $"select * from [{schema}].[MeetingAttendee] WHERE Id = '{id.ToString()}'";
        var data = dbConnection.Query<MeetingAttendee> (sql).FirstOrDefault ();
        return data;
      }
    }

    public List<MeetingAttendee> GetMeetingAttendees
      (Guid referenceId, string schema, string connectionString, string masterConnectionString)
    {
      if (referenceId == Guid.NewGuid () || string.IsNullOrEmpty (schema) || string.IsNullOrEmpty (connectionString))
        throw new ArgumentException ("Please provide a valid meeting attendee identifier, schema or connection string.");
      
      var people = new List<Person>();
      var instance = new Instance();
      
      using (IDbConnection persondbConnection = new SqlConnection(masterConnectionString))
      {
        persondbConnection.Open ();
        var personSql = $"SELECT * FROM [app].[Person] ";
        people = persondbConnection.Query<Person> (personSql).ToList();
      }
      
      using (IDbConnection dbConnection = new SqlConnection (connectionString))
      {
        dbConnection.Open ();
        
        var sql = $@"select  att.Id, att.ReferanceId, att.PersonIdentity, att.Email, att.Role, att.Status
                    FROM [{schema}].[MeetingAttendee] att  
                    WHERE att.ReferanceId = '{referenceId.ToString()}'";
        var data = dbConnection.Query<MeetingAttendee> (sql);
        foreach (var item in data)
        {
          item.ReferenceId = referenceId;
          var personDetail = people.FirstOrDefault(i => i.Email == item.Email);
          if (personDetail != null)
          {
            item.Picture = string.IsNullOrEmpty(personDetail.ProfilePicture)
              ? $"assets/images/avatar-empty.png"
              : personDetail.ProfilePicture;
            item.Picture = personDetail.ProfilePicture;
            if (string.IsNullOrEmpty(personDetail.FirstName))
            {
              item.Name = string.IsNullOrEmpty(personDetail.FullName) 
                ? personDetail.Email.Split('@')[0] 
                : personDetail.FullName;
            }
            else
            {
              item.Name = $"{personDetail.FirstName} {personDetail.LastName}";
            }
            if (!string.IsNullOrEmpty(personDetail.Company))
            {
              item.Company = personDetail.Company;
            }
            
            if (!string.IsNullOrEmpty(personDetail.Department))
            {
              item.Department = personDetail.Department;
            }

          }
          else
          {
            if (string.IsNullOrEmpty(item.Name))
            {
              item.Name = string.IsNullOrEmpty(item.Email) 
                ? item.PersonIdentity.Split('@')[0] 
                : item.Email.Split('@')[0];
            }
            item.Picture = "assets/images/avatar-empty.png";
          }
        }
        return data.ToList ();
      }
    }

    public List<MeetingAttendee> GetAvalibleAttendees
      (string schema, string connectionString, string masterConnectionString)
    {
      var people = new List<Person>();
      using (IDbConnection masterdbConnection = new SqlConnection(masterConnectionString))
      {
        masterdbConnection.Open ();
        var personSql = $"select * from [app].[Person]";
        people = masterdbConnection.Query<Person> (personSql).ToList();
      }

      using (IDbConnection dbConnection = new SqlConnection (connectionString))
      {
        dbConnection.Open ();
        var sql = $"select * from [{schema}].[AvailibleAttendee]";
        var data = dbConnection.Query<MeetingAttendee> (sql).ToList();
        var result = new List<MeetingAttendee>();
        foreach (MeetingAttendee meetingAttendee in data)
        {
          var query = string.Empty;
          if (meetingAttendee.Email == null) query = meetingAttendee.PersonIdentity;
          if (meetingAttendee.Email != null) query = meetingAttendee.Email;
          
          if (people.Any(i => i.Email == query))
          {
            var person = people.First(i => i.Email == query);
            meetingAttendee.Name = string.IsNullOrEmpty(person.FullName) 
              ? person.Email.Split('@')[0].Replace(".", " ")
              : person.FullName;
            
            meetingAttendee.Email = string.IsNullOrEmpty(meetingAttendee.Email)
              ? meetingAttendee.PersonIdentity
              : meetingAttendee.Email;

            meetingAttendee.Picture = string.IsNullOrEmpty(person.ProfilePicture)
              ? $"assets/images/avatar-empty.png"
              : person.ProfilePicture;

            if (!string.IsNullOrEmpty(person.Company))
            {
              meetingAttendee.Company = person.Company;
            }
            
            if (!string.IsNullOrEmpty(person.Department))
            {
              meetingAttendee.Department = person.Department;
            }

            result.Add(meetingAttendee);
          }
          else
          {
            meetingAttendee.Name = meetingAttendee.PersonIdentity.Split('@')[0];
            meetingAttendee.Picture =
              $"{Environment.GetEnvironmentVariable("UI_BASE_URL")}/assets/images/avatar-empty.png";
            meetingAttendee.Email = meetingAttendee.PersonIdentity;
          }
        }
        return data.ToList ();
      }
    }

    public IEnumerable<MeetingAttendee> List
      (string schema, string connectionString)
    {
      if (string.IsNullOrEmpty (connectionString) || string.IsNullOrEmpty (schema))
        throw new ArgumentException ("Please provide a valid schema or connection string.");
      using (IDbConnection dbConnection = new SqlConnection (connectionString)) {
        dbConnection.Open ();
        var sql = $"select * from [{schema}].[MeetingAttendee]";
        var data = dbConnection.Query<MeetingAttendee> (sql).ToList ();
        return data;
      }
    }

    public bool Add
      (MeetingAttendee attendee, string schema, string connectionString) 
    {
      using (IDbConnection dbConnection = new SqlConnection (connectionString)) {
        if (attendee.Id != Guid.Empty) {
          attendee.Id = Guid.NewGuid ();
        }
        dbConnection.Open ();
        string insertSql = $@"insert into [{schema}].[MeetingAttendee](
                                                                 [Id]
                                                                ,[ReferanceId]
                                                                ,[PersonIdentity]
                                                                ,[Email]
                                                                ,[Role]
                                                                ,[Status]
                                                                ) 
                                                         values(
                                                                 @Id
                                                                ,@ReferenceId
                                                                ,@PersonIdentity
                                                                ,@Email
                                                                ,@Role
                                                                ,@Status
                                                                )";
        try
        {
          var instance = dbConnection.Execute (insertSql, new {
            attendee.Id,
            attendee.ReferenceId,
            attendee.PersonIdentity,
            attendee.Email,
            attendee.Role,
            attendee.Status
          });
          return instance == 1;
        }
        catch (Exception e)
        {
          _logService.Log(LogLevel.Exception, $"Add Meeting Attendee: {e.InnerException.Message}");
          return false;
        } 
      }
    }

    public bool AddAvailibleAttendee 
      (MeetingAttendee attendee, string schema, string connectionString) 
    {
      using (IDbConnection dbConnection = new SqlConnection (connectionString)) 
      {
        if (attendee.Id != Guid.Empty)
        {
          attendee.Id = Guid.NewGuid ();
        }
        dbConnection.Open ();
        string insertSql = $@"insert into [{schema}].[AvailibleAttendee](
                                                                 [Id]
                                                                ,[ReferanceId]
                                                                ,[PersonIdentity]
                                                                ,[Email]
                                                                ,[Status]
                                                                ,[Role]
                                                                ) 
                                                         values(
                                                                 @Id
                                                                ,@ReferenceId
                                                                ,@PersonIdentity
                                                                ,@Email
                                                                ,@Status
                                                                ,@Role
                                                                )";
        var instance = dbConnection.Execute (insertSql, new {
          attendee.Id,
          attendee.ReferenceId,
          attendee.PersonIdentity,
          attendee.Email,
          attendee.Status,
          attendee.Role
        });
        return instance == 1;
      }
    }
  
    public bool AddInvitee
      (MeetingAttendee attendee, string schema, string connectionString, string defaultConnectionString, string defaultSchema, string referenceMeetingId, string inviteEmail)
    {
      using (IDbConnection dbConnection = new SqlConnection (connectionString)) {
        string reference = $"invite|{schema}&{referenceMeetingId}";
        if (attendee.Id != Guid.Empty) {
          attendee.Id = Guid.NewGuid ();
        }
        dbConnection.Open ();
        string insertSql = $@"insert into [{schema}].[AvailibleAttendee](
                                                                 [Id]
                                                                ,[ReferanceId]
                                                                ,[PersonIdentity]
                                                                ,[Email]
                                                                ,[Status]
                                                                ,[Role]
                                                                ) 
                                                         values(
                                                                 @Id
                                                                ,@ReferenceId
                                                                ,@PersonIdentity
                                                                ,@Email
                                                                ,@Status
                                                                ,@Role
                                                                )";
        var instance = dbConnection.Execute (insertSql, new {
          attendee.Id,
          attendee.ReferenceId,
          attendee.PersonIdentity,
          inviteEmail,
          attendee.Status,
          attendee.Role
        });
        return instance == 1;
      }
    }

    public (bool condition, string message) UpdateInviteeStatus 
      (string personIdentity, string newPersonIdentity, string status, string schema, string connectionString)
    {
      using (IDbConnection dbConnection = new SqlConnection (connectionString)) {
        dbConnection.Open ();
        string availibleAttendeeSql = $@"UPDATE [{schema}].[AvailibleAttendee] SET 
                      [Status] = '{status}' ,
                      [PersonIdentity] = '{newPersonIdentity}'
                      WHERE [PersonIdentity] ='{personIdentity}'";
        string meetingAttendeeSql = $@"UPDATE [{schema}].[MeetingAttendee] SET 
                      [Status] = '{status}' ,
                      [PersonIdentity] = '{newPersonIdentity}'
                      WHERE [PersonIdentity] ='{personIdentity}'";
        try {
          var availibleAttendeeResult = dbConnection.Execute (availibleAttendeeSql);
          var meetingAttendeeResult = dbConnection.Execute (meetingAttendeeSql);
          return (true, "Success");
        } catch (Exception ex) {
          this._logService.Log (LogLevel.Error, $"((bool condition, string message) UpdateInviteeStatus(string personIdentity, string newPersonIdentity ,string status, string schema, string connectionString)), {ex.Message}");
          return (false, "Failed");
        }
      }
    }

    public bool Update
      (MeetingAttendee attendee, string schema, string connectionString, string masterConnectionString) 
    {
      try
      {
        bool result;
        using (IDbConnection dbConnection = new SqlConnection (connectionString))
        {
          dbConnection.Open ();
          string updateQuery = $@"UPDATE [{schema}].[MeetingAttendee] 
                             SET Referanceid = '{attendee.ReferenceId.ToString()}', 
                                 PersonIdentity = '{attendee.PersonIdentity}', 
                                 Role = '{attendee.Role}',
                                 Status = '{attendee.Status}'
                             WHERE Id = '{attendee.Id}' ";
          var instance = dbConnection.Execute (updateQuery);
          result  = (instance == 1);
        }
        
        using (IDbConnection dbConnection = new SqlConnection (masterConnectionString))
        {
          if (attendee.Company == null) attendee.Company = string.Empty;
          if (attendee.Department == null) attendee.Department = string.Empty;
          dbConnection.Open ();
          string personUpdateQuery = $@"UPDATE [app].[Person] 
                             SET Company = '{attendee.Company}', 
                                 Department = '{attendee.Department}'
                             WHERE Email = '{attendee.Email}' ";
          var instance = dbConnection.Execute (personUpdateQuery);
          result = (instance == 1);
        }
        return result;
      }
      catch (Exception e)
      {
        return false;
      }
    }

    public bool DeleteMeetingAttendees
      (Guid referanceId, string schema, string connectionString)
    {
      if (referanceId == Guid.NewGuid () || string.IsNullOrEmpty (schema) || string.IsNullOrEmpty (connectionString))
        throw new ArgumentException ("Please provide a valid meeting attendee identifier, schema or connection string.");
      using (IDbConnection dbConnection = new SqlConnection (connectionString))
      {
        dbConnection.Open ();
        var sql = $"delete from [{schema}].[MeetingAttendee] WHERE ReferanceId = '{referanceId.ToString()}'";
        var instance = dbConnection.Execute (sql);
        return instance == 1;
      }
    }

    public bool Delete
      (Guid id, string schema, string connectionString)
    {
      if (id == Guid.NewGuid () || string.IsNullOrEmpty (schema) || string.IsNullOrEmpty (connectionString))
        throw new ArgumentException ("Please provide a valid meeting attendee identifier, schema or connection string.");
      using (IDbConnection dbConnection = new SqlConnection (connectionString))
      {
        dbConnection.Open ();
        var sql = $"delete from [{schema}].[MeetingAttendee] WHERE Id = '{id.ToString()}'";
        var instance = dbConnection.Execute (sql);
        return instance == 1;
      }
    }
  }
}