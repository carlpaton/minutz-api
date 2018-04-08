using System;
using Api.Controllers;
using Core;
using Core.LogProvider;
using Interface.Services;
using Microsoft.AspNetCore.Mvc;
using Notifications;
using NUnit.Framework;
using Minutz.Models.Entities;
using NSubstitute;
using NUnit.Framework.Internal;
using Interface.Repositories;
using Microsoft.Extensions.Logging;
using SqlRepository;

namespace Tests
{
  [TestFixture]
  public class MeetingServiceTests
  {
//    [Test]
//    public void GetMinutzActions_GivenEmptyReferenceId_ShouldThrowArgumentException()
//    {
//      //Arrange
//      var meetingRepository = Substitute.For<IMeetingRepository>();
//      var meetingAgendaRepository = Substitute.For<IMeetingAgendaRepository>();
//      var meetingAttendeeRepository = Substitute.For<IMeetingAttendeeRepository>();
//      var meetingActionRepository = Substitute.For<IMeetingActionRepository>();
//      var userValidationService = Substitute.For<IUserValidationService>();
//      var authenticationService = Substitute.For<IAuthenticationService>();
//      var applicationSetupRepository = Substitute.For<IApplicationSetupRepository>();
//      var userRepository = Substitute.For<IUserRepository>();
//      var applicationSetting = Substitute.For<IApplicationSetting>();
//      var instanceRepository = Substitute.For<IInstanceRepository>();
//      var meetingAttachmentRepository = Substitute.For<IMeetingAttachmentRepository>();
//      var meetingNoteRepository = Substitute.For<IMeetingNoteRepository>();
//      var logService = Substitute.For<ILogService>();
//      var decisionRepository = Substitute.For<IDecisionRepository>();
//      var report = Substitute.For<IReportRepository>();
//      var meetingService =
//        new MeetingService(
//          meetingRepository,meetingAgendaRepository, meetingAttendeeRepository,
//          meetingActionRepository,applicationSetting,meetingAttachmentRepository,meetingNoteRepository,decisionRepository, logService, report);
//      //Act
//
//      //Assert
//      Assert.Throws<ArgumentNullException>(() => meetingService.GetMinutzActions(string.Empty, new AuthRestModel()));
//    }

//    [Test]
//    public void GetMinutzActions_GivenEmptyUserTokenUid_ShouldThrowArgumentException()
//    {
//      //Arrange
//      var meetingRepository = Substitute.For<IMeetingRepository>();
//      var meetingAgendaRepository = Substitute.For<IMeetingAgendaRepository>();
//      var meetingAttendeeRepository = Substitute.For<IMeetingAttendeeRepository>();
//      var meetingActionRepository = Substitute.For<IMeetingActionRepository>();
//      var userValidationService = Substitute.For<IUserValidationService>();
//      var authenticationService = Substitute.For<IAuthenticationService>();
//      var applicationSetupRepository = Substitute.For<IApplicationSetupRepository>();
//      var userRepository = Substitute.For<IUserRepository>();
//      var applicationSetting = Substitute.For<IApplicationSetting>();
//      var instanceRepository = Substitute.For<IInstanceRepository>();
//      var meetingAttachmentRepository = Substitute.For<IMeetingAttachmentRepository>();
//      var meetingNoteRepository = Substitute.For<IMeetingNoteRepository>();
//      var logService = Substitute.For<ILogService>();
//      var decisionRepository = Substitute.For<IDecisionRepository>();
//      var report = Substitute.For<IReportRepository>();
//      var meetingService =
//        new MeetingService(
//          meetingRepository,meetingAgendaRepository, meetingAttendeeRepository,
//          meetingActionRepository,applicationSetting,meetingAttachmentRepository,meetingNoteRepository,decisionRepository, logService, report);
//      //Act
//
//      //Assert
//      Assert.Throws<ArgumentNullException>(() => meetingService.GetMinutzActions("referenceId", new AuthRestModel()));
//    }

//    [Test]
//    public void CreateMeetingAgendaItem_GivenEmptyUserToken_ShouldThrowArgumentException()
//    {
//      //  //Arrange
//      var meetingRepository = Substitute.For<IMeetingRepository>();
//      var meetingAgendaRepository = Substitute.For<IMeetingAgendaRepository>();
//      var meetingAttendeeRepository = Substitute.For<IMeetingAttendeeRepository>();
//      var meetingActionRepository = Substitute.For<IMeetingActionRepository>();
//      var userValidationService = Substitute.For<IUserValidationService>();
//      var authenticationService = Substitute.For<IAuthenticationService>();
//      var applicationSetupRepository = Substitute.For<IApplicationSetupRepository>();
//      var userRepository = Substitute.For<IUserRepository>();
//      var applicationSetting = Substitute.For<IApplicationSetting>();
//      var instanceRepository = Substitute.For<IInstanceRepository>();
//      var meetingAttachmentRepository = Substitute.For<IMeetingAttachmentRepository>();
//      var meetingNoteRepository = Substitute.For<IMeetingNoteRepository>();
//      var logService = Substitute.For<ILogService>();
//      var decisionRepository = Substitute.For<IDecisionRepository>();
//      var report = Substitute.For<IReportRepository>();
//      var meetingService =
//        new MeetingService(
//          meetingRepository,meetingAgendaRepository, meetingAttendeeRepository,
//          meetingActionRepository,applicationSetting,meetingAttachmentRepository,meetingNoteRepository,decisionRepository, logService, report);
//      //  //Act
//      Assert.Throws<ArgumentNullException>(() =>
//        meetingService.CreateMeetingAgendaItem(new MeetingAgenda(), new AuthRestModel()));
//      //  //Assert
//    }
    
//    [Test]
//    public void CreateMeetingAgendaItem_GivenNullModel_ShouldThrowArgumentException()
//    {
//      //  //Arrange
//      var meetingRepository = Substitute.For<IMeetingRepository>();
//      var meetingAgendaRepository = Substitute.For<IMeetingAgendaRepository>();
//      var meetingAttendeeRepository = Substitute.For<IMeetingAttendeeRepository>();
//      var meetingActionRepository = Substitute.For<IMeetingActionRepository>();
//      var userValidationService = Substitute.For<IUserValidationService>();
//      var authenticationService = Substitute.For<IAuthenticationService>();
//      var applicationSetupRepository = Substitute.For<IApplicationSetupRepository>();
//      var userRepository = Substitute.For<IUserRepository>();
//      var applicationSetting = Substitute.For<IApplicationSetting>();
//      var instanceRepository = Substitute.For<IInstanceRepository>();
//      var meetingAttachmentRepository = Substitute.For<IMeetingAttachmentRepository>();
//      var meetingNoteRepository = Substitute.For<IMeetingNoteRepository>();
//      var logService = Substitute.For<ILogService>();
//      var decisionRepository = Substitute.For<IDecisionRepository>();
//      var report = Substitute.For<IReportRepository>();
//      var meetingService =
//        new MeetingService(
//          meetingRepository,meetingAgendaRepository, meetingAttendeeRepository,
//          meetingActionRepository,applicationSetting,meetingAttachmentRepository,meetingNoteRepository,decisionRepository, logService, report);
//      //  //Act
//      Assert.Throws<ArgumentNullException>(() =>
//        meetingService.CreateMeetingAgendaItem(null, new AuthRestModel()));
//      //  //Assert
//    }
    
//    [Test]
//    public void CreateMeetingAgendaItem_GivenNewModelWithEmptyReferenceId_ShouldThrowArgumentException()
//    {
//      //  //Arrange
//      var meetingRepository = Substitute.For<IMeetingRepository>();
//      var meetingAgendaRepository = Substitute.For<IMeetingAgendaRepository>();
//      var meetingAttendeeRepository = Substitute.For<IMeetingAttendeeRepository>();
//      var meetingActionRepository = Substitute.For<IMeetingActionRepository>();
//      var userValidationService = Substitute.For<IUserValidationService>();
//      var authenticationService = Substitute.For<IAuthenticationService>();
//      var applicationSetupRepository = Substitute.For<IApplicationSetupRepository>();
//      var userRepository = Substitute.For<IUserRepository>();
//      var applicationSetting = Substitute.For<IApplicationSetting>();
//      var instanceRepository = Substitute.For<IInstanceRepository>();
//      var meetingAttachmentRepository = Substitute.For<IMeetingAttachmentRepository>();
//      var meetingNoteRepository = Substitute.For<IMeetingNoteRepository>();
//      var logService = Substitute.For<ILogService>();
//      var decisionRepository = Substitute.For<IDecisionRepository>();
//      var report = Substitute.For<IReportRepository>();
//      var meetingService =
//        new MeetingService(
//          meetingRepository,meetingAgendaRepository, meetingAttendeeRepository,
//          meetingActionRepository,applicationSetting,meetingAttachmentRepository,meetingNoteRepository,decisionRepository, logService, report);
//      //  //Act
//      Assert.Throws<ArgumentNullException>(() =>
//        meetingService.CreateMeetingAgendaItem(new MeetingAgenda(), new AuthRestModel()));
//      //  //Assert
//    }
//    
    
    //[Test]
    //public void Get_Given_Should()
    //{
    //  //Arrange

    //  //Act

    //  //Assert
    //}
  }
}
