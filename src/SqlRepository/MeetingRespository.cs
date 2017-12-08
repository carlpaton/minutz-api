using System.Collections.Generic;
using Interface.Repositories;
using System.Data.SqlClient;
using Minutz.Models.Entities;
using System.Data;
using System.Linq;
using System;
using Dapper;

namespace SqlRepository
{
  public class MeetingRepository : IMeetingRepository
  {
    public Meeting Get(Guid id, string schema, string connectionString)
    {
      if (id == Guid.NewGuid() || string.IsNullOrEmpty(schema) || string.IsNullOrEmpty(connectionString))
        throw new ArgumentException("Please provide a valid meeting identifier, schema or connection string.");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var sql = $"select * from [{schema}].[MeetingViewModel] WHERE Id = '{id.ToString()}'";
        var data = dbConnection.Query<Meeting>(sql).FirstOrDefault();
        return data;
      }
    }
    public List<Meeting> List(string schema, string connectionString)
    {
      if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(schema))
        throw new ArgumentException("Please provide a valid schema or connection string.");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var sql = $"select * from [{schema}].[MeetingViewModel]";
        var data = dbConnection.Query<Meeting>(sql).ToList();
        return data;
      }
    }
    public bool Add(Meeting action, string schema, string connectionString)
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
    public bool Update(Meeting action, string schema, string connectionString)
    {
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        string updateQuery = $@"UPDATE [{schema}].[MeetingViewModel] 
                             SET Name = @Name, 
                                 Date = @Date, 
                                 UpdatedDate = @UpdatedDate, 
                                 Time = @Time,
                                 Duration = @Duration, 
                                 IsReacurance = @IsReacurance, 
                                 IsPrivate = @IsPrivate,
                                 ReacuranceType = @ReacuranceType, 
                                 IsLocked = @IsLocked, 
                                 IsFormal = @IsFormal,
                                 PersonId = @PersonId, 
                                 DueDate = @DueDate, 
                                 IsComplete = @IsComplete
                             WHERE Id = @Id";
        var instance = dbConnection.Execute(updateQuery, new
        {
          action.Name,
          action.Date,
          action.UpdatedDate,
          action.Time,
          action.Duration,
          action.IsReacurance,
          action.IsPrivate,
          action.ReacuranceType,
          action.IsLocked,
          action.IsFormal,
          action.TimeZone,
          action.Tag,
          action.Purpose,
          action.MeetingOwnerId,
          action.Outcome
        });
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
        var sql = $"delete from [{schema}].[MeetingViewModel] WHERE Id = '{id.ToString()}'";
        var instance = dbConnection.Execute(sql);
        return instance == 1;
      }
    }
  }
}
