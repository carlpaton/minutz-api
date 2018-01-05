using System.Collections.Generic;
using Minutz.Models.Entities;

namespace Interface.Services
{
	public interface INotificationRoleService
	{
		List<NotificationRole> GetList(string token);

		NotificationRole GetNotificationRole(string token);

		NotificationRole SetNotificationRoleForSchema(
			string token,
			int notificationRoleId);
	}
}