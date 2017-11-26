using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Notifications
{
    public class StartupService
    {
        private const string publicKey = "pubkey-72b8a8c83a34cf95c95b6cda0d445c43";
        private const string domain = "minutz.net";
        private const string privateKey = "key-61f7481d1706076f65f954ff6e261e72";
        public StartupService () { }

        public bool SendSimpleMessage ()
        {
            var templateGroupId = "716b366a-5d7f-4e19-a07c-bb32762903e5";
            var apiKey = "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"; //Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY");
            var client = new SendGridClient (apiKey);
            
            var from = new EmailAddress ("test@example.com", "Example User");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress ("leeroya@gmail.com", "Example User");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail (from, to, subject, plainTextContent, htmlContent);
            msg.SetTemplateId(templateGroupId);
            var result = client.SendEmailAsync (msg).Result;
            var resultBody = result.Body.ReadAsStringAsync ().Result;
            return true;

            // HttpClient client = new HttpClient ();
            // client.BaseAddress = new Uri ("https://api.mailgun.net/v3");
            // client.DefaultRequestHeaders.Clear ();
            // client.DefaultRequestHeaders.Accept.Add (new MediaTypeWithQualityHeaderValue ("application/json"));
            // client.DefaultRequestHeaders.Add ("authorization", privateKey);
            // new HttpBasicAuthenticator ("api",
            //     "YOUR_API_KEY");
            //HttpRequestMessage request = new HttpRequestMessage (HttpMethod.Post, $"{domain}/messages");
            //string content = $"param1=1&param2=2";
            //request.AddParameter(new para);
            // request.AddParameter ("domain", "YOUR_DOMAIN_NAME", ParameterType.UrlSegment);
            // request.Resource = "{domain}/messages";
            // request.AddParameter ("from", "Excited User <mailgun@YOUR_DOMAIN_NAME>");
            // request.AddParameter ("to", "bar@example.com");
            // request.AddParameter ("to", "YOU@YOUR_DOMAIN_NAME");

            // request.AddParameter ("text", "Testing some Mailgun awesomness!");
            // request.Method = Method.POST;
            // return client.Execute (request);
            //return client.SendAsync (request).Result;
        }

        public bool TestCustomSend ()
        {
            var apiKey = "Bearer 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";
            var endpoint = "https://api.sendgrid.com/v3/mail/send";
            var client = new HttpClient ();
            client.DefaultRequestHeaders.Clear ();
            client.DefaultRequestHeaders.Add ("Authorization", apiKey);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            var sendModel = new SendModel
            {
                Personalizations = new List<Personalizations> ()
                {
                new Personalizations
                {
                To = new List<To> () { new To { Email = "leeroya@gmail.com" } },
                Subject = "Testing"
                }
                },
                From = new To { Email = "info@docker.durban" }
            };
            //var d = new { personalizations = [{ to = [ { email = "" } ] }] };
            var body = new StringContent (JsonConvert.SerializeObject (
                sendModel,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }), Encoding.UTF8, "application/json");

            var result = client.PostAsync (endpoint, body).Result;
            var content = result.Content.ReadAsStringAsync ();
            return true;
        }

    }

    public class SendModel
    {
        public List<Personalizations> Personalizations { get; set; }
        public To From { get; set; }
        public List<Body> Content { get; set; }
    }
    public class Personalizations
    {
        public List<To> To { get; set; }
        public string Subject { get; set; }
    }
    public class To
    {
        public string Email { get; set; }
    }
    public class Body
    {
        public string Type { get; set; }
        public string value { get; set; }
    }
}