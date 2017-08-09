using System;

namespace minutz_interface.Services
{
	public interface ITokenStringService
	{
		DateTime ConvertTokenStringToDate(string tokenValue);
	}
}
