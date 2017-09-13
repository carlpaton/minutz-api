using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;
using System;

namespace Notifications
{
  public class StartupService
  {
    public object ConfigurationManager { get; private set; }

    public async System.Threading.Tasks.Task<bool> SendAsync()
    {
      MailjetClient client = new MailjetClient("a8e0ebe577afed8d1af45372c2a73178",
                                              "ded64e2d81a54c76dcf18e46d32937b3");


      MailjetRequest request = new MailjetRequest
      {
        Resource = Contact.Resource,
      }.Property(Send.Messages, new JArray {
                new JObject {
                 {"From", new JObject {
                  {"Email", "leeroya@gmail.com"},
                  {"Name", "Mailjet Pilot"}
                  }},
                 {"To", new JArray {
                  new JObject {
                   {"Email", "lee@leeroya.com"},
                   {"Name", "passenger"}
                   }
                  }},
                 {"TemplateID", 1},
                 {"TemplateLanguage", true},
                 {"Subject", "Your email flight plan!"}
                 }
                });

      MailjetResponse response = await client.PostAsync(request);
      return response.IsSuccessStatusCode;
    }
    public void T()
    {
      //"MAILURL": "https://api.mailgun.net/v3/minutz.net/messages",
      //"MAILAPIKEY": "key-61f7481d1706076f65f954ff6e261e72",
      // "MAILUSER": "api",

      MailjetClient client = new MailjetClient(Environment.GetEnvironmentVariable("MJ_APIKEY_PUBLIC"), Environment.GetEnvironmentVariable("MJ_APIKEY_PRIVATE"))
      {
        Version = ApiVersion.V3_1,
      };
      MailjetRequest request = new MailjetRequest
      {
        Resource = Send.Resource,
      }.Property(Send.Messages, new JArray {
                new JObject {
                 {"From", new JObject {
                  {"Email", "pilot@mailjet.com"},
                  {"Name", "Mailjet Pilot"}
                  }},
                 {"To", new JArray {
                  new JObject {
                   {"Email", "passenger@mailjet.com"},
                   {"Name", "passenger"}
                   }
                  }},
                 {"Subject", "Your email flight plan!"},
                 {"TemplateLanguage", true},
                 {"TextPart", "Dear {{data:firstname:\"passenger\"}}, welcome to Mailjet! "},
                 {"HTMLPart", "Dear {{data:firstname:\"passenger\"}}, welcome to Mailjet!"}
                 }
                });
      //MailjetResponse response = await client.PostAsync(request);
    }
  }
}
