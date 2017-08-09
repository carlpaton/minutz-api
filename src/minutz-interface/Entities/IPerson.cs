namespace minutz_interface.Entities
{
	public interface IPerson
	{
		int Id { get; set; }
		string Identityid { get; set; }
		string FirstName { get; set; }
		string LastName { get; set; }
		string FullName { get; set; }
		string ProfilePicture { get; set; }
		string Email { get; set; }
		string Role { get; set; }
		string InstanceId { get; set; }
		bool Active { get; set; }
	}
}