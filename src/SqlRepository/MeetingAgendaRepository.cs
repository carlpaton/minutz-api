using System.Collections.Generic;
using Interface.Repositories;
using System.Data.SqlClient;
using Models.Entities;
using System.Data;
using System.Linq;
using Dapper;
using System;

namespace SqlRepository
{
  public class MeetingAgendaRepository : IMeetingAgendaRepository
  {
    public MeetingAgenda Get(Guid id, string schema, string connectionString)
    {
      if (id == Guid.NewGuid() || string.IsNullOrEmpty(schema) || string.IsNullOrEmpty(connectionString))
        throw new ArgumentException("Please provide a valid meeting agenda identifier, schema or connection string.");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var sql = $"select * from [{schema}].[MeetingAgenda] WHERE Id = '{id.ToString()}'";
        var data = dbConnection.Query<MeetingAgenda>(sql).FirstOrDefault();
        return data;
      }
    }
    public List<MeetingAgenda> GetMeetingAgenda(Guid referenceId, string schema, string connectionString)
    {
      if (referenceId == Guid.NewGuid() || string.IsNullOrEmpty(schema) || string.IsNullOrEmpty(connectionString))
        throw new ArgumentException("Please provide a valid meeting agenda identifier, schema or connection string.");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var sql = $"select * from [{schema}].[MeetingAgenda] WHERE ReferenceId = '{referenceId.ToString()}'";
        var data = dbConnection.Query<MeetingAgenda>(sql);
        return data.ToList();
      }
    }
    public IEnumerable<MeetingAgenda> List(string schema, string connectionString)
    {
      if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(schema))
        throw new ArgumentException("Please provide a valid schema or connection string.");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var sql = $"select * from [{schema}].[MeetingAgenda]";
        var data = dbConnection.Query<MeetingAgenda>(sql).ToList();
        return data;
      }
    }
    public bool Add(MeetingAgenda action, string schema, string connectionString)
    {
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        string insertSql = $@"insert into [{schema}].[MeetingAgenda](
                                                                 [Id]
                                                                ,[ReferenceId]
                                                                ,[AgendaHeading]
                                                                ,[AgendaText]
                                                                ,[MeetingAttendeeId]
                                                                ,[Duration]
                                                                ,[CreatedDate]
                                                                ,[IsComplete]
                                                                ) 
                                                         values(
                                                                 @Id
                                                                ,@ReferenceId
                                                                ,@AgendaHeading
                                                                ,@AgendaText
                                                                ,@MeetingAttendeeId
                                                                ,@Duration
                                                                 @CreatedDate
                                                                ,@IsComplete
                                                                )";
        var instance = dbConnection.Execute(insertSql, new
        {
          action.Id,
          action.ReferenceId,
          action.AgendaHeading,
          action.AgendaText,
          action.MeetingAttendeeId,
          action.Duration,
          action.CreatedDate,
          action.IsComplete
        });
        return instance == 1;
      }
    }
    public bool Update(MeetingAgenda action, string schema, string connectionString)
    {
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        string updateQuery = $@"UPDATE [{schema}].[MeetingAgenda] 
                             SET ReferenceId = @ReferenceId, 
                                 AgendaHeading = @AgendaHeading, 
                                 AgendaText = @AgendaText, 
                                 MeetingAttendeeId = @MeetingAttendeeId,
                                 Duration = @Duration, 
                                 CreatedDate = @CreatedDate, 
                                 IsComplete = @IsComplete
                             WHERE Id = @Id";
        var instance = dbConnection.Execute(updateQuery, new
        {
          action.ReferenceId,
          action.AgendaHeading,
          action.AgendaText,
          action.MeetingAttendeeId,
          action.Duration,
          action.CreatedDate,
          action.IsComplete
        });
        return instance == 1;
      }
    }
  }
}
