using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Interface.Repositories;
using Minutz.Models.Entities;
using Minutz.Models.Message;

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

    public InstanceResponse GetByUsername 
      (string username, string connectionString)
    {
      var result = new InstanceResponse
      {
        Condition = false, Message = string.Empty, Instances = new List<Instance>(), Instance =  new Instance()
      };
      try
      {
        using (IDbConnection dbConnection = new SqlConnection (connectionString))
        {
          dbConnection.Open ();
          var sql = $"SELECT * FROM [app].[Instance] WHERE [Username] = '{username}'";
          var data = dbConnection.Query<Instance>(sql);
          var enumerable = data.ToList();
          if (!enumerable.Any())
          {
            result.Message = "No instances found.";
            return result;
          }

          result.Condition = true;
          result.Instance = enumerable.FirstOrDefault();
          result.Instances.AddRange(enumerable);
          return result;
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        result.Message = e.InnerException.Message;
        return result;
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