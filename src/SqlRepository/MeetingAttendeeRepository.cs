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
  public class MeetingAttendeeRepository: IMeetingAttendeeRepository
  {
    public MeetingAttendee Get(Guid id, string schema, string connectionString)
    {
      if (id == Guid.NewGuid() || string.IsNullOrEmpty(schema) || string.IsNullOrEmpty(connectionString))
        throw new ArgumentException("Please provide a valid meeting attendee identifier, schema or connectionstring.");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var sql = $"select * from [{schema}].[MeetingAttendee] WHERE Id = '{id.ToString()}'";
        var data = dbConnection.Query<MeetingAttendee>(sql).FirstOrDefault();
        return data;
      }
    }
    public IEnumerable<MeetingAttendee> List(string schema, string connectionString)
    {
      if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(schema))
        throw new ArgumentException("Please provide a valid schema or connectionstring.");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var sql = $"select * from [{schema}].[MeetingAttendee]";
        var data = dbConnection.Query<MeetingAttendee>(sql).ToList();
        return data;
      }
    }
    public bool Add(MeetingAttendee action, string schema, string connectionString)
    {
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        string insertSql = $@"insert into [{schema}].[MeetingAttendee](
                                                                 [Id]
                                                                ,[ReferanceId]
                                                                ,[PersonIdentity]
                                                                ,[Role]
                                                                ) 
                                                         values(
                                                                 @Id
                                                                ,@ReferanceId
                                                                ,@PersonIdentity
                                                                ,@Role
                                                                )";
        var instance = dbConnection.Execute(insertSql, new
        {
          action.Id,
          action.ReferanceId,
          action.PersonIdentity,
          action.Role
        });
        return instance == 1;
      }
    }
    public bool Update(MeetingAttendee action, string schema, string connectionString)
    {
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        string updateQuery = $@"UPDATE [{schema}].[MeetingAttendee] 
                             SET ReferanceId = @ReferanceId, 
                                 PersonIdentity = @PersonIdentity, 
                                 Role = @Role
                             WHERE Id = @Id";
        var instance = dbConnection.Execute(updateQuery, new
        {
          action.ReferanceId,
          action.PersonIdentity,
          action.Role
        });
        return instance == 1;
      }
    }
  }
}
