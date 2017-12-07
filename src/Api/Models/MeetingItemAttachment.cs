using System;
namespace Api.Models
{
  public class MeetingItemAttachment
  {
    public string Id { get; set; }
    public string ReferanceId { get; set; }
    public string FileName { get; set; }
    public string MeetingAttendeeId { get; set; }
    public DateTime Date { get; set; }
    public byte[] FileData { get; set; }
  }
}
