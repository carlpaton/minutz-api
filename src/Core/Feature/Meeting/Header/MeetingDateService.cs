using System;
using Interface.Repositories.Feature.Meeting.Header;
using Interface.Services;
using Interface.Services.Feature.Meeting.Header;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Core.Feature.Meeting.Header
{
    public class MeetingDateService : IMeetingDateService
    {
        private readonly IApplicationSetting _applicationSetting;
        private readonly IMinutzDateRepository _minutzDateRepository;

        public MeetingDateService(IApplicationSetting applicationSetting, IMinutzDateRepository minutzDateRepository)
        {
            _applicationSetting = applicationSetting;
            _minutzDateRepository = minutzDateRepository;
        }

        public MessageBase Update(string meetingId, DateTime date, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _minutzDateRepository.Update(meetingId, date, user.InstanceId, instanceConnectionString);
        }
    }
}