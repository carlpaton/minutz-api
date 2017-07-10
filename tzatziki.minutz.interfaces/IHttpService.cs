using System.Net.Http;

namespace tzatziki.minutz.interfaces
{
	public interface IHttpService
	{
		HttpResponseMessage Send(string url, string username, string password);
	}
}
