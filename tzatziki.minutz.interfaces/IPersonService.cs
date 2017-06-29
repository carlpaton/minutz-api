using System.Collections.Generic;
using tzatziki.minutz.models.Auth;
using tzatziki.minutz.models.Entities;

namespace tzatziki.minutz.interfaces
{
	public interface IPersonService
	{
		IEnumerable<models.Entities.Person> GetSystemUsers(string connectionString, string schema = "app");

		IEnumerable<UserProfile> GetSchemaUsers(string connectionString, string schema);

		bool InvitePerson(Person person, string connectionString, string schema);
	}
}
