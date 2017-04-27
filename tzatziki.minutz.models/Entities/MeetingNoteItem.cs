using System;

namespace tzatziki.minutz.models.Entities
{
  public class MeetingNoteItem
  {
    public int Id { get; set; }
    public int ReferanceId { get; set; }
    public string NoteText { get; set; }
    public int MeetingAttendeeId { get; set; }
    public DateTime CreatedDate { get; set; }
  }
}