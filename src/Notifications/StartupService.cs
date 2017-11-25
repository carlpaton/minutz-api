using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;

namespace Notifications
{
    public class StartupService
    {
        private const string publicKey = "pubkey-72b8a8c83a34cf95c95b6cda0d445c43";
        private const string domain = "minutz.net";
        private const string privateKey = "key-61f7481d1706076f65f954ff6e261e72";
        public StartupService ()
        { }

        public HttpResponseMessage SendSimpleMessage ()
        {
            HttpClient client = new HttpClient ();
            client.BaseAddress = new Uri ("https://api.mailgun.net/v3");
            client.DefaultRequestHeaders.Clear ();
            client.DefaultRequestHeaders.Accept.Add (new MediaTypeWithQualityHeaderValue ("application/json"));
            client.DefaultRequestHeaders.Add ("authorization", privateKey);
            // new HttpBasicAuthenticator ("api",
            //     "YOUR_API_KEY");
            HttpRequestMessage request = new HttpRequestMessage (HttpMethod.Post, $"{domain}/messages");
            string content = $"param1=1&param2=2";
            //request.AddParameter(new para);
            // request.AddParameter ("domain", "YOUR_DOMAIN_NAME", ParameterType.UrlSegment);
            // request.Resource = "{domain}/messages";
            // request.AddParameter ("from", "Excited User <mailgun@YOUR_DOMAIN_NAME>");
            // request.AddParameter ("to", "bar@example.com");
            // request.AddParameter ("to", "YOU@YOUR_DOMAIN_NAME");
            
            // request.AddParameter ("text", "Testing some Mailgun awesomness!");
            // request.Method = Method.POST;
            // return client.Execute (request);
            return client.SendAsync (request).Result;
        }

    }
}