using System;
using Interface.Services;

namespace Notifications
{
  public class Notify : INotify
  {
    public string NotifyUser => "Minutz";
    public string NotifyKey => @"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";
    public string NotifyInvitationAddress => "invitation@minutz.net";
    public string NotifyMinutesAddress => "minutes@minutz.net";
    public string NotifyDefaultTemplateKey => "716b366a-5d7f-4e19-a07c-bb32762903e5";
    public string DestinationBaseAddress { get; private set; }

    public Notify()
    {
      DestinationBaseAddress = Environment.GetEnvironmentVariable("UI_BASE_URL");
    }
  }
}