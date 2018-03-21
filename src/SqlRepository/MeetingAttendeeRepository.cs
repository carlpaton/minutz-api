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
    /// <summary>
    /// Get the specified id, schema and connectionString.
    /// </summary>
    /// <returns>The get.</returns>
    /// <param name="id">Identifier.</param>
    /// <param name="schema">Schema.</param>
    /// <param name="connectionString">Connection string.</param>
    public MeetingAttendee Get (
      Guid id, string schema, string connectionString) {
      if (id == Guid.NewGuid () || string.IsNullOrEmpty (schema) || string.IsNullOrEmpty (connectionString))
        throw new ArgumentException ("Please provide a valid meeting attendee identifier, schema or connection string.");
      using (IDbConnection dbConnection = new SqlConnection (connectionString)) {
        dbConnection.Open ();
        var sql = $"select * from [{schema}].[MeetingAttendee] WHERE Id = '{id.ToString()}'";
        var data = dbConnection.Query<MeetingAttendee> (sql).FirstOrDefault ();
        return data;
      }
    }

    /// <summary>
    /// Gets the meeting attendees.
    /// </summary>
    /// <returns>The meeting attendees.</returns>
    /// <param name="referenceId">Reference identifier.</param>
    /// <param name="schema">Schema.</param>
    /// <param name="connectionString">Connection string.</param>
    public List<MeetingAttendee> GetMeetingAttendees (
      Guid referenceId, string schema, string connectionString, string masterConnectionString) {
      if (referenceId == Guid.NewGuid () || string.IsNullOrEmpty (schema) || string.IsNullOrEmpty (connectionString))
        throw new ArgumentException ("Please provide a valid meeting attendee identifier, schema or connection string.");
      var people = new List<Person>();
      using (IDbConnection persondbConnection = new SqlConnection(masterConnectionString))
      {
        persondbConnection.Open ();
        var personSql = $"SELECT * FROM [app].[Person] ";
        people = persondbConnection.Query<Person> (personSql).ToList();
      }

      using (IDbConnection dbConnection = new SqlConnection (connectionString)) {
        dbConnection.Open ();
        
        var sql = $@"select  att.Id, att.ReferanceId, att.PersonIdentity, att.Email,att.Role 
                    FROM [{schema}].[MeetingAttendee] att  
                    WHERE att.ReferanceId = '{referenceId.ToString()}'";
        var data = dbConnection.Query<MeetingAttendee> (sql);
        foreach (var item in data) {
          item.ReferenceId = referenceId;
          var personDetail = people.FirstOrDefault(i => i.Email == item.Email);
          if (personDetail != null)
          {
            item.Picture = personDetail.ProfilePicture;
            item.Name = $"{personDetail.FirstName} {personDetail.LastName}";
          }
        }
        return data.ToList ();
      }
    }

    /// <summary>
    /// Gets the avalible attendees.
    /// </summary>
    /// <returns>The avalible attendees.</returns>
    /// <param name="schema">Schema.</param>
    /// <param name="connectionString">Connection string.</param>
    /// <param name="masterConnectionString"></param>
    public List<MeetingAttendee> GetAvalibleAttendees (
      string schema, string connectionString, string masterConnectionString) {
      var people = new List<Person>();
      using (IDbConnection masterdbConnection = new SqlConnection(masterConnectionString))
      {
        masterdbConnection.Open ();
        var personSql = $"select * from [app].[Person]";
        people = masterdbConnection.Query<Person> (personSql).ToList();
      }

      using (IDbConnection dbConnection = new SqlConnection (connectionString)) {
        dbConnection.Open ();
        var sql = $"select * from [{schema}].[AvailibleAttendee]";
        var data = dbConnection.Query<MeetingAttendee> (sql);
        foreach (MeetingAttendee meetingAttendee in data)
        {
          var query = string.Empty;
          if (meetingAttendee.Email == null) query = meetingAttendee.PersonIdentity;
          if (meetingAttendee.Email != null) query = meetingAttendee.Email;
          var person = people.FirstOrDefault(i => i.Email == query);
          if (person != null)
          {
            if (string.IsNullOrEmpty(person.FullName))
            {
              meetingAttendee.Name = person.FirstName;
            }
            else
            {
              meetingAttendee.Name = person.FullName;
            }
            meetingAttendee.Picture = person.ProfilePicture;
            meetingAttendee.PersonIdentity = person.Identityid;
            meetingAttendee.Role = person.Role;
          }
          else
          {
            meetingAttendee.Name = query.Split('@')[0];
            meetingAttendee.Picture = $"{Environment.GetEnvironmentVariable("UI_BASE_URL")}/assets/images/avatar-empty.png";
            meetingAttendee.PersonIdentity = meetingAttendee.PersonIdentity;
            meetingAttendee.Role = meetingAttendee.Role;
          }
        }
        return data.ToList ();
      }
    }

    /// <summary>
    /// List the specified schema and connectionString.
    /// </summary>
    /// <returns>The list.</returns>
    /// <param name="schema">Schema.</param>
    /// <param name="connectionString">Connection string.</param>
    public IEnumerable<MeetingAttendee> List (
      string schema, string connectionString) {
      if (string.IsNullOrEmpty (connectionString) || string.IsNullOrEmpty (schema))
        throw new ArgumentException ("Please provide a valid schema or connection string.");
      using (IDbConnection dbConnection = new SqlConnection (connectionString)) {
        dbConnection.Open ();
        var sql = $"select * from [{schema}].[MeetingAttendee]";
        var data = dbConnection.Query<MeetingAttendee> (sql).ToList ();
        return data;
      }
    }

    /// <summary>
    /// Add the specified action, schema and connectionString.
    /// </summary>
    /// <returns>The add.</returns>
    /// <param name="attendee">Action.</param>
    /// <param name="schema">Schema.</param>
    /// <param name="connectionString">Connection string.</param>
    public bool Add (
      MeetingAttendee attendee, string schema, string connectionString) {
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
                                                                ) 
                                                         values(
                                                                 @Id
                                                                ,@ReferenceId
                                                                ,@PersonIdentity
                                                                ,@Email
                                                                ,@Role
                                                                )";
        var instance = dbConnection.Execute (insertSql, new {
          attendee.Id,
          attendee.ReferenceId,
          attendee.PersonIdentity,
          attendee.Email,
          attendee.Role
        });
        return instance == 1;
      }
    }

    public bool AddInvitee (
      MeetingAttendee attendee, string schema, string connectionString, string defaultConnectionString,
      string defaultSchema, string referenceMeetingId, string inviteEmail) {
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
            attendee.Email,
            attendee.Status,
            attendee.Role
        });
        return instance == 1;
      }
    }

    public (bool condition, string message) UpdateInviteeStatus (
      string personIdentity, string newPersonIdentity, string status, string schema, string connectionString) {
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

    /// <summary>
    /// Update the specified action, schema and connectionString.
    /// </summary>
    /// <returns>The update.</returns>
    /// <param name="attendee">Action.</param>
    /// <param name="schema">Schema.</param>
    /// <param name="connectionString">Connection string.</param>
    public bool Update (
      MeetingAttendee attendee, string schema, string connectionString) {
      using (IDbConnection dbConnection = new SqlConnection (connectionString)) {
        dbConnection.Open ();
        string updateQuery = $@"UPDATE [{schema}].[MeetingAttendee] 
                             SET Referanceid = '{attendee.ReferenceId.ToString()}', 
                                 PersonIdentity = '{attendee.PersonIdentity}', 
                                 Role = '{attendee.Role}'
                             WHERE Id = '{attendee.Id}' ";
        var instance = dbConnection.Execute (updateQuery);
        return instance == 1;
      }
    }

    /// <summary>
    /// Deletes the meeting attendees.
    /// </summary>
    /// <returns><c>true</c>, if meeting attendees was deleted, <c>false</c> otherwise.</returns>
    /// <param name="referanceId">Referance identifier.</param>
    /// <param name="schema">Schema.</param>
    /// <param name="connectionString">Connection string.</param>
    public bool DeleteMeetingAttendees (Guid referanceId, string schema, string connectionString) {
      if (referanceId == Guid.NewGuid () || string.IsNullOrEmpty (schema) || string.IsNullOrEmpty (connectionString))
        throw new ArgumentException ("Please provide a valid meeting attendee identifier, schema or connection string.");
      using (IDbConnection dbConnection = new SqlConnection (connectionString)) {
        dbConnection.Open ();
        var sql = $"delete from [{schema}].[MeetingAttendee] WHERE ReferanceId = '{referanceId.ToString()}'";
        var instance = dbConnection.Execute (sql);
        return instance == 1;
      }
    }

    /// <summary>
    /// Delete the specified id, schema and connectionString.
    /// </summary>
    /// <returns>The delete.</returns>
    /// <param name="id">Identifier.</param>
    /// <param name="schema">Schema.</param>
    /// <param name="connectionString">Connection string.</param>
    public bool Delete (Guid id, string schema, string connectionString) {
      if (id == Guid.NewGuid () || string.IsNullOrEmpty (schema) || string.IsNullOrEmpty (connectionString))
        throw new ArgumentException ("Please provide a valid meeting attendee identifier, schema or connection string.");
      using (IDbConnection dbConnection = new SqlConnection (connectionString)) {
        dbConnection.Open ();
        var sql = $"delete from [{schema}].[MeetingAttendee] WHERE Id = '{id.ToString()}'";
        var instance = dbConnection.Execute (sql);
        return instance == 1;
      }
    }
  }
}