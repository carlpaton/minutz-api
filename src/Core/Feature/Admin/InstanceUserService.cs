using System;
using System.Collections.Generic;
using System.Linq;
using Interface.Repositories;
using Interface.Repositories.Feature.Admin;
using Interface.Services;
using Interface.Services.Admin;
using Interface.Services.Feature.Notification;
using Minutz.Models;
using Minutz.Models.Entities;
using Minutz.Models.Extensions;
using Minutz.Models.Message;

namespace Core.Feature.Admin
{
    public class InstanceUserService: IInstanceUserService
    {
        private readonly IApplicationSetting _applicationSetting;
        private readonly IInstanceUserRepository _instanceUserRepository;
        private readonly INotificationService _notificationService;
        private readonly IInstanceRepository _instanceRepository;

        public InstanceUserService(
            IApplicationSetting applicationSetting,
            IInstanceUserRepository instanceUserRepository,
            INotificationService notificationService,
            IInstanceRepository instanceRepository)
        {
            _applicationSetting = applicationSetting;
            _instanceUserRepository = instanceUserRepository;
            _notificationService = notificationService;
            _instanceRepository = instanceRepository;
        }

        public PersonResponse GetInstancePeople(AuthRestModel user)
        {
            var result = new PersonResponse {Code = 500, Condition = false, Message = string.Empty, People = new List<Person>()};
            var masterConnectionString = _applicationSetting.CreateConnectionString();
            var peopleResult = _instanceUserRepository.GetInstancePeople(user.InstanceId, masterConnectionString);
            result.Condition = peopleResult.Condition;
            result.Code = peopleResult.Code;
            result.Message = peopleResult.Message;
            if (peopleResult.Condition)
                result.People = peopleResult.People;
            return result;
        }

        public PersonResponse AddInstancePerson(Person person, AuthRestModel user)
        {
            var result = new PersonResponse {Code = 500, Condition = false, Message = string.Empty, People = new List<Person>()};
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            var masterConnectionString = _applicationSetting.CreateConnectionString();

            var instancePeople = _instanceUserRepository.GetInstancePeople(user.InstanceId, masterConnectionString);
            var personExists = instancePeople.People.FirstOrDefault(i => i.Email == person.Email);
            
            if (personExists == null)
            {
                var relatedCollection = new List<(string instanceId, string meetingId)> {(user.InstanceId, string.Empty)};
                person.Related = relatedCollection.ToRelatedString();
                var newPerson =
                    _instanceUserRepository.AddInstancePerson(person, user.InstanceId, masterConnectionString);
            }
            else
            {
                var related = person.Related.SplitToList(StringDeviders.InstanceStringDevider, StringDeviders.MeetingStringDevider);
                related.Add((user.InstanceId,string.Empty));
                person.Related = related.ToRelatedString();
                var updatePerson =
                    _instanceUserRepository.UpdateInstancePerson(person, user.InstanceId, masterConnectionString);
            }
            var p = _instanceUserRepository
                .GetInstancePeople(user.InstanceId, masterConnectionString)
                .People
                .FirstOrDefault(i=> i.Email == person.Email);
            if (p != null)
            {
                try
                {
                    _notificationService.SendInstanceInvatation(p, user.InstanceId, user.Company);
                }
                catch (Exception e)
                {
                    Console.WriteLine("NOTE: Trying to send instance invatation.");
                    Console.WriteLine(e);
                }
            }

            if (p != null)
            {
                result.Code = 200;
                result.Person = p;
                result.Message = "Success";
            }
            else
            {
                result.Code = 404;
                result.Message = "The user cannot be found.";
            }

            return result;
        }
    }
}