using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Interface.Repositories;
using Minutz.Models.Entities;

namespace SqlRepository
{
	public class NotificationTypeRepository :INotificationTypeRepository
	{
		private const string _notificationType = "[notificationType]";
		private const string _instance = "[instance]";
		
		/// <summary>
		/// Get all notification types from the database
		/// </summary>
		/// <param name="schema">default app schema</param>
		/// <param name="connectionString">default connection string</param>
		/// <returns>collection of notification types</returns>
		public List<NotificationType> GetList(
			string schema,
			string connectionString)
		{
			using (IDbConnection dbConnection = new SqlConnection(connectionString))
			{
				dbConnection.Open();
				var sql = $"select * from [{schema}].{_notificationType}";
				var data = dbConnection.Query<NotificationType>(sql).ToList();
				return data;
			}
		}

		/// <summary>
		/// get the notification type for a schema
		/// </summary>
		/// <param name="schema">user schema instance username</param>
		/// <param name="connectionString">user connection string</param>
		/// <returns>notification type object</returns>
		public NotificationType GetNotificationType(
			string schema,
			string connectionString)
		{
			using (IDbConnection dbConnection = new SqlConnection(connectionString))
			{
				dbConnection.Open();

				var instanceSql = $"select * from [{schema}].{_instance}";
				var instance = dbConnection.Query<Instance>(instanceSql).ToList().FirstOrDefault();
				if (instance == null) return null;
				if (instance.NotificationTypeId == 0) return null;
				dbConnection.Open();
				var notificationTypeSql = $"select * from [{schema}].{_notificationType}";
				var notificationType = dbConnection.Query<NotificationType>(notificationTypeSql).ToList().First(i=> i.Id == instance.NotificationTypeId);
				return notificationType;
			}
		}

		/// <summary>
		/// Set the Notification type for a schema 
		/// </summary>
		/// <param name="schema">default app schema</param>
		/// <param name="connectionString">default connection string</param>
		/// <param name="notificationTypeid">notification type id</param>
		/// <param name="instanceId">instance username as schema</param>
		/// <returns>notification type object</returns>
		public NotificationType SetNotificationTypeForSchema(
			string schema,
			string connectionString,
			int notificationTypeid,
			string instanceId)
		{
			using (IDbConnection dbConnection = new SqlConnection(connectionString))
			{
				dbConnection.Open();
				var notificationTypeSql = $"select * from [{schema}].{_notificationType}";
				var notificationType = dbConnection.Query<NotificationType>(notificationTypeSql).ToList().First(i=> i.Id == notificationTypeid);

				var updateUserSql = $"UPDATE [{schema}].{_instance} SET notificationTypeId = '{notificationTypeid}' WHERE Username = '{instanceId}' ";
				var updateUserResult = dbConnection.Execute(updateUserSql);
				
				return notificationType;
			}
		}
	}
}