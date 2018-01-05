using Core.Helper;
using Interface.Repositories;
using Interface.Services;
using Minutz.Models.Entities;

namespace Core
{
	public class InstanceService: IInstanceService
	{
		private readonly IAuthenticationService _authenticationService;
		private readonly IInstanceRepository _instanceRepository;
		private readonly IApplicationSetting _applicationSetting;
		private readonly IUserValidationService _userValidationService;

		public InstanceService(
			IAuthenticationService authenticationService,
			IInstanceRepository instanceRepository,
			IApplicationSetting applicationSetting,
			IUserValidationService userValidationService)
		{
			this._instanceRepository = instanceRepository;
		}

		public Instance SetInstanceDetailsForSchema(
			string token,
			Instance instance)
		{
			var auth = new AuthenticationHelper(token, _authenticationService, _instanceRepository, _applicationSetting,
				_userValidationService);

			auth.Instance.Colour = instance.Colour;
			auth.Instance.Style = instance.Style;
			auth.Instance.AllowInformal = instance.AllowInformal;
			
			return _instanceRepository.SetInstanceDetailsForSchema(_applicationSetting.Schema,
				_applicationSetting.CreateConnectionString(), auth.Instance);
		}
	}
}