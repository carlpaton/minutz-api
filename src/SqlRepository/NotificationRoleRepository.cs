using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Interface.Repositories;
using Minutz.Models.Entities;

namespace SqlRepository
{
	public class NotificationRoleRepository : INotificationRoleRepository
	{
		private const string _notificationRole = "[notificationRole]";
		private const string _instance = "[instance]";
		
		/// <summary>
		/// Get all notification roles in the database
		/// </summary>
		/// <param name="schema">app default schema</param>
		/// <param name="connectionString">default connectionString</param>
		/// <returns>collection of notification roles</returns>
		public List<NotificationRole> GetList(
			string schema,
			string connectionString)
		{
			using (IDbConnection dbConnection = new SqlConnection(connectionString))
			{
				dbConnection.Open();
				var sql = $"select * from [{schema}].{_notificationRole}";
				var data = dbConnection.Query<NotificationRole>(sql).ToList();
				return data;
			}
		}
		
		/// <summary>
		/// Get the notification role for the current schema
		/// </summary>
		/// <param name="schema">instance username which is the schema</param>
		/// <param name="connectionString">user connection string</param>
		/// <returns>notification role object</returns>
		public NotificationRole GetNotificationRole(
			string schema,
			string connectionString)
		{
			using (IDbConnection dbConnection = new SqlConnection(connectionString))
			{
				dbConnection.Open();

				var instanceSql = $"select * from [{schema}].{_instance}";
				var instance = dbConnection.Query<Instance>(instanceSql).ToList().FirstOrDefault();
				if (instance == null) return null;
				if (instance.NotificationRoleId == 0) return null;
				dbConnection.Open();
				var notificationRoleSql = $"select * from [{schema}].{_notificationRole}";
				var notificationRole = dbConnection.Query<NotificationRole>(notificationRoleSql).ToList().First(i=> i.Id == instance.NotificationRoleId);
				return notificationRole;
			}
		}
		
		/// <summary>
		/// Set the notificationRole for a schema
		/// </summary>
		/// <param name="schema">default app schema</param>
		/// <param name="connectionString">default connection string</param>
		/// <param name="notificationRoleId">notification role id</param>
		/// <param name="instanceId">instance username used as schema</param>
		/// <returns>notification role object</returns>
		public NotificationRole SetNotificationRoleForSchema(
			string schema,
			string connectionString,
			int notificationRoleId,
			string instanceId)
		{
			using (IDbConnection dbConnection = new SqlConnection(connectionString))
			{
				dbConnection.Open();
				var notificationRoleSql = $"select * from [{schema}].{_notificationRole}";
				var notificationRole = dbConnection.Query<NotificationRole>(notificationRoleSql).ToList().First(i=> i.Id == notificationRoleId);

				var updateUserSql = $"UPDATE [{schema}].{_instance} SET notificationRoleId = {notificationRoleId} WHERE Username = '{instanceId}' ";
				var updateUserResult = dbConnection.Execute(updateUserSql);
				
				return notificationRole;
			}
		}
		
	}
}