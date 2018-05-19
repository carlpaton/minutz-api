using System;

namespace Minutz.Models.Entities
{
  public class MinutzAction
  {
    public Guid Id { get; set; }
    public Guid referenceId { get; set; }
    public string ActionTitle { get; set; }
    public string ActionText { get; set; }
    public string PersonId { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsComplete { get; set; }
    public string Type { get; set; }
  }
}