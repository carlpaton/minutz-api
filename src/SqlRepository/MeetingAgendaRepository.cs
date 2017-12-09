using System.Collections.Generic;
using Interface.Repositories;
using System.Data.SqlClient;
using Minutz.Models.Entities;
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
        var sql = $"select * from [{schema}].[MeetingAgenda] WHERE ReferanceId = '{referenceId.ToString()}'";
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
    public bool Add(MeetingAgenda agendaItem, string schema, string connectionString)
    {
      if(string.IsNullOrEmpty(agendaItem.Id.ToString()))
      {
        agendaItem.Id = Guid.NewGuid();
      }
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
                                                                ,@ReferenceId
                                                                ,@AgendaHeading
                                                                ,@AgendaText
                                                                ,@MeetingAttendeeId
                                                                ,@Duration
                                                                ,@CreatedDate
                                                                ,@IsComplete
                                                                )";
        var instance = dbConnection.Execute(insertSql, new
        {
          agendaItem.Id,
          agendaItem.ReferenceId,
          agendaItem.AgendaHeading,
          agendaItem.AgendaText,
          agendaItem.MeetingAttendeeId,
          agendaItem.Duration,
          agendaItem.CreatedDate,
          agendaItem.IsComplete
        });
        return instance == 1;
      }
    }
    public bool Update(MeetingAgenda agendaItem, string schema, string connectionString)
    {
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        string updateQuery = $@"UPDATE [{schema}].[MeetingAgenda] 
                             SET ReferanceId = @ReferenceId, 
                                 AgendaHeading = @AgendaHeading, 
                                 AgendaText = @AgendaText, 
                                 MeetingAttendeeId = @MeetingAttendeeId,
                                 Duration = @Duration, 
                                 CreatedDate = @CreatedDate, 
                                 IsComplete = @IsComplete
                             WHERE Id = @Id";
        var instance = dbConnection.Execute(updateQuery, new
        {
          agendaItem.ReferenceId,
          agendaItem.AgendaHeading,
          agendaItem.AgendaText,
          agendaItem.MeetingAttendeeId,
          agendaItem.Duration,
          agendaItem.CreatedDate,
          agendaItem.IsComplete,
          agendaItem.Id
        });
        return instance == 1;
      }
    }
    public bool DeleteMeetingAgenda(Guid referenceId, string schema, string connectionString)
    {
      if (referenceId == Guid.NewGuid() || string.IsNullOrEmpty(schema) || string.IsNullOrEmpty(connectionString))
        throw new ArgumentException("Please provide a valid meeting attendee identifier, schema or connection string.");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var sql = $"delete from [{schema}].[MeetingAgenda] WHERE ReferanceId = '{referenceId.ToString()}'";
        var instance = dbConnection.Execute(sql);
        return instance == 1;
      }
    }
    public bool Delete(Guid id, string schema, string connectionString)
    {
      if (id == Guid.NewGuid() || string.IsNullOrEmpty(schema) || string.IsNullOrEmpty(connectionString))
        throw new ArgumentException("Please provide a valid meeting attendee identifier, schema or connection string.");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var sql = $"delete from [{schema}].[MeetingAgenda] WHERE Id = '{id.ToString()}'";
        var instance = dbConnection.Execute(sql);
        return instance == 1;
      }
    }
  }
}
