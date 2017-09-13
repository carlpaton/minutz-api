using System.Collections.Generic;
using System.Data.SqlClient;
using Models.Entities;
using System.Data;
using System.Linq;
using System;
using Dapper;

namespace SqlRepository
{
  public class MeetingRespository
  {
    public Meeting Get(Guid id, string schema, string connectionString)
    {
      if (id == Guid.NewGuid() || string.IsNullOrEmpty(schema) || string.IsNullOrEmpty(connectionString))
        throw new ArgumentException("Please provide a valid meeting identifier, schema or connectionstring.");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var sql = $"select * from [{schema}].[Meeting] WHERE Id = '{id.ToString()}'";
        var data = dbConnection.Query<Meeting>(sql).FirstOrDefault();
        return data;
      }
    }
    public IEnumerable<Meeting> List(string schema, string connectionString)
    {
      if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(schema))
        throw new ArgumentException("Please provide a valid schema or connectionstring.");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var sql = $"select * from [{schema}].[Meeting]";
        var data = dbConnection.Query<Meeting>(sql).ToList();
        return data;
      }
    }
  }
}
