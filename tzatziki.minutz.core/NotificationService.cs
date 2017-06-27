using tzatziki.minutz.interfaces;

namespace tzatziki.minutz.core
{
	public class NotificationService : INotificationService
	{
		private readonly ISettingService _settingService;
		public NotificationService(ISettingService settingService)
		{
			_settingService = settingService;
		}

		public bool InvitePerson()
		{
			return true;
		}
	}
}
