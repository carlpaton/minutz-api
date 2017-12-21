using System;
using Api.Controllers;
using Core;
using Interface.Services;
using Microsoft.AspNetCore.Mvc;
using Notifications;
using NUnit.Framework;
using Minutz.Models.Entities;
using NSubstitute;
using NUnit.Framework.Internal;
using Interface.Repositories;
using Microsoft.Extensions.Logging;

namespace Tests
{
  [TestFixture]
  public class MeetingServiceTests
  {
    [Test]
    public void GetMinutzActions_GivenEmptyReferenceId_ShouldThrowArgumentException()
    {
      //Arrange
      var meetingRepository = Substitute.For<IMeetingRepository>();
      var meetingAgendaRepository = Substitute.For<IMeetingAgendaRepository>();
      var meetingAttendeeRepository = Substitute.For<IMeetingAttendeeRepository>();
      var meetingActionRepository = Substitute.For<IMeetingActionRepository>();
      var userValidationService = Substitute.For<IUserValidationService>();
      var authenticationService = Substitute.For<IAuthenticationService>();
      var applicationSetupRepository = Substitute.For<IApplicationSetupRepository>();
      var userRepository = Substitute.For<IUserRepository>();
      var applicationSetting = Substitute.For<IApplicationSetting>();
      var instanceRepository = Substitute.For<IInstanceRepository>();
      var meetingAttachmentRepository = Substitute.For<IMeetingAttachmentRepository>();
      var meetingNoteRepository = Substitute.For<IMeetingNoteRepository>();
      var loggerFactory = Substitute.For<ILoggerFactory>();
      var meetingService =
        new MeetingService(
        meetingRepository,
        meetingAgendaRepository,
        meetingAttendeeRepository,
        meetingActionRepository,
        authenticationService,
        userValidationService,
        applicationSetupRepository,
        userRepository,
        applicationSetting,
        instanceRepository,
        meetingAttachmentRepository,
        meetingNoteRepository, loggerFactory);
      //Act

      //Assert
      Assert.Throws<ArgumentNullException>(() => meetingService.GetMinutzActions(string.Empty, string.Empty));
    }

    [Test]
    public void GetMinutzActions_GivenEmptyUserTokenUid_ShouldThrowArgumentException()
    {
      //Arrange
      var meetingRepository = Substitute.For<IMeetingRepository>();
      var meetingAgendaRepository = Substitute.For<IMeetingAgendaRepository>();
      var meetingAttendeeRepository = Substitute.For<IMeetingAttendeeRepository>();
      var meetingActionRepository = Substitute.For<IMeetingActionRepository>();
      var userValidationService = Substitute.For<IUserValidationService>();
      var authenticationService = Substitute.For<IAuthenticationService>();
      var applicationSetupRepository = Substitute.For<IApplicationSetupRepository>();
      var userRepository = Substitute.For<IUserRepository>();
      var applicationSetting = Substitute.For<IApplicationSetting>();
      var instanceRepository = Substitute.For<IInstanceRepository>();
      var meetingattachmentRepository = Substitute.For<IMeetingAttachmentRepository>();
      var meetingNoteRepository = Substitute.For<IMeetingNoteRepository>();
      var loggerFactory = Substitute.For<ILoggerFactory>();
      var meetingService =
        new MeetingService(
          meetingRepository,
          meetingAgendaRepository,
          meetingAttendeeRepository,
          meetingActionRepository,
          authenticationService,
          userValidationService,
          applicationSetupRepository,
          userRepository,
          applicationSetting,
          instanceRepository,
          meetingattachmentRepository,
          meetingNoteRepository, loggerFactory);
      //Act

      //Assert
      Assert.Throws<ArgumentNullException>(() => meetingService.GetMinutzActions("referenceId", string.Empty));
    }

    //[Test]
    //public void Get_Given_Should()
    //{
    //  //Arrange

    //  //Act

    //  //Assert
    //}
  }
}
