using System.Collections.Generic;
using Models.Entities;

namespace Interface.Repositories
{
	public interface IInstanceRepository
	{
		IEnumerable<Instance> GetAll(string schema, string connectionString);
	  Instance GetByUsername(string username, string schema, string connectionString);
	}
}
