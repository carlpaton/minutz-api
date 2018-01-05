using System.Collections.Generic;
using Minutz.Models.Entities;

namespace Interface.Services
{
	public interface INotificationTypeService
	{
		List<NotificationType> GetList(
			string token);

		NotificationType GetNotificationType(
			string token);

		NotificationType SetNotificationTypeForSchema(
			string token,
			int notificationTypeId);
	}
}