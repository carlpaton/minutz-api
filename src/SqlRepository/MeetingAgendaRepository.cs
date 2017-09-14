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
        throw new ArgumentException("Please provide a valid meeting identifier, schema or connectionstring.");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var sql = $"select * from [{schema}].[MeetingAgenda] WHERE Id = '{id.ToString()}'";
        var data = dbConnection.Query<MeetingAgenda>(sql).FirstOrDefault();
        return data;
      }
    }
    public IEnumerable<MeetingAgenda> List(string schema, string connectionString)
    {
      if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(schema))
        throw new ArgumentException("Please provide a valid schema or connectionstring.");
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
                                                                ,[ReferanceId]
                                                                ,[AgendaHeading]
                                                                ,[AgendaText]
                                                                ,[MeetingAttendeeId]
                                                                ,[Duration]
                                                                ,[CreatedDate]
                                                                ,[IsComplete]
                                                                ) 
                                                         values(
                                                                 @Id
                                                                ,@ReferanceId
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
          action.ReferanceId,
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
        string updateQuery = $@"UPDATE [{schema}].[Meeting] 
                             SET ReferanceId = @ReferanceId, 
                                 AgendaHeading = @AgendaHeading, 
                                 AgendaText = @AgendaText, 
                                 MeetingAttendeeId = @MeetingAttendeeId,
                                 Duration = @Duration, 
                                 CreatedDate = @CreatedDate, 
                                 IsComplete = @IsComplete
                             WHERE Id = @Id";
        var instance = dbConnection.Execute(updateQuery, new
        {
          action.ReferanceId,
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
