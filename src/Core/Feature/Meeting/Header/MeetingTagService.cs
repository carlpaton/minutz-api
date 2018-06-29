using Interface.Repositories.Feature.Meeting.Header;
using Interface.Services;
using Interface.Services.Feature.Meeting.Header;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Core.Feature.Meeting.Header
{
    public class MeetingTagService :IMeetingTagService
    {
        private readonly IMinutzTagRepository _minutzTagRepository;
        private readonly IApplicationSetting _applicationSetting;

        public MeetingTagService(IMinutzTagRepository minutzTagRepository, IApplicationSetting applicationSetting)
        {
            _minutzTagRepository = minutzTagRepository;
            _applicationSetting = applicationSetting;
        }

        public MessageBase Update(string meetingId, string tags, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _minutzTagRepository.Update(meetingId, tags, user.InstanceId, instanceConnectionString);
        }
    }
}