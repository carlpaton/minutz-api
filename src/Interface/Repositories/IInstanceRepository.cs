using System.Collections.Generic;
using Minutz.Models.Entities;

namespace Interface.Repositories
{
  public interface IInstanceRepository
  {
    IEnumerable<Instance> GetAll(
      string schema,
      string connectionString);
    
    Instance GetByUsername(
      string username,
      string schema,
      string connectionString);

    Instance SetInstanceDetailsForSchema(
      string schema,
      string connectionString,
      Instance instance);
  }
}
