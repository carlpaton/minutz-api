using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Interface.Repositories;
using Minutz.Models.Entities;

namespace SqlRepository
{
  public class InstanceRepository : IInstanceRepository
  {

    private const string _instance = "[instance]";

    public IEnumerable<Instance> GetAll (
      string schema, string connectionString)
    {
      using (IDbConnection dbConnection = new SqlConnection (connectionString))
      {
        dbConnection.Open ();
        var sql = $"select * from [{schema}].{_instance}";
        var data = dbConnection.Query<Instance> (sql);
        return data;
      }
    }

    /// <summary>
    /// This gets the instance by the instance username that is in the person table for a user
    /// </summary>
    /// <param name="username">instance username used as the schema</param>
    /// <param name="schema">default app schema</param>
    /// <param name="connectionString">default connection string</param>
    /// <returns></returns>
    public Instance GetByUsername (
      string username, string schema, string connectionString)
    {
      using (IDbConnection dbConnection = new SqlConnection (connectionString))
      {
        dbConnection.Open ();
        var sql = $"select * from [{schema}].{_instance} WHERE Username = '{username}'";
        var data = dbConnection.Query<Instance> (sql).FirstOrDefault ();
        return data;
      }
    }

    public Instance SetInstanceDetailsForSchema (
      string schema, string connectionString, Instance instance)
    {
      using (IDbConnection dbConnection = new SqlConnection (connectionString))
      {
        dbConnection.Open ();
        if (string.IsNullOrEmpty (instance.Colour)) instance.Colour = "default";
        if (string.IsNullOrEmpty (instance.Style)) instance.Style = "default";
        var allowInformal = instance.AllowInformal == true ? 1 : 0;

        var updateUserSql = $@"UPDATE [{schema}].{_instance} SET
                               colour = '{instance.Colour}' ,
                               style = '{instance.Style}'
                               allowInformal = {allowInformal},
                               WHERE Id = '{instance.Id}' ";
        var updateUserResult = dbConnection.Execute (updateUserSql);

        var instanceSql = $"select * from [{schema}].{_instance} WHERE Id = '{instance.Id}' ";
        var instanceResult = dbConnection.Query<Instance> (instanceSql).ToList ().FirstOrDefault ();

        return instanceResult;
      }
    }
  }
}