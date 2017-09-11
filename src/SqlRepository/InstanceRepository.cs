using System.Collections.Generic;
using Interface.Repositories;
using System.Data.SqlClient;
using Models.Entities;
using System.Data;
using Dapper;

namespace SqlRepository
{
  public class InstanceRepository : IInstanceRepository
  {
    public IEnumerable<Instance> GetAll(string connectionString)
    {
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        dbConnection.Open();
        var data = dbConnection.Query<Instance>($"select * from [app].[Instance]");
        return data;
      }
    }
  }
}