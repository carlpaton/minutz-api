using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Interface.Repositories;
using Minutz.Models.Entities;

namespace SqlRepository
{
  public class MeetingDecisionRepository: IDecisionRepository
  {
    public List<MinutzDecision> GetMeetingDecisions
      (Guid referenceId, string schema, string connectionString)
    {
      if (referenceId == Guid.NewGuid() || string.IsNullOrEmpty(schema) || string.IsNullOrEmpty(connectionString))
        throw new ArgumentException("Please provide a valid meeting identifier, schema or connection string.");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var sql = $"select * from [{schema}].[MinutzDecision] WHERE ReferanceId = '{referenceId.ToString()}'";
        var data = dbConnection.Query<MinutzDecision>(sql).ToList();
        return data;
      }
    }

    public MinutzDecision Get
      (Guid id, string schema, string connectionString)
    {
      if (id == Guid.NewGuid() || string.IsNullOrEmpty(schema) || string.IsNullOrEmpty(connectionString))
        throw new ArgumentException("Please provide a valid meeting identifier, schema or connection string.");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var sql = $"select * from [{schema}].[MinutzDecision] WHERE Id = '{id.ToString()}'";
        var data = dbConnection.Query<MinutzDecision>(sql).FirstOrDefault();
        return data;
      }
    }
    
    public bool Add
      (MinutzDecision decision, string schema, string connectionString)
    {
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        string insertSql = $@"insert into [{schema}].[MinutzDecision](
                                                                 [Id]
                                                                ,[ReferanceId]
                                                                ,[DecisionText]
                                                                ,[DecisionComment]
                                                                ,[AgendaId]
                                                                ,[PersonId]
                                                                ,[CreatedDate]
                                                                ,[IsOverturned]
                                                                ) 
                                                         values(
                                                                 @Id
                                                                ,@ReferanceId
                                                                ,@DecisionText
                                                                ,@DecisionComment
                                                                ,@AgendaId
                                                                ,@PersonId
                                                                ,@CreatedDate
                                                                ,@IsOverturned)";
        var instance = dbConnection.Execute(insertSql, new
        {
          decision.Id,
          decision.ReferenceId,
          decision.DescisionText,
          decision.Descisioncomment,
          decision.AgendaId,
          decision.PersonId,
          decision.CreatedDate,
          decision.IsOverturned
        });
        return instance == 1;
      }
    }
    
    public bool Update
      (MinutzDecision decision, string schema, string connectionString)
    {
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        string updateQuery = $@"UPDATE [{schema}].[MinutzDecision]
                             SET 
                                [ReferanceId] = @ReferanceId,
                                [DecisionText] = @DecisionText,
                                [DecisionComment] = @DecisionComment,
                                [AgendaId] = @AgendaId,
                                [PersonId] = @PersonId,
                                [CreatedDate] = @CreatedDate,
                                [IsOverturned] = @IsOverturned
                             WHERE Id = @Id";
        var instance = dbConnection.Execute(updateQuery, new
        {
          decision.ReferenceId,
          decision.DescisionText,
          decision.Descisioncomment,
          decision.AgendaId,
          decision.PersonId,
          decision.CreatedDate,
          decision.IsOverturned
        });
        return instance == 1;
      }
    }
    
    public bool Delete
      (Guid id, string schema, string connectionString)
    {
      if (id == Guid.NewGuid() || string.IsNullOrEmpty(schema) || string.IsNullOrEmpty(connectionString))
        throw new ArgumentException("Please provide a valid meeting attendee identifier, schema or connection string.");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var sql = $"delete from [{schema}].[MinutzDecision] WHERE Id = '{id.ToString()}'";
        var instance = dbConnection.Execute(sql);
        return instance == 1;
      }
    }
  }
}