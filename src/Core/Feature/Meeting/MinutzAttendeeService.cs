using System;
using Interface.Repositories.Feature.Meeting;
using Interface.Services;
using Interface.Services.Feature.Meeting;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Core.Feature.Meeting
{
    public class MinutzAttendeeService: IMinutzAttendeeService
    {
        private readonly IApplicationSetting _applicationSetting;
        private readonly IMinutzAttendeeRepository _minutzAttendeeRepository;

        public MinutzAttendeeService(IApplicationSetting applicationSetting,
                                     IMinutzAttendeeRepository minutzAttendeeRepository)
        {
            _applicationSetting = applicationSetting;
            _minutzAttendeeRepository = minutzAttendeeRepository;
        }

        public AttendeeMessage GetAttendees(Guid meetingId, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString (_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword (user.InstanceId));
            var masterConnectionString = _applicationSetting.CreateConnectionString();
            return _minutzAttendeeRepository.GetAttendees(meetingId, user.InstanceId, instanceConnectionString, masterConnectionString);
        }

        public AttendeeMessage AddAttendee(Guid meetingId, MeetingAttendee attendee ,AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString (_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword (user.InstanceId));
            return _minutzAttendeeRepository.AddAttendee(meetingId, attendee, user.InstanceId,
                instanceConnectionString);
        }
        
        public AttendeeMessage UpdateAttendee(Guid meetingId, MeetingAttendee attendee ,AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString (_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword (user.InstanceId));
            return _minutzAttendeeRepository.AddAttendee(meetingId, attendee, user.InstanceId,
                instanceConnectionString);
        }

        public MessageBase DeleteAttendee(Guid meetingId, string attendeeEmail, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString (_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword (user.InstanceId));
            return _minutzAttendeeRepository.DeleteAttendee(meetingId, attendeeEmail, user.InstanceId,
                instanceConnectionString);
        }
    }
}