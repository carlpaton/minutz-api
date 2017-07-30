using System.Collections.Generic;

namespace tzatziki.minutz.models.Interfaces
{
	public interface IMessageModel
	{
		KeyValuePair<string, string> To { get; set; }
		KeyValuePair<string, string> From { get; set; }
		KeyValuePair<string, string> Subject { get; set; }
		KeyValuePair<string, string> Body { get; set; }
	}
}
