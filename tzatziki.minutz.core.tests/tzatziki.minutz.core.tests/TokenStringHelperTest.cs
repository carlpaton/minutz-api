using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using tzatziki.minutz.interfaces;

namespace tzatziki.minutz.core.tests
{
	[TestClass]
	public class TokenStringHelperTest
	{
		private readonly ITokenStringHelper _tokenStringHelper;
		private const string dateTestString =  "\"2017-03-18T08:41:51.9Z\"";
		private DateTime dateComparison = DateTime.Parse("2017-03-18T08:41:51.9Z");
		public TokenStringHelperTest()
		{
			_tokenStringHelper = new TokenStringHelper();
		}
    
    [TestMethod]
		public void ConvertTokenStringToDate_ShouldReturnDateTimeType()
		{
			var result = _tokenStringHelper.ConvertTokenStringToDate("2017-03-18T08:41:51.9Z");
			Assert.IsInstanceOfType(result, typeof(DateTime));
		}
    [TestMethod]
    [ExpectedException(typeof(BadImageFormatException))]
    public void ConvertTokenStringToDate_ShouldThrowBadImageFormatExceptionIfStringIsInvalid()
    {
      var result = _tokenStringHelper.ConvertTokenStringToDate("someDate");
      Assert.IsInstanceOfType(result, typeof(DateTime));
    }
    [TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConvertTokenStringToDate_ShouldThrowErrorExceptionIfEmptyStringSupplied() 
		{
			var result = _tokenStringHelper.ConvertTokenStringToDate(string.Empty);
		}

		[TestMethod]
		public void ConvertTokenStringToDate_ShouldReturnActualDate() 
		{
			var result = _tokenStringHelper.ConvertTokenStringToDate(dateTestString);
			Assert.AreEqual(result, dateComparison);
		}
	}
}
