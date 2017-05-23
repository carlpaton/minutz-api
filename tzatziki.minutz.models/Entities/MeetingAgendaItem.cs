using System;

namespace tzatziki.minutz.models.Entities
{
  public class MeetingAgendaItem
  {
    public Guid Id { get; set; }
    public string AgendaHeading { get; set; }
    public int ReferanceId { get; set; }
    public string AgendaText { get; set; }
    public int MeetingAttendeeId { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsComplete { get; set; }
  }
}