using System.Collections.Generic;


namespace minutz_interface.Services
{
	public interface IMeetingService
	{
		IEnumerable<KeyValuePair<string, string>> ExtractQueries(string returnUri);
	}
}