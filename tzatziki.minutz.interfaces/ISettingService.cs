using System.Collections.Generic;

namespace tzatziki.minutz.interfaces
{
	public interface ISettingService
	{
		KeyValuePair<string, string> InviteSettings { get; }

		KeyValuePair<string, string> ActionSettings { get; }

		KeyValuePair<string, string> MinutesSettings { get; }

		KeyValuePair<string, string> SMTPSettings { get; }

		List<string> SystemAdminCollection { get; }
	}
}
