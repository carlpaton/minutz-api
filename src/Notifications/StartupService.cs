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
using Minutz.Models.Entities;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Notifications
{
  public class StartupService : IStartupService
  {
    private readonly INotify _notify;
    private readonly string _invitationSubject = "You are invited to a Minutz MeetingViewModel.";
    public StartupService(INotify notify)
    {
      this._notify = notify;
    }

    public bool SendInvitationMessage(Minutz.Models.Entities.MeetingAttendee attendee, Minutz.Models.Entities.Meeting meeting)
    {
      var to = new EmailAddress(attendee.Email, attendee.Name);
      var result = new SendGridClient(_notify.NotifyKey)
                                      .SendEmailAsync(CreateInvitationMessage(to,
                                                                               _invitationSubject,
                                                                               meeting.Id.ToString(),
                                                                               meeting.Name))
                                      .Result;
      var resultBody = result.Body.ReadAsStringAsync().Result;
      return true;
    }

    internal SendGridMessage CreateInvitationMessage(EmailAddress to,
                                                     string subject,
                                                     string meetingId,
                                                     string meetingName)
    {
      var message = MailHelper.CreateSingleEmail(CreateFromUser(),
                                                  to,
                                                  subject,
                                                  createInvitationTextMessage(),
                                                  createInvitationHtmlMessage(to.Name, meetingId, meetingName));
      message.SetTemplateId(_notify.NotifyDefaultTemplateKey);
      return message;
    }

    internal EmailAddress CreateFromUser()
    {
      return new EmailAddress(_notify.NotifyInvitationAddress,
                               _notify.NotifyUser);
    }

    internal string createInvitationHtmlMessage(string attendeeName,
                                                string meetingId,
                                                string meetingName)
    {
      var message = new StringBuilder();
      message.AppendLine($"<div><h2>Welcome {attendeeName},</h2></div>");
      message.AppendLine($"<div><p>You are invited to: {meetingName} .</p></div>");
      message.AppendLine($"<div><p></p></div>");
      message.AppendLine($"<div><p>Click <a href='{_notify.DestinationBaseAddress}?meetingId={meetingId}'>Join</a> to accept the meeting request, and start collaborating. </p></div>");
      message.AppendLine($"<div></div>");
      return message.ToString();
    }
    internal string createInvitationTextMessage()
    {
      return "and easy to do anywhere, even with C#";
    }
  }
}