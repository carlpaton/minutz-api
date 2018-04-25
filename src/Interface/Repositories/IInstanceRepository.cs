using System.Collections.Generic;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Repositories
{
  public interface IInstanceRepository
  {
    IEnumerable<Instance> GetAll(
      string schema,
      string connectionString);
    
    InstanceResponse GetByUsername
      (string username, string connectionString);

    Instance SetInstanceDetailsForSchema(
      string schema,
      string connectionString,
      Instance instance);
  }
}
