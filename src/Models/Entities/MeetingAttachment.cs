using System;

namespace Minutz.Models.Entities
{
  public class MeetingAttachment
  {
    public Guid Id { get; set; }
    public Guid ReferanceId { get; set; }
    public string FileName { get; set; }
    public string MeetingAttendeeId { get; set; }
    public DateTime Date { get; set; }
    public byte[] FileData { get; set; }
    public int Order { get; set; }
  }
}
