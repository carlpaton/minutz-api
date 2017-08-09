namespace minutz_interface.Entities
{
    public interface IInstance
    {
		 int Id { get; set; }
		 string Name { get; set; }
		 string Username { get; set; }
		 string Password { get; set; }
		 bool Active { get; set; }
		 int Type { get; set; }
	}
}