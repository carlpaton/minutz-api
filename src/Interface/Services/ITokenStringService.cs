using System;

namespace Interface.Services
{
	public interface ITokenStringService
	{
		DateTime ConvertTokenStringToDate(string tokenValue);
	}
}
