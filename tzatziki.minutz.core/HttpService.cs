using System;
using System.Text;
using System.Net.Http;
using tzatziki.minutz.interfaces;

namespace tzatziki.minutz.core
{
	public class HttpService :IHttpService
	{
		public HttpResponseMessage Send(string url, string username, string password)
		{
			var client = new HttpClient();
			var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

			HttpResponseMessage response = client.PostAsync(url, new MultipartFormDataContent
			{
				{new StringContent("invitations@minutz.net"),"from" },
				{new StringContent("leeroya@gmail.com"),"to" },
				{new StringContent("test from code"),"subject" },
				{new StringContent("<b>Some text</b>"),"html" }
			}).Result;
			return response;
		}
	}
}
