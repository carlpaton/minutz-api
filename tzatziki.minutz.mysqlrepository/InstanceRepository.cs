
using System.Collections.Generic;
using System.Linq;
using tzatziki.minutz.interfaces.Repositories;
using tzatziki.minutz.models.Entities;

namespace tzatziki.minutz.mysqlrepository
{
	public class InstanceRepository : IInstanceRepository
	{

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
