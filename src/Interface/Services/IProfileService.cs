using System.Collections.Generic;
using System.Security.Claims;
using Models.ViewModels;

namespace Interface.Services
{
	public interface IProfileService
	{
		UserProfile GetFromClaims(IEnumerable<Claim> claims);
	}
}