using System.Linq;
using Microsoft.AspNetCore.Mvc;
using tzatziki.minutz.interfaces;

namespace tzatziki.minutz.Controllers
{
	public class MeetingController : Controller
	{
		private readonly ITokenStringHelper _tokenStringHelper;
		private readonly IProfileService _profileService;
		public MeetingController(ITokenStringHelper tokenStringHelper, IProfileService profileService) 
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
