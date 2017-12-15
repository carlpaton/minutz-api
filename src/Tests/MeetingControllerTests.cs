using Api.Controllers;
using Interface.Services;
using Microsoft.AspNetCore.Mvc;
using Minutz.Models.Entities;
using NUnit.Framework;
using NSubstitute;
using Minutz.Models.ViewModels;

namespace Tests
{
  [TestFixture]
  public class MeetingControllerTests
  {

    [Test]
    public void Get_GivenEmptyMeetingName_ShouldReturnBadRequest()
    {
      //Arrange
      var meetingService = Substitute.For<IMeetingService>();
      var inviteService = Substitute.For<IInvatationService>();
      var controller = new MeetingController(meetingService, inviteService);
      var meeting = new Minutz.Models.ViewModels.MeetingViewModel
      {
        Name = string.Empty
      };

      //Act
      var result = controller.CreateMeeting();

      //Assert
      Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
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
