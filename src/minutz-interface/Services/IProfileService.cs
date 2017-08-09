using minutz_interface.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;

namespace minutz_interface.Services
{
	public interface IProfileService
	{
		IUserProfile GetFromClaims(IEnumerable<Claim> claims);
	}
}