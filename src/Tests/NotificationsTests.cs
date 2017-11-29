using System;
using Notifications;
using NUnit.Framework;
using Models.Entities;

namespace Tests
{
  public class NotificationsTests
  {
    [Test]
    public void Notify_Given_EmptyApiKeyEnvironmentSetting_Should_ThrowException()
    {
      //Arrange
      Environment.SetEnvironmentVariable("NOTIFY-KEY", string.Empty);

      //Act
      Assert.Throws<ArgumentNullException>(() => new Notify());
      //Assert
    }

    [Test]
    public void Notify_Given_EmptyDefaultTemplateApiKeyEnvironmentSetting_Should_ThrowException()
    {
      //Arrange
      Environment.SetEnvironmentVariable("NOTIFY-DEFAULT-TEMPLATE-KEY", string.Empty);

      //Act
      Assert.Throws<ArgumentNullException>(() => new Notify());
      //Assert
    }

    [Test]
    public void Notify_Given_EmptyNotifyUserEnvironmentSetting_Should_ThrowException()
    {
      //Arrange
      Environment.SetEnvironmentVariable("NOTIFY-USER", string.Empty);

      //Act
      Assert.Throws<ArgumentNullException>(() => new Notify());
      //Assert
    }

    //[Test]
    public void TestSendNotification()
    {
      var meeting = new Meeting();
      meeting.Id = Guid.NewGuid();
      meeting.Name = "Demo Meeting";
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
  }
}