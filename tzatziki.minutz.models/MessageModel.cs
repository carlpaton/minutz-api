using tzatziki.minutz.models.Interfaces;
using System.Collections.Generic;

namespace tzatziki.minutz.models
{
	public class MessageModel : IMessageModel
	{
		public KeyValuePair<string, string> To { get; set; }
		public KeyValuePair<string, string> From { get; set; }
		public KeyValuePair<string, string> Subject { get; set; }
		public KeyValuePair<string, string> Body { get; set; }
	}
}
