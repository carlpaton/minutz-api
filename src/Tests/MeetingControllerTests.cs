using Api.Controllers;
using Interface.Services;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using NUnit.Framework;
using NSubstitute;

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
      var controller = new MeetingController(meetingService);
      var meeting = new Models.ViewModels.Meeting
      {
        Name = string.Empty
      };

      //Act
      var result = controller.Put(meeting);

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
