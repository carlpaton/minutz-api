using System;

namespace Minutz.Models.Entities
{
  public class MinutzDecision
  {
    public Guid Id { get; set; }
    public Guid ReferenceId { get; set; }
    public string DescisionText { get; set; }
    public string Descisioncomment { get; set; }
    public string AgendaId { get; set; }
    public Guid PersonId { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsOverturned { get; set; }
  }
}