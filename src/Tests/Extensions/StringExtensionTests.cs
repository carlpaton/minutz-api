using System;
using NUnit.Framework;
using Minutz.Models.Extensions;
using System.Collections.Generic;

namespace Tests.Extensions
{
  [TestFixture]
  public class StringExtensionTests
  {
    [Test]
    public void ToSplitTupleList_GivenEmptyString_ShouldReturn_NewEmptyTuple()
    {
      //Arrange
      string input = string.Empty;
      //Act
      var result = input.ToSplitTupleList();
      //Assert
      Assert.IsInstanceOf(typeof(List<(string, string)>), result);
    }

    [Test]
    public void ToSplitTupleList_GivenEmptyCommaString_ShouldReturn_NewEmptyTuple()
    {
      //Arrange
      string input = ",";
      //Act
      var result = input.ToSplitTupleList();
      //Assert
      Assert.IsInstanceOf(typeof(List<(string, string)>), result);
    }

    [Test]
    public void ToSplitTupleList_GivenEmptyPipeString_ShouldReturn_NewEmptyTuple()
    {
      //Arrange
      string input = "|";
      //Act
      var result = input.ToSplitTupleList();
      //Assert
      Assert.IsInstanceOf(typeof(List<(string, string)>), result);
    }

    [Test]
    public void ToSplitTupleList_GivenCommaPipeString_ShouldReturn_NewFilledTuple()
    {
      //Arrange
      string input = "instance|meeting";
      //Act
      var result = input.ToSplitTupleList();
      //Assert
      Assert.IsInstanceOf(typeof(List<(string, string)>), result);
      Assert.IsTrue(result.Count == 1);
    }

    [Test]
    public void ToSplitTupleList_GivenCommaPipeString_ShouldReturn_NewMultipleFilledTuple()
    {
      //Arrange
      string input = "instance|meeting,instance2|meerting1";
      //Act
      var result = input.ToSplitTupleList();
      //Assert
      Assert.IsInstanceOf(typeof(List<(string, string)>), result);
      Assert.IsTrue(result.Count == 2);
    }
  }
}
