using System;

namespace Models.Entities
{
  public class MeetingAction
  {
    public Guid Id { get; set; }
    public Guid ReferanceId { get; set; }
    public string ActionText { get; set; }
    public Guid PersonId { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsComplete { get; set; }
  }
}