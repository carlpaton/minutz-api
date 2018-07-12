using System;
using Core.Feature.Invite;
using Core.Helper;
using Interface.Helper;
using Minutz.Models.Entities;
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
            var emailValidationService = new EmailValidationService();
            var attendee = new MeetingAttendee { Email = string.Empty };
            var meeting = new Meeting{ Name = "Foo", Id = Guid.NewGuid() };

            var service = new InvitationService(_emailValidationService);

            var result = service.InviteUser(attendee, meeting);
            
            Assert.IsFalse(result);
        }
        
        //[Test]
        public void InviteUser_Given_MeetingAttendee_With_InvalidEmail_ReturnTrue()
        {
            var attendee = new MeetingAttendee { Email = "foo" };
            var meeting = new Meeting{ Name = "Foo", Id = Guid.NewGuid() };
            
        }
        
        //[Test]
        public void InviteUser_Given_MeetingAttendee_With_Email_ReturnTrue()
        {
            var attendee = new MeetingAttendee { Email = "leeroya@gmail.com" };
            var meeting = new Meeting{ Name = "Foo", Id = Guid.NewGuid() };
            
        }
    }
}

/*
 *check that the invitee has email
 *check that the meeting is valid
 *check that the invite person exists call is made
 *if person does not exist then db create is called
 *if the person exists then check the related
 *build the link for the invite
 *check if the call to the sendgrid is called
 */