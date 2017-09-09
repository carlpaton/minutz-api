using Interface.Entities;
using System.Collections.Generic;

namespace Interface.Repositories
{
	public interface IInstanceRepository
	{
		IEnumerable<IInstance> GetAll(string connectionString);
	}
}
