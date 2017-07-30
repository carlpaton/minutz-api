using System.Net.Http;
using tzatziki.minutz.models.Interfaces;

namespace tzatziki.minutz.interfaces
{
	public interface IHttpService
	{
		HttpResponseMessage SendMessage(string url, string username, string password, IMessageModel message);
	}
}
