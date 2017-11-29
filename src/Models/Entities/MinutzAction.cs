using System;

namespace Models.Entities
{
  public class MinutzAction
  {
    public Guid Id { get; set; }
    public Guid ReferanceId { get; set; }
    public string ActionText { get; set; }
    public Guid PersonId { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsComplete { get; set; }
    public string Type { get; set; }
  }
}