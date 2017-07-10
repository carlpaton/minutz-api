using MailKit.Net.Smtp;
using MimeKit;
using System;
using tzatziki.minutz.interfaces;

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

		public bool InvitePerson()
		{
			try
			{
				var q = new HttpService();
				q.Send(Environment.GetEnvironmentVariable("MAILURL"),
							 Environment.GetEnvironmentVariable("MAILUSER"), 
							 Environment.GetEnvironmentVariable("MAILAPIKEY"));
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
