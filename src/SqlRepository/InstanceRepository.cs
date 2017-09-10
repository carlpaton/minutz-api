using Interface.Repositories;
using System.Collections.Generic;
using Interface.Entities;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Models.Entities;

namespace SqlRepository
{
	public class InstanceRepository : IInstanceRepository
	{
		public IEnumerable<Instance> GetAll(string connectionString)
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