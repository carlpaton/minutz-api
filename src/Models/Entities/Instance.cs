using System;

namespace Minutz.Models.Entities
{
  public class Instance
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public bool Active { get; set; }
    public int Type { get; set; }
    public int SubscriptionId { get; set; }
    public DateTime SubscriptionDate { get; set; }
    public byte[] Logo { get; set; }
    public string Colour { get; set; }
    public string Style { get; set; }
    public bool AllowInformal { get; set; }
    public int NotificationTypeId { get; set; }
    public int NotificationRoleId { get; set; }
    public int ReminderId { get; set; }
  }
}