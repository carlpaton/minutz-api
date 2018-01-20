using System;
using NUnit.Framework;
using SqlRepository.Extensions;

namespace Tests.Extensions
{
  [TestFixture]
  public class TupleToStringExtensionTests
  {

    [Test]
    public void ToString_GivenEmptyTuple_ShouldReturnEmptyString()
    {
      //arrange
      (string key, string reference) input = (string.Empty, string.Empty);
      //act
      var result = input.ToFormattedString("|");
      //assert
      Assert.AreEqual(result, string.Empty);
    }

    [Test]
    public void ToString_GivenTuple_ShouldReturnReferenceString()
    {
      //arrange
      (string key, string reference) input = ("invite", "Instance");
      //act
      var result = input.ToFormattedString("|");
      //assert
      Assert.AreEqual(result, "invite|Instance");
    }

    [Test]
    public void ToString_GivenTupleButEmptyDevider_ShouldThrowException()
    {
      //arrange
      (string key, string reference) input = ("invite", "Instance");
      //act

      //assert
      Assert.Throws<ArgumentException>(() => input.ToFormattedString(string.Empty));
    }

  }
}
