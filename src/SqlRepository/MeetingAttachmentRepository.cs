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
  public class MeetingAttachmentRepository : IMeetingAttachmentRepository
  {
    public MeetingAttachment Get(Guid id, string schema, string connectionString)
    {
      if (id == Guid.NewGuid() || string.IsNullOrEmpty(schema) || string.IsNullOrEmpty(connectionString))
        throw new ArgumentException("Please provide a valid meeting agenda identifier, schema or connection string.");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var sql = $"select * from [{schema}].[MeetingAttachment] WHERE Id = '{id.ToString()}'";
        var data = dbConnection.Query<MeetingAttachment>(sql).FirstOrDefault();
        return data;
      }
    }

    public List<MeetingAttachment> GetMeetingAttachments(Guid referenceId, string schema, string connectionString)
    {
      if (referenceId == Guid.NewGuid() || string.IsNullOrEmpty(schema) || string.IsNullOrEmpty(connectionString))
        throw new ArgumentException("Please provide a valid meeting agenda identifier, schema or connection string.");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var sql = $"select * from [{schema}].[MeetingAttachment] WHERE ReferanceId = '{referenceId.ToString()}'";
        var data = dbConnection.Query<MeetingAttachment>(sql);
        return data.ToList();
      }
    }
    
    public IEnumerable<MeetingAttachment> List(string schema, string connectionString)
    {
      if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(schema))
        throw new ArgumentException("Please provide a valid schema or connection string.");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var sql = $"select [Id], [ReferanceId], [FileName], [MeetingAttendeeId], [Date], [FileData] from [{schema}].[MeetingAttachment]";
        var data = dbConnection.Query<MeetingAttachment>(sql).ToList();
        return data;
      }
    }
    
    public bool Add
      (MeetingAttachment attachment, string schema, string connectionString)
    {
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        try
        {
          dbConnection.Open();
          string insertSql = $@"insert into [{schema}].[MeetingAttachment](
                                                                 [Id]
                                                                ,[ReferanceId]
                                                                ,[FileName]
                                                                ,[MeetingAttendeeId]
                                                                ,[Date]
                                                                ,[FileData]
                                                                ) 
                                                         values(
                                                                 @Id
                                                                ,@ReferanceId
                                                                ,@FileName
                                                                ,@MeetingAttendeeId
                                                                ,@Date
                                                                ,@FileData
                                                                )";
          var instance = dbConnection.Execute(insertSql, new
          {
            attachment.Id,
            attachment.ReferanceId,
            attachment.FileName,
            attachment.MeetingAttendeeId,
            attachment.Date,
            attachment.FileData
          });
          return instance == 1;
        }
        catch (Exception e)
        {
          return false;
        }
        
      }
    }
    
    public bool Update
      (MeetingAttachment attachment, string schema, string connectionString)
    {
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        try
        {
          dbConnection.Open();
          string updateQuery = $@"UPDATE [{schema}].[MeetingAttachment] 
                             SET ReferanceId = @ReferanceId, 
                                 FileName = @FileName, 
                                 MeetingAttendeeId = @MeetingAttendeeId, 
                                 Date = @Date
                             WHERE Id = @Id";
          var instance = dbConnection.Execute(updateQuery, new
          {
            attachment.Id,
            attachment.ReferanceId,
            attachment.FileName,
            attachment.MeetingAttendeeId,
            attachment.Date
          });
          return instance == 1;
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
          return false;
        }
        
      }
    }
    
    public bool DeleteMeetingAcchments(Guid referenceId, string schema, string connectionString)
    {
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var sql = $"DELETE from [{schema}].[MeetingAttachment] WHERE ReferanceId = '{referenceId.ToString()}'";
        var instance = dbConnection.Execute(sql);
        return instance == 1;
      }
    }
    
    public bool Delete(Guid attachmentId, string schema, string connectionString)
    {
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var sql = $"DELETE from [{schema}].[MeetingAttachment] WHERE Id = '{attachmentId.ToString()}'";
        var instance = dbConnection.Execute(sql);
        return instance == 1;
      }
    }
  }
}
