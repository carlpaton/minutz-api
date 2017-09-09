using Interface.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;

namespace Interface.Services
{
	public interface IProfileService
	{
		IUserProfile GetFromClaims(IEnumerable<Claim> claims);
	}
}