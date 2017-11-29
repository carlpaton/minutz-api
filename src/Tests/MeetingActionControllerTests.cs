using Api.Controllers;
using Interface.Services;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using NSubstitute;

namespace Tests
{
  [TestFixture]
  public class MeetingActionControllerTests
  {

    [Test]
    public void Get_GivenEmptyreferenceId_ShouldReturnBadRequest()
    {
      //Arrange
      var meetingService = Substitute.For<IMeetingService>();
      var controller = new MeetingActionController(meetingService);

      //Act
      var result = controller.Get(string.Empty);

      //Assert
      Assert.IsInstanceOf(typeof(BadRequestObjectResult),result);
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
