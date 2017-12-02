using System.Collections.Generic;
using System.Data.SqlClient;
using Models.Entities;
using System.Linq;
using System.Data;
using Dapper;
using System;
using Interface.Repositories;

namespace SqlRepository
{
  public class MeetingNoteRepository : IMeetingNoteRepository
  {
    public MeetingNote Get(Guid id, string schema, string connectionString)
    {
      if (id == Guid.NewGuid() || string.IsNullOrEmpty(schema) || string.IsNullOrEmpty(connectionString))
        throw new ArgumentException("Please provide a valid meeting attendee identifier, schema or connection string.");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var sql = $"select * from [{schema}].[MeetingNote] WHERE Id = '{id.ToString()}'";
        var data = dbConnection.Query<MeetingNote>(sql).FirstOrDefault();
        return data;
      }
    }
    public List<MeetingNote> GetMeetingNotes(Guid referenceId, string schema, string connectionString)
    {
      if (referenceId == Guid.NewGuid() || string.IsNullOrEmpty(schema) || string.IsNullOrEmpty(connectionString))
        throw new ArgumentException("Please provide a valid meeting attendee identifier, schema or connection string.");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var sql = $"select * from [{schema}].[MeetingNote] WHERE ReferenceId = '{referenceId.ToString()}'";
        var data = dbConnection.Query<MeetingNote>(sql);
        return data.ToList();
      }
    }
    public IEnumerable<MeetingNote> List(string schema, string connectionString)
    {
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var sql = $"select * from [{schema}].[MeetingNote]";
        var data = dbConnection.Query<MeetingNote>(sql);
        return data.ToList();
      }
    }
    public bool Add(MeetingNote note, string schema, string connectionString)
    {
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        string insertSql = $@"insert into [{schema}].[MeetingNote](
                                                                 [Id]
                                                                ,[ReferanceId]
                                                                ,[NoteText]
                                                                ,[MeetingAttendeeId]
                                                                ,[CreatedDate]
                                                                ) 
                                                         values(
                                                                 @Id
                                                                ,@ReferanceId
                                                                ,@NoteText
                                                                ,@MeetingAttendeeId
                                                                ,@CreatedDate
                                                                )";
        var instance = dbConnection.Execute(insertSql, new
        {
          note.Id,
          note.ReferanceId,
          note.NoteText,
          note.MeetingAttendeeId,
          note.CreatedDate
        });
        return instance == 1;
      }
    }
    public bool Update(MeetingNote note, string schema, string connectionString)
    {
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        string updateQuery = $@"UPDATE [{schema}].[MeetingNote] 
                             SET ReferanceId = @ReferanceId, 
                                 NoteText = @NoteText, 
                                 MeetingAttendeeId = @MeetingAttendeeId
                                 CreatedDate = @CreatedDate
                             WHERE Id = @Id";
        var instance = dbConnection.Execute(updateQuery, new
        {
          note.Id,
          note.ReferanceId,
          note.NoteText,
          note.MeetingAttendeeId,
          note.CreatedDate
        });
        return instance == 1;
      }
    }
    public bool DeleteMeetingNotes(Guid referenceId, string schema, string connectionString)
    {
      if (referenceId == Guid.NewGuid() || string.IsNullOrEmpty(schema) || string.IsNullOrEmpty(connectionString))
        throw new ArgumentException("Please provide a valid meeting attendee identifier, schema or connection string.");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var sql = $"delete from [{schema}].[MeetingNote] WHERE ReferanceId = '{referenceId.ToString()}'";
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
        var sql = $"delete from [{schema}].[MeetingNote] WHERE Id = '{id.ToString()}'";
        var instance = dbConnection.Execute(sql);
        return instance == 1;
      }
    }
  }
}
