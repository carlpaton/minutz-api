using System;
using tzatziki.minutz.interfaces;

namespace tzatziki.minutz.core
{
	public class TokenStringHelper : ITokenStringHelper
	{
		public DateTime ConvertTokenStringToDate(string tokenValue)
		{
			if (string.IsNullOrEmpty(tokenValue))
				throw new ArgumentNullException($"The value supplied: {tokenValue} ,please provide a valid string.");
			var firstTrim = tokenValue.Remove(0, 1);
			var final = firstTrim.Remove((firstTrim.Length - 1), 1);
			DateTime result;
			var trueDate = DateTime.TryParse(final, out result);
			if (!trueDate)
				throw new BadImageFormatException("The input string is not a valid date.");
			return DateTime.Parse(final);
		}
	}
}
