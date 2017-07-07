using MailKit.Net.Smtp;
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
			using (var client = new SmtpClient())
			{
				client.Connect("smtp.example.com", 587, false);
				client.AuthenticationMechanisms.Remove("XOAUTH2");
				// Note: since we don't have an OAuth2 token, disable 	// the XOAUTH2 authentication mechanism.     client.Authenticate("anuraj.p@example.com", "password");
				client.Send(message);
				client.Disconnect(true);
			}
			return true;
		}
	}
}
