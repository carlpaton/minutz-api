using System;

namespace Minutz.Models.Entities
{
  public class MeetingNote
  {
    public Guid Id { get; set; }
    public Guid ReferanceId { get; set; }
    public string NoteText { get; set; }
    public string MeetingAttendeeId { get; set; }
    public DateTime CreatedDate { get; set; }
    public int Order { get; set; }
  }
}
