using System;

namespace Models.Entities
{
  public class MeetingNote
  {
    public Guid Id { get; set; }
    public Guid ReferanceId { get; set; }
    public string NoteText { get; set; }
    public Guid MeetingAttendeeId { get; set; }
    public DateTime CreatedDate { get; set; }
  }
}
