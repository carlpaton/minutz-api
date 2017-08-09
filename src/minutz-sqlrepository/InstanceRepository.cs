using minutz_interface.Repositories;
using System.Collections.Generic;
using minutz_interface.Entities;
using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace minutz_sqlrepository
{
	public class InstanceRepository : IInstanceRepository
	{
		public IEnumerable<IInstance> GetAll(string connectionString)
		{
			using (IDbConnection dbConnection = new SqlConnection(connectionString))
			{
				dbConnection.Open();
				return dbConnection.Query<IInstance>($"SELECT * FROM instance");
			}
		}
	}
}
