using System.Collections.Generic;
using Minutz.Models.Entities;

namespace Interface.Repositories
{
	public interface INotificationTypeRepository
	{
		List<NotificationType> GetList(
			string schema,
			string connectionString);

		NotificationType GetNotificationType(
			string schema,
			string connectionString);

		NotificationType SetNotificationTypeForSchema(
			string schema,
			string connectionString,
			int notificationRoleId,
			string instanceId);
	}
}