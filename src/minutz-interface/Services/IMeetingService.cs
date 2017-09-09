using System.Collections.Generic;


namespace Interface.Services
{
	public interface IMeetingService
	{
		IEnumerable<KeyValuePair<string, string>> ExtractQueries(string returnUri);
	}
}