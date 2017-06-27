using System.Collections.Generic;
using tzatziki.minutz.interfaces;
using System;

namespace tzatziki.minutz.core
{
	public class SettingService: ISettingService
	{
		private const string INVITESACCOUNT = "INVITESACCOUNT";
		private const string INVITESPASSWORD = "INVITESPASSWORD";
		private const string ACTIONSPASSWORD = "ACTIONSPASSWORD";
		private const string ACTIONSACCOUNT = "ACTIONSACCOUNT";
		private const string MINUTESPASSWORD = "MINUTESPASSWORD";
		private const string MINUTESACCOUNT = "MINUTESACCOUNT";
		private const string SMTPDOMAIN = "SMTPDOMAIN";
		private const string SMTPPORT = "SMTPPORT";

		/// <summary>
		/// Key is the mail account, value is the password
		/// </summary>
		public KeyValuePair<string, string> InviteSettings
		{
			get
			{
				try
				{
					return new KeyValuePair<string, string>(
						Environment.GetEnvironmentVariable(INVITESACCOUNT),
						Environment.GetEnvironmentVariable(INVITESPASSWORD));
				}
				catch (Exception)
				{
					throw new Exception($"EnvironmentVariable for: {INVITESACCOUNT} cannot be found.");
				}
			}
		}

		/// <summary>
		/// Key is the mail account, value is the password
		/// </summary>
		public KeyValuePair<string, string> ActionSettings
		{
			get
			{
				try
				{
					return new KeyValuePair<string, string>(
						Environment.GetEnvironmentVariable(ACTIONSACCOUNT),
						Environment.GetEnvironmentVariable(ACTIONSPASSWORD));
				}
				catch (Exception)
				{
					throw new Exception($"EnvironmentVariable for: {ACTIONSACCOUNT} cannot be found.");
				}
			}
		}

		/// <summary>
		/// Key is the mail account, value is the password
		/// </summary>
		public KeyValuePair<string, string> MinutesSettings
		{
			get
			{
				try
				{
					return new KeyValuePair<string, string>(
						Environment.GetEnvironmentVariable(MINUTESACCOUNT),
						Environment.GetEnvironmentVariable(MINUTESPASSWORD));
				}
				catch (Exception)
				{
					throw new Exception($"EnvironmentVariable for: {MINUTESACCOUNT} cannot be found.");
				}
			}
		}

		/// <summary>
		/// Key is SMTP Domain, Value is the Port
		/// </summary>
		public KeyValuePair<string, string> SMTPSettings
		{
			get
			{
				try
				{
					return new KeyValuePair<string, string>(
						Environment.GetEnvironmentVariable(SMTPDOMAIN),
						Environment.GetEnvironmentVariable(SMTPPORT));
				}
				catch (Exception)
				{
					throw new Exception($"EnvironmentVariable for: {MINUTESACCOUNT} cannot be found.");
				}
			}
		}

		public List<string> SystemAdminCollection
		{
			get
			{
				var admins = new List<string>() { "leeroya@gmail.com", "mauro@minutz.co", "russell@minutz.co" };
				return admins;
			}
		}
	}
}
