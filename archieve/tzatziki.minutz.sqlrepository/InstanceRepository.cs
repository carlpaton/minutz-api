using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using tzatziki.minutz.interfaces.Repositories;
using tzatziki.minutz.models.Auth;
using tzatziki.minutz.models.Entities;

namespace tzatziki.minutz.sqlrepository
{
  public class InstanceRepository : IInstanceRepository
  {
    public IEnumerable<Instance> GetInstances(string connectionString)
    {
      using (var context = new DBConnectorContext(connectionString, "app"))
      {
        context.Database.EnsureCreated();
        return context.Instance.ToList();
      }
    }

    public Instance CreateInstance(string connectionString, UserProfile userprofile)
    {
      using (var context = new DBConnectorContext(connectionString, "app"))
      {
        context.Database.EnsureCreated();
        var insertObject = new Instance
        {
          Active = true,
          Name = userprofile.InstanceId.ToSchemaString(),
          Password = StringExtentions.GeneratePassword(),
          Type = 1,
          Username = userprofile.EmailAddress.ToUserNameString()
        };
        context.Instance.Add(insertObject);
        CreateInstanceTables(connectionString, insertObject);

        try
        {
          context.SaveChanges();
          return insertObject;
        }
        catch (Exception ex)
        {
          throw (ex);
        }
      }
    }

    private static void CreateInstanceTables(string connectionString, Instance insertObject)
    {
      using (SqlConnection con = new SqlConnection(connectionString))
      {
        con.Open();
        using (SqlCommand command = new SqlCommand($"exec app.createInstanceSchema @tenant=account_{insertObject.Name}", con))
        {
          command.ExecuteNonQuery();
        }
      }

      using (SqlConnection con = new SqlConnection(connectionString))
      {
        con.Open();
        using (SqlCommand command = new SqlCommand($"exec app.createInstanceUser @tenant=account_{insertObject.Name}", con))
        {
          command.ExecuteNonQuery();
        }
      }
    }
  }
}