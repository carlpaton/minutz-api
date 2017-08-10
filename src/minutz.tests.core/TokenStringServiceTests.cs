using Microsoft.VisualStudio.TestTools.UnitTesting;
using minutz_interface.Services;
using minutz_core;
using System;

namespace minutz.tests.core
{
  [TestClass]
  public class TokenStringServiceTests
  {
    private readonly ITokenStringService _tokenStringService;
    public TokenStringServiceTests()
    {
      _tokenStringService = new TokenStringService();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ConvertTokenStringToDate_GivenEmptyString_ShouldThrowException()
    {
      var result = _tokenStringService.ConvertTokenStringToDate(string.Empty);
    }

    [TestMethod]
    [ExpectedException(typeof(BadImageFormatException))]
    public void ConvertTokenStringToDate_GivenBadString_ShouldThrowException()
    {
      var result = _tokenStringService.ConvertTokenStringToDate("foo");
    }
  }
}