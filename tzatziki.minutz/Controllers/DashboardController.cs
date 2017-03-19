using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tzatziki.minutz.interfaces;

namespace tzatziki.minutz.Controllers
{
	[Authorize]
	public class DashboardController : Controller
	{
		private readonly ITokenStringHelper _tokenStringHelper;
		private readonly IProfileService _profileService;

		public DashboardController(ITokenStringHelper tokenStringHelper, IProfileService profileService)
		{
			_tokenStringHelper = tokenStringHelper;
			_profileService = profileService;
		}

		public IActionResult Index()
		{
			var claims = User.Claims.ToList();
			var model = _profileService.GetFromClaims(claims, _tokenStringHelper);
			return View(model);
		}
	}
}
