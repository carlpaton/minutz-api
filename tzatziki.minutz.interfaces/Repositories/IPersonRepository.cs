using System;
using System.Collections.Generic;
using tzatziki.minutz.models.Auth;
using tzatziki.minutz.models.Entities;

namespace tzatziki.minutz.interfaces.Repositories
{
  public interface IPersonRepository
  {
    bool IsPerson(string email);

    UserProfile Get(string identifier, string email, string name, string picture,string connectionString);

    UserProfile InsertInstanceIdForUser(UserProfile user, string connectionString);

    RoleEnum GetRole(string identifier, string connectionString, UserProfile profile);

		Guid GetInstanceIdForUser(string userIdentifier, string connectionString, string schema = "app");

		IEnumerable<models.Entities.Person> GetSystemUsers(string connectionString, string schema = "app");

		IEnumerable<UserProfile> GetSchemaUsers(string connectionString, string schema);

		bool InvitePerson(Person person, string connectionString, string schema);
	}
}