using minutz_interface.Entities;
using System.Collections.Generic;

namespace minutz_interface.Repositories
{
	public interface IInstanceRepository
	{
		IEnumerable<IInstance> GetAll(string connectionString);
	}
}
