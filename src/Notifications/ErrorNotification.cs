using System;
using Interface.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Notifications
{
  public class ErrorNotification
  {
    private readonly INotify _notify;
    private readonly string _invitationSubject = "Application Error";

    public ErrorNotification(INotify notify)
    {
      this._notify = notify;
    }

    public bool SendErrorMail(Exception ex)
    {
      var to = new EmailAddress("lee@leeroya.com", "Admin");
      var result = new SendGridClient(_notify.NotifyKey)
        .SendEmailAsync(CreateInvitationMessage(to, ex, this._invitationSubject))
                                      .Result;
      var resultBody = result.Body.ReadAsStringAsync().Result;
      if (result.StatusCode == System.Net.HttpStatusCode.OK || result.StatusCode == System.Net.HttpStatusCode.Accepted)
      {
        return true;
      }
      return false;
    }
    internal EmailAddress CreateFromUser()
    {
      return new EmailAddress(_notify.NotifyInvitationAddress,
                               _notify.NotifyUser);
    }

    internal SendGridMessage CreateInvitationMessage(EmailAddress to, Exception ex, string subject)
    {
      var message = MailHelper.CreateSingleEmail(CreateFromUser(),
                                                  to,
                                                  subject,
                                                  ex.InnerException.Message,
                                                  ex.InnerException.Message + "\n" + ex.StackTrace);
      message.SetTemplateId(_notify.NotifyDefaultTemplateKey);
      return message;
    }
  }
}
