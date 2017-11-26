using System;
using Interface.Services;

namespace Notifications
{
    public class Notify : INotify
    {
        public string NotifyUser{ get; private set;}
        public string NotifyKey { get; private set; }
        public string NotifyDefaultTemplateKey {get; private set;}
        public Notify ()
        {
            if (string.IsNullOrEmpty (Environment.GetEnvironmentVariable ("NOTIFY-KEY")))
            {
                throw new ArgumentNullException ("The environment variable: NOTIFY-KEY, was not supplied, please set this variable and try again.");
            }
            this.NotifyKey = Environment.GetEnvironmentVariable ("NOTIFY-KEY");

            if(String.IsNullOrEmpty(Environment.GetEnvironmentVariable("NOTIFY-DEFAULT-TEMPLATE-KEY")))
            {
                throw new ArgumentException("The environment variable: NOTIFY-DEFAULT-TEMPLATE-KEY, was not supplied, please set this variable and try again.");
            }
            this.NotifyDefaultTemplateKey = Environment.GetEnvironmentVariable("NOTIFY-DEFAULT-TEMPLATE-KEY");

            if(string.IsNullOrEmpty(Environment.GetEnvironmentVariable("NOTIFY-USER")))
            {
                throw new ArgumentException("The environment variable: NOTIFY-USER, was not supplied, please set this variable and try again.");
            }
            this.NotifyUser = Environment.GetEnvironmentVariable("NOTIFY-USER");
        }
    }
}