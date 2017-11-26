using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Interface.Services;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Models.Entities;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Notifications
{
    public class StartupService : IStartupService
    {
        private readonly INotify _notify;
        private readonly string _fromAddress = "invitation@minutz.net";
        private readonly string _invitationSubject = "You are invited to a Minutz Meeting.";
        public StartupService (INotify notify) 
        {
            this._notify = notify;    
        }

        public bool SendInvitationMessage (MeetingAttendee attendee)
        {
            var to = new EmailAddress (attendee.Email, attendee.Name);
            var result = new SendGridClient (_notify.NotifyKey).SendEmailAsync (CreateInvitationMessage(to, _invitationSubject)).Result;
            var resultBody = result.Body.ReadAsStringAsync ().Result;
            return true;
        }

        internal SendGridMessage CreateInvitationMessage(EmailAddress to, string subject)
        {
            var message = MailHelper.CreateSingleEmail (CreateFromUser(), to, subject, createInvitationTextMessage(), createInvitationHtmlMessage());
            message.SetTemplateId(_notify.NotifyDefaultTemplateKey);
            return message;
        }

        internal EmailAddress CreateFromUser()
        {
            return new EmailAddress (this._fromAddress, _notify.NotifyUser);
        }

        internal string createInvitationHtmlMessage()
        {
            return "<strong>and easy to do anywhere, even with C#</strong>";
        }
        internal string createInvitationTextMessage()
        {
            return "and easy to do anywhere, even with C#";
        }
    }
}