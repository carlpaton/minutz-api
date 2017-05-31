using System;

namespace tzatziki.minutz.models.Entities
{
  public class ActionItem
  {
    public int Id { get; set; }
    public Guid ReferanceId { get; set; }
    public string ActionText { get; set; }
    public Guid PersonId { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsComplete { get; set; }
  }
}