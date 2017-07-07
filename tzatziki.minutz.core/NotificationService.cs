using MailKit.Net.Smtp;
using MimeKit;
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
			var bodyBuilder = new BodyBuilder();
			bodyBuilder.HtmlBody = @"<b>This is bold and this is <i>italic</i></b>";
			//message.Body = bodyBuilder.ToMessageBody();

			var message = new MimeMessage();
			message.From.Add(new MailboxAddress("Minutz", "leeroya@gmail.com"));
			message.To.Add(new MailboxAddress("Lee-Roy Ashworth", "leeroya@gmail.com"));
			message.Subject = "Hello World - A mail from ASPNET Core";
			message.Body = new TextPart("plain")
			{
				Text = "Hello World - A mail from ASPNET Core"
			};
			using (var client = new SmtpClient())
			{
				client.Connect("smtp.gmail.com", 587, false);
				client.Authenticate("leeroya@gmail.com", "7qwWi8gIzrI6");
				//client.AuthenticationMechanisms.Remove("XOAUTH2");
				// Note: since we don't have an OAuth2 token, disable 	// the XOAUTH2 authentication mechanism.     client.Authenticate("anuraj.p@example.com", "password");
				client.Send(message);
				client.Disconnect(true);
			}
			return true;
		}
	}
}
