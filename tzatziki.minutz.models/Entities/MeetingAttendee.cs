namespace tzatziki.minutz.models.Entities
{
  public class MeetingAttendee
  {
    public int Id { get; set; }
    public string PersonIdentity { get; set; }
    public int ReferanceId { get; set; }
    public string Role { get; set; }
  }
}