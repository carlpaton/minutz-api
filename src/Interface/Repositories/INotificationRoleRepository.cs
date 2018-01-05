using System.Collections.Generic;
using Minutz.Models.Entities;

namespace Interface.Repositories
{
	public interface INotificationRoleRepository
	{
		List<NotificationRole> GetList(
			string schema,
			string connectionString);

		NotificationRole GetNotificationRole(
			string schema,
			string connectionString);

		NotificationRole SetNotificationRoleForSchema(
			string schema,
			string connectionString,
			int notificationRoleId,
			string instanceId);
	}
}