using tzatziki.minutz.interfaces.Repositories;
using tzatziki.minutz.models.Entities;
using tzatziki.minutz.models.Auth;
using System.Collections.Generic;
using tzatziki.minutz.interfaces;

namespace tzatziki.minutz.core
{
	public class PersonService : IPersonService
	{
		private readonly IPersonRepository _personRepository;
		private readonly INotificationService _notificationService;
		private readonly IHttpService _httpService;

		public PersonService(IPersonRepository personRepository, 
												 INotificationService notificationService, 
												 IHttpService httpService)
		{
			_personRepository = personRepository;
			_notificationService = notificationService;
			_httpService = httpService;
		}

		public IEnumerable<UserProfile> GetSchemaUsers(string connectionString, string schema)
		{
			return _personRepository.GetSchemaUsers(connectionString, schema);
		}

		public IEnumerable<Person> GetSystemUsers(string connectionString, string schema = "app")
		{
			return _personRepository.GetSystemUsers(connectionString, schema);
		}

		/// <summary>
		/// This save the person into the User table and sends a invatation
		/// </summary>
		/// <param name="person"></param>
		/// <param name="connectionString"></param>
		/// <param name="schema"></param>
		/// <returns></returns>
		public bool InvitePerson(Person person, string message ,string connectionString, string schema)
		{
			if (string.IsNullOrEmpty(person.Identityid)) person.Identityid = System.Guid.NewGuid().ToString();
			var successful = _personRepository.InvitePerson(person,connectionString, schema);
			if (successful)
				_notificationService.InvitePerson(person.Email, message,_httpService);
			return successful;
		}
	}
}
