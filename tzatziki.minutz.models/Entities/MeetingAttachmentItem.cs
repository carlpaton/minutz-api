using System;

namespace tzatziki.minutz.models.Entities
{
  public class MeetingAttachmentItem
  {
    public Guid Id { get; set; }
    public Guid ReferanceId { get; set; }
    public Guid MeetingAttendeeId { get; set; }
    public string FileName { get; set; }
    public byte[] FileData { get; set; }
    public DateTime Date { get; set; }
  }
}