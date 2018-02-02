using System.Collections.Generic;
using System.Text;
using Interface.Services;
using Minutz.Models.Entities;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Notifications
{
  public class InvatationService : IInvatationService
  {
    private readonly INotify _notify;
    private readonly string _invitationSubject = "You are invited to a Minutz Meeting.";

    public InvatationService(INotify notify)
    {
      this._notify = notify;
    }

    public bool SendMeetingInvatation(MeetingAttendee attendee,
                                      Minutz.Models.ViewModels.MeetingViewModel meeting,
                                      string instanceId)
    {
      var to = new EmailAddress(attendee.Email, attendee.Name);
      var result = new SendGridClient(_notify.NotifyKey)
                                      .SendEmailAsync(CreateInvitationMessage(to,
                                                                               _invitationSubject,
                                                                               meeting.Location,
                                                                               meeting.Id.ToString(),
                                                                               meeting.Name, meeting.MeetingAgendaCollection,
                                                                               instanceId))
                                      .Result;
      var resultBody = result.Body.ReadAsStringAsync().Result;
      if (result.StatusCode == System.Net.HttpStatusCode.OK || result.StatusCode == System.Net.HttpStatusCode.Accepted)
      {
        return true;
      }
      return false;
    }

    internal SendGridMessage CreateInvitationMessage(EmailAddress to,
                                                     string subject,
                                                      string location,
                                                     string meetingId,
                                                     string meetingName,
                                                     List<MeetingAgenda> agenda,
                                                     string instanceId)
    {
      var message = MailHelper.CreateSingleEmail(CreateFromUser(),
                                                  to,
                                                  subject,
                                                  createInvitationTextMessage(to.Name, meetingId, meetingName, agenda),
                                                  createInvitationHtmlMessage(to.Name, meetingId, meetingName, agenda, instanceId));
      message.CreateCalenderEvent(to.Email,to.Name,location,meetingName);
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
                                                string meetingName,
                                                List<MeetingAgenda> agenda,
                                                string instanceId)
    {
      var message = new StringBuilder();
      message.AppendLine($"<div><h2>Welcome {attendeeName},</h2></div>");
      message.AppendLine($"<div><p>You are invited to the meeting: {meetingName} .</p></div>");
      message.AppendLine($"<div><p></p></div>");
      foreach (var agendaItem in agenda)
      {
        message.AppendLine($"<div><div>{agendaItem.AgendaText}</div><div></div></div>");
      }
      string referanceString = $"{_notify.DestinationBaseAddress}?invite|{instanceId}&{meetingId}";
      message.AppendLine($"<div><p>Click <a href='{referanceString}'>Join</a> to accept the meeting request, and start collaborating. </p></div>");
      message.AppendLine($"<div></div>");
      return message.ToString();
    }
    internal string createInvitationTextMessage(string attendeeName,
                                                string meetingId,
                                                string meetingName,
                                                List<MeetingAgenda> agenda)
    {
      return "and easy to do anywhere, even with C#";
    }
  }
}
