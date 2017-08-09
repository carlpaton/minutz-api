using minutz_interface.ViewModels;


namespace minutz_interface.Repositories
{
	public interface IPersonRepository
	{
		IUserProfile Get(string identifier, string email, string name, string picture, string connectionString);

		RoleEnum GetRole(string identifier, string connectionString, IUserProfile profile);

		RoleEnum GetRole(string identifier, string connectionString, IUserProfile profile, string schema);
	}
}