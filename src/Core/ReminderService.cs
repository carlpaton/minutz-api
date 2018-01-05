using System.Collections.Generic;
using Core.Helper;
using Interface.Repositories;
using Interface.Services;
using Minutz.Models.Entities;

namespace Core
{
	public class ReminderService : IReminderService
	{
		private readonly IReminderRepository _reminderRepository;
		private readonly IAuthenticationService _authenticationService;
		private readonly IInstanceRepository _instanceRepository;
		private readonly IApplicationSetting _applicationSetting;
		private readonly IUserValidationService _userValidationService;
		
		public ReminderService(
			IReminderRepository reminderRepository,
			IAuthenticationService authenticationService,
			IInstanceRepository instanceRepository,
			IApplicationSetting applicationSetting,
			IUserValidationService userValidationService)
		{
			this._reminderRepository = reminderRepository;
			this._authenticationService = authenticationService;
			this._instanceRepository = instanceRepository;
			this._applicationSetting = applicationSetting;
			this._userValidationService = userValidationService;
		}

		public List<Reminder> GetList(
			string token)
		{
			return _reminderRepository.GetList(_applicationSetting.Schema, _applicationSetting.CreateConnectionString());
		}

		public Reminder GetReminder(
			string token)
		{
			var auth = new AuthenticationHelper(token, _authenticationService, _instanceRepository, _applicationSetting,
				_userValidationService);
			return _reminderRepository.GetReminder(auth.Instance.Username, auth.ConnectionString);
		}

		public Reminder SetReminderForSchema(
			string token,
			int reminderId)
		{
			var auth = new AuthenticationHelper(token, _authenticationService, _instanceRepository, _applicationSetting,
				_userValidationService);
			return _reminderRepository.SetReminderForSchema(_applicationSetting.Schema,
				_applicationSetting.CreateConnectionString(), reminderId, auth.Instance.Username);
		}
	}
}