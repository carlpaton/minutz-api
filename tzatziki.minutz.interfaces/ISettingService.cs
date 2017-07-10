using System;
using System.Collections.Generic;

namespace tzatziki.minutz.interfaces
{
	public interface ISettingService
	{
		Tuple<string, string, string> MAILSettings { get; }

		List<string> SystemAdminCollection { get; }
	}
}
