using tzatziki.minutz.interfaces.Repositories;
using tzatziki.minutz.models.Entities;
using System.Collections.Generic;
using tzatziki.minutz.interfaces;
using tzatziki.minutz.models.Auth;


namespace tzatziki.minutz.core
{
	public class PersonService : IPersonService
	{
		private readonly IPersonRepository _personRepository;
		private readonly INotificationService _notificationService;

		public PersonService(IPersonRepository personRepository, 
												 INotificationService notificationService)
		{
			_personRepository = personRepository;
			_notificationService = notificationService;
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
		public bool InvitePerson(Person person,string connectionString, string schema)
		{
			//_notificationService
			var successful = _personRepository.InvitePerson(person,connectionString, schema);
			if (successful)
				_notificationService.InvitePerson();
			return successful;
		}
	}
}
