using System;
using Core.Feature.Invite;
using Core.Helper;
using Interface.Helper;
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
            //Arrange
            var attendee = new MeetingAttendee {Email = string.Empty};
            var meeting = new Meeting {Name = "Foo", Id = Guid.NewGuid()};

            var service = new InvitationService(_emailValidationService);

            var result = service.InviteUser(attendee, meeting);

            Assert.IsFalse(result.Condition);
        }

        [Test]
        public void InviteUser_Given_MeetingAttendee_With_InvalidEmail_ReturnFalse()
        {
            var attendee = new MeetingAttendee {Email = "foo"};
            var meeting = new Meeting {Name = "Foo", Id = Guid.NewGuid()};
            var service = new InvitationService(_emailValidationService);

            var result = service.InviteUser(attendee, meeting);

            Assert.IsFalse(result.Condition);
        }

        [Test]
        public void InviteUser_Given_MeetingAttendee_With_Email_ReturnTrue()
        {
            var attendee = new MeetingAttendee {Email = "leeroya@gmail.com"};
            var meeting = new Meeting {Name = "Foo", Id = Guid.NewGuid()};
            var service = new InvitationService(_emailValidationService);

            var result = service.InviteUser(attendee, meeting);

            Assert.IsTrue(result.Condition);
        }

        [Test]
        public void InviteUser_Given_Meeting_With_IdEmpty_ReturnFalse()
        {
            var attendee = new MeetingAttendee {Email = "leeroya@gmail.com"};
            var meeting = new Meeting {Name = "Foo", Id = Guid.Empty};
            var service = new InvitationService(_emailValidationService);

            var result = service.InviteUser(attendee, meeting);

            Assert.IsFalse(result.Condition);
        }

        [Test]
        public void InviteUser_Given_Meeting_With_IdButEmptyName_ReturnFalse()
        {
            var attendee = new MeetingAttendee {Email = "leeroya@gmail.com"};
            var meeting = new Meeting {Name = string.Empty, Id = Guid.NewGuid()};
            var service = new InvitationService(_emailValidationService);

            var result = service.InviteUser(attendee, meeting);

            Assert.IsFalse(result.Condition);
        }

        [Test]
        public void InviteUser_Given_Meeting_With_IdAndValidName_ReturnTrue()
        {
            var attendee = new MeetingAttendee {Email = "leeroya@gmail.com"};
            var meeting = new Meeting {Name = "Foo", Id = Guid.NewGuid()};
            var service = new InvitationService(_emailValidationService);

            var result = service.InviteUser(attendee, meeting);

            Assert.IsTrue(result.Condition);
        }

        //[Test]
        public void InviteUser_Given_Attendee_And_Meeting_IEmailValidationService_IsCalled()
        {
            Mock<IEmailValidationService> _emailValidationService = 
                new Mock<IEmailValidationService>();
            
            _emailValidationService.Setup(x => x.Valid(It.IsAny<string>()).Returns(true));
            
            var attendee = new MeetingAttendee {Email = "leeroya@gmail.com"};
            var meeting = new Meeting {Name = "Foo", Id = Guid.NewGuid()};
            
            var service = new InvitationService(_emailValidationService.Object);

            service.InviteUser(attendee, meeting);
            
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