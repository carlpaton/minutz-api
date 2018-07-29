using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
  public class MeetingItemAttachment
  {
    public string Id { get; set; }
    [Required]
    public string ReferanceId { get; set; }
    [Required]
    public string FileName { get; set; }
    public string MeetingAttendeeId { get; set; }
    public DateTime Date { get; set; }
    public byte[] FileData { get; set; }
  }
}
