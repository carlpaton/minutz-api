using System.Collections.Generic;
using Core.Helper;
using Interface.Repositories;
using Interface.Services;
using Minutz.Models.Entities;

namespace Core
{
	public class NotificationTypeService : INotificationTypeService
	{
		
		private readonly INotificationTypeRepository _notificationTypeRepository;
		private readonly IAuthenticationService _authenticationService;
		private readonly IInstanceRepository _instanceRepository;
		private readonly IApplicationSetting _applicationSetting;
		private readonly IUserValidationService _userValidationService;

		public NotificationTypeService(
			INotificationTypeRepository notificationTypeRepository,
			IAuthenticationService authenticationService,
			IInstanceRepository instanceRepository,
			IApplicationSetting applicationSetting,
			IUserValidationService userValidationService)
		{
			this._notificationTypeRepository = notificationTypeRepository;
			this._authenticationService = authenticationService;
			this._instanceRepository = instanceRepository;
			this._applicationSetting = applicationSetting;
			this._userValidationService = userValidationService;
		}

		public List<NotificationType> GetList(
			string token)
		{
			return _notificationTypeRepository.GetList(_applicationSetting.Schema, _applicationSetting.CreateConnectionString());
		}

		public NotificationType GetNotificationType(
			string token)
		{
			var auth = new AuthenticationHelper(token, _authenticationService, _instanceRepository, _applicationSetting,
				_userValidationService);
			return _notificationTypeRepository.GetNotificationType(auth.Instance.Username, auth.ConnectionString);
		}

		public NotificationType SetNotificationTypeForSchema(
			string token,
			int notificationTypeId)
		{
			var auth = new AuthenticationHelper(token, _authenticationService, _instanceRepository, _applicationSetting,
				_userValidationService);
			return _notificationTypeRepository.SetNotificationTypeForSchema(_applicationSetting.Schema,
				_applicationSetting.CreateConnectionString(), notificationTypeId, auth.Instance.Username);
		}
	}
}