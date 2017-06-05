using System;

namespace tzatziki.minutz.models.Entities
{
  public class MeetingAttendee
  {
    public Guid Id { get; set; }
    public string PersonIdentity { get; set; }
    public Guid ReferanceId { get; set; }
    public string Role { get; set; }
  }
}