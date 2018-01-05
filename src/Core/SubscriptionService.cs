using System.Collections.Generic;
using Core.Helper;
using Interface.Repositories;
using Interface.Services;
using Minutz.Models.Entities;

namespace Core
{
	public class SubscriptionService : ISubscriptionService
	{
		private readonly ISubscriptionRepository _subscriptionRepository;
		private readonly IAuthenticationService _authenticationService;
		private readonly IInstanceRepository _instanceRepository;
		private readonly IApplicationSetting _applicationSetting;
		private readonly IUserValidationService _userValidationService;
		
		public SubscriptionService(
			ISubscriptionRepository subscriptionRepository,
			IAuthenticationService authenticationService,
			IInstanceRepository instanceRepository,
			IApplicationSetting applicationSetting,
			IUserValidationService userValidationService)
		{
			this._subscriptionRepository = subscriptionRepository;
			this._authenticationService = authenticationService;
			this._instanceRepository = instanceRepository;
			this._applicationSetting = applicationSetting;
			this._userValidationService = userValidationService;
		}
		
		public List<Subscription> GetList(
			string token)
		{
			return _subscriptionRepository.GetList(_applicationSetting.Schema, _applicationSetting.CreateConnectionString());
		}

		public Subscription GetSubscription(
			string token)
		{
			var auth = new AuthenticationHelper(token, _authenticationService, _instanceRepository, _applicationSetting,
				_userValidationService);
			return _subscriptionRepository.GetSubscription(auth.Instance.Username, auth.ConnectionString);
		}

		public Subscription SetSubscriptionForSchema(
			string token,
			int subscriptionId)
		{
			var auth = new AuthenticationHelper(token, _authenticationService, _instanceRepository, _applicationSetting,
				_userValidationService);
			return _subscriptionRepository.SetSubscriptionTypeForSchema(_applicationSetting.Schema,
				_applicationSetting.CreateConnectionString(), subscriptionId, auth.Instance.Username);
		}
	}
}