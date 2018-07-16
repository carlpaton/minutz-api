using System;
using System.Collections.Generic;
using AuthenticationRepository;
using Core;
using Minutz.Models.Entities;
using Notifications;
using NUnit.Framework;
using Reports;
using SqlRepository;

namespace Tests.Features.Notification
{
  public class NotificationsTests: TestBase
  {
    [Test]
    public void TestSendNotification()
    {
      
      var Id = Guid.NewGuid();
      var meeting = new Meeting
                    {
                      Id = Id,
                      Name = "Unit Test Email"
                    };
      
      var attendee = new MeetingAttendee
                     {
                       ReferenceId = Id,
                       Email = "leeroya@gmail.com",
                       Name = "Lee-Roy Ashworth"
                     };
  

      var notify = new Notify();
      var service = new NotificationService(notify);
      //var not = new StartupService(notify);
      var mail = service.SendMeetingInvitation(attendee, meeting, Id.ToString());
      Assert.True(true);
    }

    //[Test]
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