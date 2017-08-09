using System.Collections.Generic;
using System.Security.Claims;

namespace minutz_interface.Services
{
	public interface IAuthService
	{
		void Getrole(ClaimsIdentity identity,List<KeyValuePair<string, string>> queries = null);
	}
}
