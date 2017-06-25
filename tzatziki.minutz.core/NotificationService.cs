using System;
using System.Collections.Generic;
using System.Text;
using tzatziki.minutz.interfaces;

namespace tzatziki.minutz.core
{
	public class NotificationService
	{
		private readonly ISettingService _settingService;
		public NotificationService(ISettingService settingService)
		{
			_settingService = settingService;
		}

		
	}
}
