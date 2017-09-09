using System.Collections.Generic;
using Models.Entities;

namespace Interface.Repositories
{
	public interface IInstanceRepository
	{
		IEnumerable<Instance> GetAll(string connectionString);
	}
}
