using System.Collections.Generic;
using Core.Helper;
using Interface.Repositories;
using Interface.Services;
using Minutz.Models.Entities;

namespace Core
{
	public class NotificationRoleService : INotificationRoleService
	{
		private readonly INotificationRoleRepository _notificationRoleRepository;
		private readonly IAuthenticationService _authenticationService;
		private readonly IInstanceRepository _instanceRepository;
		private readonly IApplicationSetting _applicationSetting;
		private readonly IUserValidationService _userValidationService;

		public NotificationRoleService(
			INotificationRoleRepository notificationRoleRepository,
			IAuthenticationService authenticationService,
			IInstanceRepository instanceRepository,
			IApplicationSetting applicationSetting,
			IUserValidationService userValidationService)
		{
			this._notificationRoleRepository = notificationRoleRepository;
			this._authenticationService = authenticationService;
			this._instanceRepository = instanceRepository;
			this._applicationSetting = applicationSetting;
			this._userValidationService = userValidationService;
		}

		public List<NotificationRole> GetList(
			string token)
		{
			return _notificationRoleRepository.GetList(_applicationSetting.Schema, _applicationSetting.CreateConnectionString());
		}

		public NotificationRole GetNotificationRole(
			string token)
		{
			var auth = new AuthenticationHelper(token, _authenticationService, _instanceRepository, _applicationSetting,
				_userValidationService);
			return _notificationRoleRepository.GetNotificationRole(auth.Instance.Username, auth.ConnectionString);
		}

		public NotificationRole SetNotificationRoleForSchema(
			string token,
			int notificationRoleId)
		{
			var auth = new AuthenticationHelper(token, _authenticationService, _instanceRepository, _applicationSetting,
				_userValidationService);
			return _notificationRoleRepository.SetNotificationRoleForSchema(_applicationSetting.Schema,
				_applicationSetting.CreateConnectionString(), notificationRoleId, auth.Instance.Username);
		}
	}
}