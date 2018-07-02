using Interface.Services;
using Minutz.Models.Message;
using Minutz.Models.Entities;
using Interface.Repositories.Feature.Meeting;

namespace Core.Feature.Meeting
{
    public class UserManageMeetingService: IUserManageMeetingService
    {
        private readonly IApplicationSetting _applicationSetting;
        private readonly IUserManageMeetingRepository _userManageMeetingRepository;

        public UserManageMeetingService(IApplicationSetting applicationSetting, IUserManageMeetingRepository userManageMeetingRepository)
        {
            _applicationSetting = applicationSetting;
            _userManageMeetingRepository = userManageMeetingRepository;
        }

        public MeetingMessage UpdateMeeting (Minutz.Models.Entities.Meeting meeting, AuthRestModel user) {
            var instanceConnectionString = _applicationSetting.CreateConnectionString (_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword (user.InstanceId));
            return _userManageMeetingRepository.Update(meeting, user.InstanceId, instanceConnectionString);
        }
    }
}