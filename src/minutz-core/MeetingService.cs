using minutz_interface.Services;
using System.Collections.Generic;

namespace minutz_core
{
	public class MeetingService : IMeetingService
	{
		public IEnumerable<KeyValuePair<string, string>> ExtractQueries(string returnUri)
		{
			var queries = new List<KeyValuePair<string, string>>();
			var queryCollection = returnUri.Split('?');
			foreach (var query in queryCollection)
			{
				if (query.Contains("="))
				{
					var temp = query.Split('=');
					queries.Add(new KeyValuePair<string, string>(temp[0], temp[1]));
				}
			}
			return queries;
		}
	}
}