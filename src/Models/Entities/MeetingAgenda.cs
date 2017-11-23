using System;

namespace Models.Entities
{
  public class MeetingAgenda
  {
    public Guid Id { get; set; }  
    public Guid ReferenceId { get; set; }
    public string AgendaHeading { get; set; }
    public string AgendaText { get; set; }
    public string MeetingAttendeeId { get; set; }
    public string Duration { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsComplete { get; set; }
  }
}
