using System.Collections.Generic;
using System.Net;
using System.Text;
using Interface.Services;
using Interface.Services.Feature.Notification;
using Minutz.Models.Entities;
using Minutz.Models.Message;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly INotify _notify;
        private readonly string _invitationSubject = "You are invited to a Minutz Meeting.";
        private readonly EmailAddress _createFromUser;

        public NotificationService(INotify notify)
        {
            _notify = notify;
            _createFromUser = new EmailAddress(_notify.NotifyInvitationAddress, _notify.NotifyUser);
        }

        public MessageBase SendMeetingInvitation(MeetingAttendee attendee, Meeting meeting, string instanceId)
        {
            var to = new EmailAddress(attendee.Email, attendee.Name);

            var result = new SendGridClient(_notify.NotifyKey)
              .SendEmailAsync(CreateInvitationMessage(to, _invitationSubject, meeting.Location, meeting.Id.ToString(), meeting.Name, instanceId)).Result;
            if (result.StatusCode == HttpStatusCode.OK || result.StatusCode == HttpStatusCode.Accepted)
            {
                return new MessageBase { Condition = true, Message = result.Body.ReadAsStringAsync().Result, Code = 200 };
            }
            return new MessageBase { Condition = false, Message = "There was a issue with sending the email invitation.", Code = 500 };
        }

        private SendGridMessage CreateInvitationMessage(
          EmailAddress to, string subject, string location, string meetingId, string meetingName, string instanceId)
        {
            var message = MailHelper.CreateSingleEmail(_createFromUser, to, subject,
                                                        CreateInvitationTextMessage(to.Name, meetingId, meetingName, instanceId),
                                                        CreateInvitationHtmlMessage(to.Name, meetingId, meetingName, instanceId));
            message.CreateCalenderEvent(to.Email, to.Name, location, meetingName);
            message.SetTemplateId(_notify.NotifyDefaultTemplateKey);
            return message;
        }

        private SendGridMessage CreateMinutesMessage(
                  EmailAddress to, string subject, string location, string meetingId, string meetingName, string instanceId)
        {
            var message = MailHelper.CreateSingleEmail(_createFromUser, to, subject,
                                                        CreateInvitationTextMessage(to.Name, meetingId, meetingName, instanceId),
                                                        CreateMeetingHtmlMessage(to.Name, meetingId, meetingName, instanceId));
            //message.CreateCalenderEvent(to.Email, to.Name, location, meetingName);
            message.SetTemplateId(_notify.NotifyDefaultTemplateKey);
            return message;
        }

        private string CreateMeetingHtmlMessage(string attendeeName, string meetingId, string meetingName, string instanceId)
        {
            var message = new StringBuilder();
            message.AppendLine($"<div><h2>{attendeeName},</h2></div>");
            message.AppendLine($"<div><p>Here are the minutes from the meeting: {meetingName} .</p></div>");
            message.AppendLine($"<div><p></p></div>");

            string referanceString = $"{_notify.DestinationBaseAddress}?invite|{instanceId}&{meetingId}";
            message.AppendLine($"<div><p>Click <a href='{referanceString}'>to view</a> the meeting. </p></div>");
            message.AppendLine($"<div></div>");
            return message.ToString();
        }

        private string CreateInvitationHtmlMessage(string attendeeName, string meetingId, string meetingName, string instanceId)
        {
            var message = new StringBuilder();
            message.AppendLine($"<div><h2>Welcome {attendeeName},</h2></div>");
            message.AppendLine($"<div><p>You are invited to the meeting: {meetingName} .</p></div>");
            message.AppendLine($"<div><p></p></div>");

            string referanceString = $"{_notify.DestinationBaseAddress}?invite|{instanceId}&{meetingId}";
            message.AppendLine($"<div><p>Click <a href='{referanceString}'>Join</a> to accept the meeting request, and start collaborating. </p></div>");
            message.AppendLine($"<div></div>");
            return message.ToString();
        }

        private static string CreateInvitationTextMessage(string attendeeName, string meetingId, string meetingName, string instanceId)
        {
            return "and easy to do anywhere, even with C#";
        }

        public MessageBase SendMeetingMinutes(MeetingAttendee attendee, Meeting meeting, string instanceId)
        {
            var to = new EmailAddress(attendee.Email, attendee.Name);

            var result = new SendGridClient(_notify.NotifyKey)
              .SendEmailAsync(CreateMinutesMessage(to, _invitationSubject, meeting.Location, meeting.Id.ToString(), meeting.Name, instanceId)).Result;
            if (result.StatusCode == HttpStatusCode.OK || result.StatusCode == HttpStatusCode.Accepted)
            {
                return new MessageBase { Condition = true, Message = result.Body.ReadAsStringAsync().Result, Code = 200 };
            }
            return new MessageBase { Condition = false, Message = "There was a issue with sending the email invitation.", Code = 500 };
        }
    }
}
