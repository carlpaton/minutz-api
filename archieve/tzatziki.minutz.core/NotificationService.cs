using tzatziki.minutz.interfaces;
using tzatziki.minutz.models;
using System;

namespace tzatziki.minutz.core
{
	public class NotificationService : INotificationService
	{
		private readonly ISettingService _settingService;

		public string MinutesAccount
		{
			get
			{
				try
				{
					var account = Environment.GetEnvironmentVariable("MINUTESACCOUNT");
					return account;
				}
				catch (Exception)
				{
					throw new NullReferenceException("MINUTESACCOUNT Environment Variable was not found.");
				}
			}
		}

		public string AcctionAccount
		{
			get
			{
				try
				{
					var account = Environment.GetEnvironmentVariable("ACTIONSACCOUNT");
					return account;
				}
				catch (Exception)
				{
					throw new NullReferenceException("ACTIONSACCOUNT Environment Variable was not found.");
				}
			}
		}

		public string InvitesAccount
		{
			get
			{
				try
				{
					var account = Environment.GetEnvironmentVariable("INVITESACCOUNT");
					return account;
				}
				catch (Exception)
				{
					throw new NullReferenceException("INVITESACCOUNT Environment Variable was not found.");
				}
			}
		}

		public NotificationService(ISettingService settingService)
		{
			_settingService = settingService;
		}

		public bool InvitePerson(string email, string message, IHttpService httpService)
		{
			try
			{
				var body = message;
				var subject = "Minutz invitation";
				httpService.SendMessage(Environment.GetEnvironmentVariable("MAILURL"),
							 Environment.GetEnvironmentVariable("MAILUSER"),
							 Environment.GetEnvironmentVariable("MAILAPIKEY")
							 , new MessageModel
							 {
								 To = new System.Collections.Generic.KeyValuePair<string, string>("to", email ),
								 From = new System.Collections.Generic.KeyValuePair<string, string>("from", InvitesAccount),
								 Subject = new System.Collections.Generic.KeyValuePair<string, string>("subject", subject ),
								 Body = new System.Collections.Generic.KeyValuePair<string, string>("html", body)
							 }
							 );
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}


	}
}
