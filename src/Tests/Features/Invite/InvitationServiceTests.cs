using System;
using Core.Feature.Invite;
using Core.Helper;
using Interface.Helper;
using Interface.Repositories;
using Interface.Repositories.Feature.Meeting;
using Interface.Services;
using Interface.Services.Feature.Invite;
using Minutz.Models.Entities;
using Minutz.Models.Message;
using Moq;
using NSubstitute;
using NUnit.Framework;

namespace Tests.Features.Invite
{
    [TestFixture]
    public class InvitationServiceTests
    {
        private readonly IEmailValidationService _emailValidationService;

        public InvitationServiceTests()
        {
            _emailValidationService = new EmailValidationService();
        }

        [Test]
        public void InviteUser_Given_MeetingAttendee_With_EmptyEmail_ReturnFalse()
        {
            var availabilityRepository = Substitute.For<IMinutzAvailabilityRepository>();
            var attendeeRepository = Substitute.For<IMinutzAttendeeRepository>();
            var userRepository = Substitute.For<IUserRepository>();
            var applicationSetting = Substitute.For<IApplicationSetting>();
            //Arrange
            var attendee = new MeetingAttendee {Email = string.Empty};
            var meeting = new Meeting {Name = "Foo", Id = Guid.NewGuid()};
            var restAuthUser = new AuthRestModel{InstanceId = Guid.NewGuid().ToString(), Email = "foo@foo.com"};

            var service = new InvitationService(_emailValidationService, availabilityRepository, attendeeRepository, userRepository , applicationSetting);

            var result = service.InviteUser(restAuthUser, attendee, meeting);

            Assert.IsFalse(result.Condition);
        }

        [Test]
        public void InviteUser_Given_MeetingAttendee_With_InvalidEmail_ReturnFalse()
        {
            var availabilityRepository = Substitute.For<IMinutzAvailabilityRepository>();
            var attendeeRepository = Substitute.For<IMinutzAttendeeRepository>();
            var userRepository = Substitute.For<IUserRepository>();
            var applicationSetting = Substitute.For<IApplicationSetting>();
            var restAuthUser = new AuthRestModel{InstanceId = Guid.NewGuid().ToString(), Email = "foo@foo.com"};
            
            var attendee = new MeetingAttendee {Email = "foo"};
            var meeting = new Meeting {Name = "Foo", Id = Guid.NewGuid()};
            var service = new InvitationService(_emailValidationService, availabilityRepository, attendeeRepository, userRepository , applicationSetting);

            var result = service.InviteUser(restAuthUser, attendee, meeting);

            Assert.IsFalse(result.Condition);
        }

        [Test]
        public void InviteUser_Given_MeetingAttendee_With_Email_ReturnTrue()
        {   var availabilityRepository = Substitute.For<IMinutzAvailabilityRepository>();
            var attendeeRepository = Substitute.For<IMinutzAttendeeRepository>();
            var userRepository = Substitute.For<IUserRepository>();
            var applicationSetting = Substitute.For<IApplicationSetting>();
            var restAuthUser = new AuthRestModel{InstanceId = Guid.NewGuid().ToString(), Email = "foo@foo.com"};

            userRepository.CheckIfNewUser("foo", "foo", "foo", "foo", "foo").ReturnsForAnyArgs(new MessageBase { Condition = true, Code = 4});
            
            var attendee = new MeetingAttendee {Email = "leeroya@gmail.com"};
            var meeting = new Meeting {Name = "Foo", Id = Guid.NewGuid()};
            var service = new InvitationService(_emailValidationService,
                availabilityRepository,
                attendeeRepository,
                userRepository , applicationSetting);

            var result = service.InviteUser(restAuthUser,attendee, meeting);

            Assert.IsTrue(result.Condition);
        }

