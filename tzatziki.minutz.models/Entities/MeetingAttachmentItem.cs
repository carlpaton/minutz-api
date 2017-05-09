using System;

namespace tzatziki.minutz.models.Entities
{
  public class MeetingAttachmentItem
  {
    public int Id { get; set; }
    public int ReferanceId { get; set; }
    public int MeetingAttendeeId { get; set; }
    public string FileName { get; set; }
    public byte[] FileData { get; set; }
    public DateTime Date { get; set; }
  }
}