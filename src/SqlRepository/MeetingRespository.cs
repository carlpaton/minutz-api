using System.Collections.Generic;
using Interface.Repositories;
using System.Data.SqlClient;
using Minutz.Models.Entities;
using System.Data;
using System.Linq;
using System;
using Dapper;
using Minutz.Models.ViewModels;

namespace SqlRepository
{
  public class MeetingRepository : IMeetingRepository
  {
    /// <summary>
    /// Get the specified id, schema and connectionString.
    /// </summary>
    /// <returns>The get.</returns>
    /// <param name="id">Identifier.</param>
    /// <param name="schema">Schema.</param>
    /// <param name="connectionString">Connection string.</param>
    public Meeting Get
      (Guid id, string schema, string connectionString)
    {
      if (id == Guid.NewGuid() || string.IsNullOrEmpty(schema) || string.IsNullOrEmpty(connectionString))
        throw new ArgumentException("Please provide a valid meeting identifier, schema or connection string.");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var sql = $"select * from [{schema}].[Meeting] WHERE Id = '{id.ToString()}'";
        var data = dbConnection.Query<Meeting>(sql).FirstOrDefault();
        return data;
      }
    }

    /// <summary>
    /// List the specified meetings for schema and connectionString.
    /// </summary>
    /// <returns>The list.</returns>
    /// <param name="schema">Schema.</param>
    /// <param name="connectionString">Connection string.</param>
    public List<Meeting> List
      (string schema, string connectionString)
    {
      if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(schema))
        throw new ArgumentException("Please provide a valid schema or connection string.");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var sql = $"select * from [{schema}].[Meeting]";
        var data = dbConnection.Query<Meeting>(sql).ToList();
        return data;
      }
    }

    public List<Meeting> List
      (string schema, string connectionString, List<string> meetingIds)
    {
      if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(schema))
        throw new ArgumentException("Please provide a valid schema or connection string.");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var sql = $"select * from [{schema}].[Meeting]";
        var data = dbConnection.Query<Meeting>(sql).ToList();
        List<Meeting> filtered = new List<Meeting>();
        foreach (var meeting in data)
        {
          foreach (var filterId in meetingIds)
          {
            if (meeting.Id.ToString() == filterId)
              filtered.Add(meeting);
          }
        }
        return filtered;
      }
    }

    /// <summary>
    /// Add the specified action, schema and connectionString.
    /// </summary>
    /// <returns>The add.</returns>
    /// <param name="action">Action.</param>
    /// <param name="schema">Schema.</param>
    /// <param name="connectionString">Connection string.</param>
    public bool Add
      (Meeting action, string schema, string connectionString)
    {
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        string insertSql = $@"insert into [{schema}].[Meeting](
                                                                 [Id]
                                                                ,[Name]
                                                                ,[Date]
                                                                ,[UpdatedDate]
                                                                ,[Time]
                                                                ,[Duration]
                                                                ,[IsReacurance]
                                                                ,[IsPrivate]
                                                                ,[ReacuranceType]
                                                                ,[IsLocked]
                                                                ,[Status]
                                                                ,[IsFormal]
                                                                ,[TimeZone]
                                                                ,[Tag]
                                                                ,[Purpose]
                                                                ,[MeetingOwnerId]
                                                                ,[Outcome]
                                                                ) 
                                                         values(
                                                                 @Id
                                                                ,@Name
                                                                ,@Date
                                                                ,@UpdatedDate
                                                                ,@Time
                                                                ,@Duration
                                                                ,@IsReacurance
                                                                ,@IsPrivate
                                                                ,@ReacuranceType
                                                                ,@IsLocked
                                                                ,@Status
                                                                ,@IsFormal
                                                                ,@TimeZone
                                                                ,@Tag
                                                                ,@Purpose
                                                                ,@MeetingOwnerId
                                                                ,@Outcome
                                                                )";
        try
        {
          var instance = dbConnection.Execute(insertSql, new
          {
            action.Id,
            action.Name,
            action.Date,
            action.UpdatedDate,
            action.Time,
            action.Duration,
            action.IsReacurance,
            action.IsPrivate,
            action.ReacuranceType,
            action.IsLocked,
            action.Status,
            action.IsFormal,
            action.TimeZone,
            action.Tag,
            action.Purpose,
            action.MeetingOwnerId,
            action.Outcome
          });
          return instance == 1;
        }
        catch (Exception ex)
        {
          throw new Exception(ex.Message);
        }

      }
    }

    /// <summary>
    /// Update the specified meeting, schema and connectionString.
    /// </summary>
    /// <returns>The update.</returns>
    /// <param name="meeting">Meeting.</param>
    /// <param name="schema">Schema.</param>
    /// <param name="connectionString">Connection string.</param>
    public bool Update
      (Meeting meeting, string schema, string connectionString)
    {
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        string updateQuery = $@"UPDATE [{schema}].[Meeting] 
                             SET Name = @Name, 
                                 Date = @Date, 
                                 UpdatedDate = @UpdatedDate, 
                                 Time = @Time,
                                 Duration = @Duration, 
                                 Location = @Location,
                                 IsReacurance = @IsReacurance, 
                                 IsPrivate = @IsPrivate,
                                 ReacuranceType = @ReacuranceType, 
                                 IsLocked = @IsLocked,
                                 Status = @Status,
                                 IsFormal = @IsFormal,
                                 TimeZone = @TimeZone,
                                 Tag = @Tag,
                                 Purpose = @Purpose,
                                 MeetingOwnerId = @MeetingOwnerId, 
                                 Outcome = @Outcome
                             WHERE Id   = @Id";
        var instance = dbConnection.Execute(updateQuery, new
        {
          meeting.Name,
          meeting.Date,
          meeting.UpdatedDate,
          meeting.Time,
          meeting.Duration,
          meeting.Location,
          meeting.IsReacurance,
          meeting.IsPrivate,
          meeting.ReacuranceType,
          meeting.IsLocked,
          meeting.Status,
          meeting.IsFormal,
          meeting.TimeZone,
          meeting.Tag,
          meeting.Purpose,
          meeting.MeetingOwnerId,
          meeting.Outcome,
          meeting.Id
        });
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
    public bool Delete
      (Guid id, string schema, string connectionString)
    {
      if (id == Guid.NewGuid() || string.IsNullOrEmpty(schema) || string.IsNullOrEmpty(connectionString))
        throw new ArgumentException("Please provide a valid meeting attendee identifier, schema or connection string.");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var sql = $"delete from [{schema}].[Meeting] WHERE Id = '{id.ToString()}'";
        var instance = dbConnection.Execute(sql);
        return instance == 1;
      }
    }

    public (bool condition, string message, MinutesViewModel minutes) GetMeetingMinutes
      (string schema, string connectionString,string meetingId)
    {
      if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(schema))
        throw new ArgumentException("Please provide a valid schema or connection string.");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        MinutesViewModel data;
        try
        {
          dbConnection.Open();
          var sql = $"select * from [{schema}].[Meeting] WHERE Id = '{meetingId}'";
          data = dbConnection.Query<MinutesViewModel>(sql).ToList().FirstOrDefault();
          if (data != null) data.FileName = $"{data.IssuedDate}.pdf";
        }
        catch (Exception e)
        {
          return (false, e.InnerException.Message, null);
        }
        return (data != null, data != null? "Success": "Can't find the meeting that you are looking for", data);
      }
    }
    
    public (bool condition, string message) CreateUpdateMeetingMinutes
      (string schema, string connectionString,string meetingId, byte[] fileData)
    {
      var meeting = new Meeting{Id = Guid.Parse(meetingId), IssuedMinutes = fileData, IssuedDate = DateTime.UtcNow};
      
      if (Guid.Parse(meetingId) == Guid.NewGuid() || string.IsNullOrEmpty(schema) || string.IsNullOrEmpty(connectionString))
        throw new ArgumentException("Please provide a valid meeting attendee identifier, schema or connection string.");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        string updateQuery = $@"UPDATE [{schema}].[Meeting] 
                             SET IssuedMinutes = @IssuedMinutes, 
                                 IssuedDate = @IssuedDate
                             WHERE Id   = @Id";
        var instance = dbConnection.Execute(updateQuery, new
        {
          meeting.IssuedDate,
          meeting.IssuedMinutes,
          meeting.Id
        });
        return (instance == 1,instance == 1? "Successfull":"There was a issue saving the meeting minutes.");
      }
    }
    
  }
}
