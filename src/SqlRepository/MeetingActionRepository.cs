using System.Collections.Generic;
using Interface.Repositories;
using System.Data.SqlClient;
using Models.Entities;
using System.Data;
using System.Linq;
using System;
using Dapper;

namespace SqlRepository
{
  public class MeetingActionRepository : IMeetingActionRepository
  {
    public MeetingAction Get(Guid id, string schema, string connectionString)
    {
      if (id == Guid.NewGuid() || string.IsNullOrEmpty(schema) || string.IsNullOrEmpty(connectionString))
        throw new ArgumentException("Please provide a valid meeting identifier, schema or connectionstring.");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var sql = $"select * from [{schema}].[MeetingAction] WHERE Id = '{id.ToString()}'";
        var data = dbConnection.Query<MeetingAction>(sql).FirstOrDefault();
        return data;
      }
    }
    public IEnumerable<MeetingAction> List(string schema, string connectionString)
    {
      if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(schema))
        throw new ArgumentException("Please provide a valid schema or connectionstring.");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var sql = $"select * from [{schema}].[MeetingAction]";
        var data = dbConnection.Query<MeetingAction>(sql).ToList();
        return data;
      }
    }
    public bool Add(MeetingAction action,string schema, string connectionString)
    {
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        string insertSql = $@"insert into [{schema}].[MeetingAction](
                                                                 [Id]
                                                                ,[ReferanceId]
                                                                ,[ActionText]
                                                                ,[PersonId]
                                                                ,[DueDate]
                                                                ,[IsComplete]
                                                                ) 
                                                         values(
                                                                 @Id
                                                                ,@ReferanceId
                                                                ,@ActionText
                                                                ,@PersonId
                                                                ,@DueDate
                                                                ,@IsComplete)";
        var instance = dbConnection.Execute(insertSql, new
                                                        {
                                                          action.Id,
                                                          action.ReferanceId,
                                                          action.ActionText,
                                                          action.PersonId,
                                                          action.DueDate,
                                                          action.IsComplete
                                                        });
        return instance == 1;
      }
    }
    public bool Update(MeetingAction action, string schema, string connectionString)
    {
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        string updateQuery = $@"UPDATE [{schema}].[MeetingAction]
                             SET ActionText = @ActionText, 
                                 PersonId = @PersonId, 
                                 DueDate = @DueDate, 
                                 IsComplete = @IsComplete
                             WHERE Id = @Id";
        var instance = dbConnection.Execute(updateQuery, new
        {
          Action = action.ActionText,
          PersonId = action.PersonId.ToString(),
          action.DueDate,
          action.IsComplete
        });
        return instance == 1;
      }
    }
  }
}
