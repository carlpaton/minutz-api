using System.Collections.Generic;
using tzatziki.minutz.interfaces;
using System;

namespace tzatziki.minutz.core
{
	public class SettingService: ISettingService
	{
		/// <summary>
		/// Key is SMTP Domain, Value is the Port
		/// </summary>
		public Tuple<string, string, string> MAILSettings
		{
			get
			{
				try
				{
					return new Tuple<string, string, string>(
						Environment.GetEnvironmentVariable("MAILURL"),
						Environment.GetEnvironmentVariable("MAILAPIKEY"),
						Environment.GetEnvironmentVariable("MAILUSER")
						);
				}
				catch (Exception)
				{
					throw new Exception($"EnvironmentVariable for: MAILURL,  cannot be found.");
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
