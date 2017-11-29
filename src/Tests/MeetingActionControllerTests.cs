using System;
using Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Notifications;
using NUnit.Framework;
using Models.Entities;

namespace Tests
{
  [TestFixture]
  public class MeetingActionControllerTests
  {
    public MeetingActionControllerTests()
    {
    }

    [Test]
    public void Get_GivenEmptyreferenceId_ShouldReturnBadRequest()
    {
      //Arrange
      var controller = new MeetingActionController();

      //Act
      var result = controller.Get(string.Empty);

      //Assert
      Assert.IsInstanceOf(typeof(BadRequestObjectResult),result);
    }

    [Test]
    public void Get_Given_Should()
    {
      //Arrange

      //Act

      //Assert
    }
  }
}
