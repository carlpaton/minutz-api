using Microsoft.AspNetCore.Builder;

namespace minutz_interface.Services
{
	public interface IAuth0OptionsService
	{
		OpenIdConnectOptions GetOptions();
	}
}