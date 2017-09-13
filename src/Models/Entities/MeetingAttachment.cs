using System;

namespace Models.Entities
{
  public class MeetingAttachment
  {
    public Guid Id { get; set; }
    public Guid ReferanceId { get; set; }
    public string FileName { get; set; }
    public Guid MeetingAttendeeId { get; set; }
    public DateTime Date { get; set; }
    public byte[] FileData { get; set; }
  }
}
