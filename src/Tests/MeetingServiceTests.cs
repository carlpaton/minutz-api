using System;
using Api.Controllers;
using Core;
using Interface.Services;
using Microsoft.AspNetCore.Mvc;
using Notifications;
using NUnit.Framework;
using Models.Entities;
using NSubstitute;
using NUnit.Framework.Internal;
using Interface.Repositories;

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
        instanceRepository);
      //Act

      //Assert
      Assert.Throws<ArgumentNullException>(() => meetingService.GetMinutzActions(string.Empty));
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
