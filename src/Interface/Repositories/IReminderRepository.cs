using System.Collections.Generic;
using Minutz.Models.Entities;

namespace Interface.Repositories
{
	public interface IReminderRepository
	{
		List<Reminder> GetList(
			string schema,
			string connectionString);
		
		Reminder GetReminder(
			string schema,
			string connectionString);

		Reminder SetReminderForSchema(
			string schema,
			string connectionString,
			int reminderId,
			string instanceId);
	}
}