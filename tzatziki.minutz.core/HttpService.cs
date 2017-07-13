using tzatziki.minutz.models.Interfaces;
using tzatziki.minutz.interfaces;
using System.Net.Http;
using System.Text;
using System;

namespace tzatziki.minutz.core
{
	public class HttpService :IHttpService
	{
		public HttpResponseMessage SendMessage(string url, string username, string password, IMessageModel message)
		{
			var client = new HttpClient();
			var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

			HttpResponseMessage response = client.PostAsync(url, new MultipartFormDataContent
			{
				{new StringContent(message.From.Value),message.From.Key},
				{new StringContent(message.To.Value),message.To.Key},
				{new StringContent(message.Subject.Value),message.Subject.Key },
				{new StringContent(message.Body.Value),message.Body.Key }
			}).Result;
			return response;
		}
	}
}
