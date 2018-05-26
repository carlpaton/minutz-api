using System;
using System.Collections.Generic;
using AuthenticationRepository;
using Core;
using Notifications;
using NUnit.Framework;
using Minutz.Models.Entities;
using Reports;
using SqlRepository;

namespace Tests
{
  public class NotificationsTests: TestBase
  {
    //[Test]
    public void Notify_Given_EmptyApiKeyEnvironmentSetting_Should_ThrowException()
    {
      //Arrange
      Environment.SetEnvironmentVariable("NOTIFY-KEY", string.Empty);

      //Act
      Assert.Throws<ArgumentNullException>(() => new Notify());
      //Assert
    }

    //[Test]
    public void Notify_Given_EmptyDefaultTemplateApiKeyEnvironmentSetting_Should_ThrowException()
    {
      //Arrange
      Environment.SetEnvironmentVariable("NOTIFY-DEFAULT-TEMPLATE-KEY", string.Empty);

      //Act
      Assert.Throws<ArgumentNullException>(() => new Notify());
      //Assert
    }

    //[Test]
    public void Notify_Given_EmptyNotifyUserEnvironmentSetting_Should_ThrowException()
    {
      //Arrange
      Environment.SetEnvironmentVariable("NOTIFY-USER", string.Empty);

      //Act
      Assert.Throws<ArgumentNullException>(() => new Notify());
      //Assert
    }

    //[Test] this is a integration test
    public void TestSendNotification()
    {
      var meeting = new Minutz.Models.ViewModels.MeetingViewModel();
      meeting.Id = Guid.NewGuid().ToString();
      meeting.Name = "Demo MeetingViewModel";
      var attendee = new MeetingAttendee();
      attendee.Email = "leeroya@gmail.com";
      attendee.Name = "Lee-Roy Ashworth";


      Environment.SetEnvironmentVariable("NOTIFY-DEFAULT-TEMPLATE-KEY", "716b366a-5d7f-4e19-a07c-bb32762903e5");
      Environment.SetEnvironmentVariable("NOTIFY-KEY", "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");
      Environment.SetEnvironmentVariable("NOTIFY-USER", "Minutz");
      Environment.SetEnvironmentVariable("NOTIFY-INVITATION-ADDRESS", "invitation@minutz.net");
      Environment.SetEnvironmentVariable("UI-BASE-URL", "http://test.minutz.net");

      var notify = new Notify();
      var not = new StartupService(notify);
      var q = not.SendInvitationMessage(attendee, meeting);
      Assert.True(true);
    }

    [Test]
    public void TestReport()
    {
      var appsetting = new ApplicationSetting(new InstanceRepository(), new MinutzEncryption.Encryptor());
      var reportRepo = new JsReportRepository(appsetting,new HttpService());
      var agenditems = new List<dynamic>(){new {agendaHeading = "Some cool meeting idea",agendaText = "Some other information"}};
      var attendees = new List<dynamic>() {new {name = "Lee-Roy", role = "meeting owner"}};
      var notes = new List<dynamic>() {new {noteText = "Somethning to note"}};
      reportRepo.CreateMinutesReport(new 
        {
          name = "Some cool meeting",
          date = DateTime.Now,
          location = "durban",
          time = DateTime.UtcNow,
          purpose = "our purpose",
          outcome = "some outcome",
          agenda = agenditems,
          attendees = attendees,
          notes = notes
        });
    }
  }
}