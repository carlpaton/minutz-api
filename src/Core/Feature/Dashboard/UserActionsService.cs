using Interface.Repositories.Feature.Dashboard;
using Interface.Services;
using Interface.Services.Feature.Dashboard;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Core.Feature.Dashboard
{
    public class UserActionsService : IUserActionsService
    {
        private readonly IUserActionsRepository _userActionsRepository;
        private readonly IApplicationSetting _applicationSetting;

        public UserActionsService(IUserActionsRepository userActionsRepository, IApplicationSetting applicationSetting)
        {
            _userActionsRepository = userActionsRepository;
            _applicationSetting = applicationSetting;
        }
        
        public ActionMessage Actions(AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _userActionsRepository.Actions(user.Email, user.InstanceId, instanceConnectionString);
        }
    }
}