using System;
namespace Api.Models
{
  public class MeetingItemNote
  {
    public string Id { get; set; }
    public string ReferanceId { get; set; }
    public string NoteText { get; set; }
    public string MeetingAttendeeId { get; set; }
    public DateTime CreatedDate { get; set; }
  }
}
