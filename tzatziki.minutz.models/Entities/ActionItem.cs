using System;

namespace tzatziki.minutz.models.Entities
{
  public class ActionItem
  {
    public int Id { get; set; }
    public int ReferanceId { get; set; }
    public string ActionText { get; set; }
    public int PersonId { get; set; }
    public DateTime DueDate { get; set; }
  }
}