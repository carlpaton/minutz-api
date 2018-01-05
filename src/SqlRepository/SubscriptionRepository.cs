using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Interface.Repositories;
using Minutz.Models.Entities;

namespace SqlRepository
{
	public class SubscriptionRepository: ISubscriptionRepository
	{
		private const string _subscription = "[subscription]";
		private const string _instance = "[instance]";
		
		public List<Subscription> GetList(
			string schema,
			string connectionString)
		{
			using (IDbConnection dbConnection = new SqlConnection(connectionString))
			{
				dbConnection.Open();
				var sql = $"select * from [{schema}].{_subscription}";
				var data = dbConnection.Query<Subscription>(sql).ToList();
				return data;
			}
		}

		public Subscription GetSubscription(
			string schema,
			string connectionString)
		{
			using (IDbConnection dbConnection = new SqlConnection(connectionString))
			{
				dbConnection.Open();

				var instanceSql = $"select * from [{schema}].{_instance}";
				var instance = dbConnection.Query<Instance>(instanceSql).ToList().FirstOrDefault();
				if (instance == null) return null;
				if (instance.SubscriptionId == 0) return null;
				dbConnection.Open();
				var subscriptionSql = $"select * from [{schema}].{_subscription}";
				var subscription = dbConnection.Query<Subscription>(subscriptionSql).ToList().First(i => i.Id == instance.SubscriptionId);
				return subscription;
			}
		}

		public Subscription SetSubscriptionTypeForSchema(
			string schema,
			string connectionString,
			int subscriptionId,
			string instanceId)
		{
			using (IDbConnection dbConnection = new SqlConnection(connectionString))
			{
				dbConnection.Open();
				var subscriptionSql = $"select * from [{schema}].{_subscription}";
				var subscription = dbConnection.Query<Subscription>(subscriptionSql).ToList().First(i => i.Id == subscriptionId);

				var updateUserSql = $"UPDATE [{schema}].{_instance} SET subscriptionId = {subscriptionId} , subscriptionDate = '{DateTime.UtcNow}' WHERE Username = '{instanceId}' ";
				var updateUserResult = dbConnection.Execute(updateUserSql);
				
				return subscription;
			}
		}
	}
}