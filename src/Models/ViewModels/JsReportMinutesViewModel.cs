using System.Collections.Generic;

namespace Minutz.Models.ViewModels
{
  public class JsReportMinutesViewModel
  {
    public string name { get; set; }
    public string location { get; set; }
    public string date { get; set; }
    public string time { get; set; }
    public string purpose { get; set; }
    public string outcome { get; set; }
    public List<JsReportMinutesAgenda> agenda { get; set; }
    public List<JsReportAttendee> attendees { get; set; }
    public List<JsReportNote> notes { get; set; }
  }
}