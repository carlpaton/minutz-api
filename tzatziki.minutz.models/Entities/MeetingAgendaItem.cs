using System;

namespace tzatziki.minutz.models.Entities
{
  public class MeetingAgendaItem
  {
    public Guid Id { get; set; }
    public string AgendaHeading { get; set; }
    public Guid ReferanceId { get; set; }
    public string AgendaText { get; set; }
    public string MeetingAttendeeId { get; set; }
    public string Duration { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsComplete { get; set; }
  }
}