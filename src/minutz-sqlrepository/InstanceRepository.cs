using minutz_interface.Repositories;
using System.Collections.Generic;
using minutz_interface.Entities;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using minutz_models.Entities;

namespace minutz_sqlrepository
{
	public class InstanceRepository : IInstanceRepository
	{
		public IEnumerable<IInstance> GetAll(string connectionString)
		{
			using (IDbConnection dbConnection = new SqlConnection(connectionString))
			{
				dbConnection.Open();
				var data = dbConnection.Query<Instance>($"select * from [app].[Instance]");
				return data;
			}
		}
	}
}
