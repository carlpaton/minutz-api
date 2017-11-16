using System;

namespace Models.Entities
{
  public class MeetingAttendee
  {
    public Guid Id { get; set; }
    public Guid ReferanceId { get; set; }
    public string Email {get;set;}
    public  string PersonIdentity { get; set; }
    public  string Role { get; set; }
  }
}