        [Test]
        public void InviteUser_Given_Meeting_With_IdEmpty_ReturnFalse()
        {
            var availabilityRepository = Substitute.For<IMinutzAvailabilityRepository>();
            var attendeeRepository = Substitute.For<IMinutzAttendeeRepository>();
            var userRepository = Substitute.For<IUserRepository>();
            var applicationSetting = Substitute.For<IApplicationSetting>();
            var restAuthUser = new AuthRestModel{InstanceId = Guid.NewGuid().ToString(), Email = "foo@foo.com"};
            
            var attendee = new MeetingAttendee {Email = "leeroya@gmail.com"};
            var meeting = new Meeting {Name = "Foo", Id = Guid.Empty};
            var service = new InvitationService(_emailValidationService, availabilityRepository, attendeeRepository, userRepository , applicationSetting);

            var result = service.InviteUser(restAuthUser,attendee, meeting);

            Assert.IsFalse(result.Condition);
        }

        [Test]
        public void InviteUser_Given_Meeting_With_IdButEmptyName_ReturnFalse()
        {
            var availabilityRepository = Substitute.For<IMinutzAvailabilityRepository>();
            var attendeeRepository = Substitute.For<IMinutzAttendeeRepository>();
            var userRepository = Substitute.For<IUserRepository>();
            var applicationSetting = Substitute.For<IApplicationSetting>();
            var restAuthUser = new AuthRestModel{InstanceId = Guid.NewGuid().ToString(), Email = "foo@foo.com"};
            
            var attendee = new MeetingAttendee {Email = "leeroya@gmail.com"};
            var meeting = new Meeting {Name = string.Empty, Id = Guid.NewGuid()};
            var service = new InvitationService(_emailValidationService, availabilityRepository, attendeeRepository, userRepository , applicationSetting);

            var result = service.InviteUser(restAuthUser,attendee, meeting);

            Assert.IsFalse(result.Condition);
        }

        [Test]
        public void InviteUser_Given_Meeting_With_IdAndValidName_ReturnTrue()
        {
            var availabilityRepository = Substitute.For<IMinutzAvailabilityRepository>();
            var attendeeRepository = Substitute.For<IMinutzAttendeeRepository>();
            var userRepository = Substitute.For<IUserRepository>();
            var applicationSetting = Substitute.For<IApplicationSetting>();
            var restAuthUser = new AuthRestModel{InstanceId = Guid.NewGuid().ToString(), Email = "foo@foo.com"};
            userRepository.CheckIfNewUser("foo", "foo", "foo", "foo", "foo").ReturnsForAnyArgs(new MessageBase { Condition = true, Code = 4});
            
            var attendee = new MeetingAttendee {Email = "leeroya@gmail.com"};
            var meeting = new Meeting {Name = "Foo", Id = Guid.NewGuid()};
            var service = new InvitationService(_emailValidationService, availabilityRepository, attendeeRepository, userRepository , applicationSetting);

            var result = service.InviteUser(restAuthUser, attendee, meeting);

            Assert.IsTrue(result.Condition);
        }

        //[Test]
        public void InviteUser_Given_Attendee_And_Meeting_IEmailValidationService_IsCalled()
        {
            var availabilityRepository = Substitute.For<IMinutzAvailabilityRepository>();
            var attendeeRepository = Substitute.For<IMinutzAttendeeRepository>();
            var userRepository = Substitute.For<IUserRepository>();
            var applicationSetting = Substitute.For<IApplicationSetting>();
            var restAuthUser = new AuthRestModel{InstanceId = Guid.NewGuid().ToString(), Email = "foo@foo.com"};
            
            Mock<IEmailValidationService> _emailValidationService = 
                new Mock<IEmailValidationService>();
            
            _emailValidationService.Setup(x => x.Valid(It.IsAny<string>()).Returns(true));
            
            var attendee = new MeetingAttendee {Email = "leeroya@gmail.com"};
            var meeting = new Meeting {Name = "Foo", Id = Guid.NewGuid()};
            
            var service = new InvitationService(_emailValidationService.Object, availabilityRepository, attendeeRepository, userRepository , applicationSetting);

            service.InviteUser(restAuthUser, attendee, meeting);
            
            _emailValidationService.Verify(x=> x.Valid("leeroya@gmail.com"),Times.AtLeastOnce);
            
        }
    }
}

/*
 *check that the invite person exists call is made
 *if person does not exist then db create is called
 *if the person exists then check the related
 *build the link for the invite
 *check if the call to the sendgrid is called
 */