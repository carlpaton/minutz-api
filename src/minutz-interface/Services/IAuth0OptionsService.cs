using Microsoft.AspNetCore.Builder;

namespace Interface.Services
{
	public interface IAuth0OptionsService
	{
		OpenIdConnectOptions GetOptions();
	}
}