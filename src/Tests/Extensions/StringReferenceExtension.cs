using System;
using System.Collections.Generic;
using NUnit.Framework;
using SqlRepository.Extensions;
namespace Tests.Extensions
{
  [TestFixture]
  public class StringReferenceExtension
  {
    [Test]
    public void SplitToList_GivenEmptyString_ShouldThrowArgumentException()
    {
      //Arrange
      var input = string.Empty;
      //Act

      //Assert
      Assert.Throws<ArgumentException>(() => input.SplitToList(string.Empty, string.Empty));
    }

    [Test]
    public void SplitToList_GivenValidStringEmptyDevider_ShouldThrowArgumentException()
    {
      //Arrange
      var input = "instance&meeting";
      //Act

      //Assert
      Assert.Throws<FormatException>(() => input.SplitToList(string.Empty, string.Empty));
    }

    [Test]
    public void SplitToList_GivenValidStringAndEmptySplit_ShouldThrowArgumentException()
    {
      //Arrange
      var input = "instance&meeting";
      //Act

      //Assert
      Assert.Throws<FormatException>(() => input.SplitToList(string.Empty, string.Empty));
    }

    [Test]
    public void SplitToList_GivenValidStringWithValidDeviderAndValidSplit_ReturnOneInstance()
    {
      //Arrange
      var input = "instance&meeting";
      //Act
      var result = input.SplitToList("&", ";");
      //Assert
      Assert.IsInstanceOf(typeof(List<(string key, string value)>), result);
      Assert.IsTrue(result.Count == 1);
    }

    [Test]
    public void SplitToList_GivenValidStringWithValidDeviderAndValidSplitInvalidSecondValue_ReturnOneInstance()
    {
      //Arrange
      var input = "instance&meeting;&;";
      //Act
      var result = input.SplitToList("&", ";");
      //Assert
      Assert.IsInstanceOf(typeof(List<(string key, string value)>), result);
      Assert.IsTrue(result.Count == 1);
    }

    [Test]
    public void SplitToList_GivenValidStringWithValidDeviderAndValidSplit_ReturnMultipleInstance()
    {
      //Arrange
      var input = "instance&meeting;instance&meeting";
      //Act
      var result = input.SplitToList("&", ";");
      //Assert
      Assert.IsInstanceOf(typeof(List<(string key, string value)>), result);
      Assert.IsTrue(result.Count == 2);
    }
  }
}

//static List<(string key, string value)> SplitToList(this string input, string split, string devider)