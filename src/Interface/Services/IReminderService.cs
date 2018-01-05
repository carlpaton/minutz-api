using System.Collections.Generic;
using Minutz.Models.Entities;

namespace Interface.Services
{
	public interface IReminderService
	{
		List<Reminder> GetList(
			string token);

		Reminder GetReminder(
			string token);

		Reminder SetReminderForSchema(
			string token,
			int reminderId);
	}
}