using Interface.Repositories.Feature.Dashboard;
using Interface.Services;
using Interface.Services.Feature.Dashboard;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Core.Feature.Dashboard
{
    public class UserMeetingsService : IUserMeetingsService
    {
        private readonly IUserMeetingsRepository _userMeetingsRepository;
        private readonly IApplicationSetting _applicationSetting;

        public UserMeetingsService(IUserMeetingsRepository userMeetingsRepository,
                                   IApplicationSetting applicationSetting)
        {
            _userMeetingsRepository = userMeetingsRepository;
            _applicationSetting = applicationSetting;
        }

        public MeetingMessage CreateEmptyUserMeeting(AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _userMeetingsRepository.CreateEmptyUserMeeting(user.Email, user.InstanceId, instanceConnectionString);
        }

        public MeetingMessage Meetings(AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _userMeetingsRepository.Meetings(user.Email, user.InstanceId, instanceConnectionString);
        }
    }
}