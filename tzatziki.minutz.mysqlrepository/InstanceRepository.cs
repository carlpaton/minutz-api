using System;
using System.Collections.Generic;
using System.Linq;
using tzatziki.minutz.interfaces.Repositories;
using tzatziki.minutz.models.Auth;
using tzatziki.minutz.models.Entities;

namespace tzatziki.minutz.mysqlrepository
{
  public class InstanceRepository : IInstanceRepository
  {
    public Instance CreateInstance(string connectionString, UserProfile userprofile)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<Instance> GetInstances(string connectionString)
    {
      using (var context = new DBConnectorContext(connectionString))
      {
        context.Database.EnsureCreated();
        return context.instance.ToList();
      }
    }
  }
}