namespace tzatziki.minutz.models.Entities
{
  public class Person
  {
    public int Id { get; set; }
    public string Identityid { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string InstanceId { get; set; }
    public bool Active { get; set; }
  }
}