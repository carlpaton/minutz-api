using System.Collections.Generic;
using Interface.Repositories;
using System.Data.SqlClient;
using Models.Entities;
using System.Data;
using System.Linq;
using Dapper;

namespace SqlRepository
{
  public class InstanceRepository : IInstanceRepository
  {
    public IEnumerable<Instance> GetAll(string schema, string connectionString)
    {
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var sql = $"select * from [{schema}].[Instance]";
        var data = dbConnection.Query<Instance>(sql);
        return data;
      }
    }
    public Instance GetByUsername(string username,string schema,string connectionString)
    {
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var sql = $"select * from [{schema}].[Instance] WHERE Username = '{username}'";
        var data = dbConnection.Query<Instance>(sql).FirstOrDefault();
        return data;
      }
    }
  }
}