using System;

namespace Minutz.Models.Entities
{
  public class MeetingNote
  {
    public string Id { get; set; }
    public string ReferanceId { get; set; }
    public string NoteText { get; set; }
    public string MeetingAttendeeId { get; set; }
    public DateTime CreatedDate { get; set; }
    public int Order { get; set; }
  }
}
