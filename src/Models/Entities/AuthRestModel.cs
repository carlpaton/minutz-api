namespace Minutz.Models.Entities
{
  public class AuthRestModel
  {
    public string Sub { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Name { get; set; }
    public string Nickname { get; set; }
    public string Picture { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string Company { get; set; }
    public string InstanceId { get; set; }
    public string Related { get; set; }
    public bool IsVerified {get;set;}
  }
}
