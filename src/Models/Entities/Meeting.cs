using System;

namespace Minutz.Models.Entities
{
  public class Meeting
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string Time { get; set; }
    public int Duration { get; set; }
    public string Location { get; set; }
    public bool IsReacurance { get; set; }
    public bool IsPrivate { get; set; }
    public string ReacuranceType { get; set; }
    public bool IsLocked { get; set; }
    public bool IsFormal { get; set; }
    public string TimeZone { get; set; }
    public string Tag { get; set; }
    public string Purpose { get; set; }
    public string MeetingOwnerId { get; set; }
    public string Outcome { get; set; }
  }
}
