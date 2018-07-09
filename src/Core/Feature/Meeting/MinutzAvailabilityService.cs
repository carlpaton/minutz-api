using Interface.Repositories.Feature.Meeting;
using Interface.Services;
using Interface.Services.Feature.Meeting;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Core.Feature.Meeting
{
    /// <summary>
    /// Manage the users that can be used for a meeting
    /// </summary>
    public class MinutzAvailabilityService: IMinutzAvailabilityService
    {
        private readonly IApplicationSetting _applicationSetting;
        private readonly IMinutzAvailabilityRepository _minutzAvailabilityRepository;

        public MinutzAvailabilityService(IApplicationSetting applicationSetting,
                                         IMinutzAvailabilityRepository minutzAvailabilityRepository)
        {
            _applicationSetting = applicationSetting;
            _minutzAvailabilityRepository = minutzAvailabilityRepository;
        }

        /// <summary>
        /// Get the attendees that can be used for a meeting
        /// </summary>
        /// <param name="user">Current logged in user</param>
        /// <returns>Collection of people that can be used for a meeting</returns>
        public AttendeeMessage GetAvailableAttendees( AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString (_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword (user.InstanceId));
            var masterConnectionString = _applicationSetting.CreateConnectionString();
            return _minutzAvailabilityRepository.GetAvailableAttendees(user.InstanceId, instanceConnectionString, masterConnectionString);
        }
    }
}