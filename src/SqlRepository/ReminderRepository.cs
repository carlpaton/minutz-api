using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Interface.Repositories;
using Minutz.Models.Entities;

namespace SqlRepository
{
	public class ReminderRepository : IReminderRepository
	{
		private const string _reminder = "[reminder]";
		private const string _instance = "[instance]";

		/// <summary>
		/// Get all static Reminder Types from the database
		/// </summary>
		/// <param name="schema">app schema</param>
		/// <param name="connectionString">the default connection string</param>
		/// <returns>Collection of reminders for the UI</returns>
		public List<Reminder> GetList(
			string schema,
			string connectionString)
		{
			using (IDbConnection dbConnection = new SqlConnection(connectionString))
			{
				dbConnection.Open();
				var sql = $"select * from [{schema}].{_reminder}";
				var data = dbConnection.Query<Reminder>(sql).ToList();
				return data;
			}
		}

		/// <summary>
		/// Get the reminder that was set for the schema
		/// </summary>
		/// <param name="schema">Instance Schema</param>
		/// <param name="connectionString">Schema ConnectionString</param>
		/// <returns>Reminder Object</returns>
		public Reminder GetReminder(
			string schema,
			string connectionString)
		{
			using (IDbConnection dbConnection = new SqlConnection(connectionString))
			{
				dbConnection.Open();

				var instanceSql = $"select * from [{schema}].{_instance}";
				var instance = dbConnection.Query<Instance>(instanceSql).ToList().FirstOrDefault();
				if (instance == null) return null;
				if (instance.ReminderId == 0) return null;
				dbConnection.Open();
				var remindersSql = $"select * from [{schema}].{_reminder}";
				var reminder = dbConnection.Query<Reminder>(remindersSql).ToList().First(i=> i.Id == instance.ReminderId);
				return reminder;
			}
		}

		/// <summary>
		/// Set the reminder for a schema account
		/// </summary>
		/// <param name="schema">app schema</param>
		/// <param name="connectionString">default connectionString</param>
		/// <param name="reminderId">account schema</param>
		/// <returns>Reminder Object</returns>
		public Reminder SetReminderForSchema(
			string schema,
			string connectionString,
			int reminderId,
			string instanceId)
		{
			using (IDbConnection dbConnection = new SqlConnection(connectionString))
			{
				dbConnection.Open();
				var remindersSql = $"select * from [{schema}].{_reminder}";
				var reminder = dbConnection.Query<Reminder>(remindersSql).ToList().First(i=> i.Id == reminderId);

				var updateUserSql = $"UPDATE [{schema}].{_instance} SET reminderId = '{reminderId}' WHERE Username = '{instanceId}' ";
				var updateUserResult = dbConnection.Execute(updateUserSql);
				
				return reminder;
			}
		}
	}
}