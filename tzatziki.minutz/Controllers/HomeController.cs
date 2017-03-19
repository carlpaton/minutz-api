using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tzatziki.minutz.interfaces;


namespace tzatziki.minutz.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private readonly ITokenStringHelper _tokenStringHelper;
		private readonly IProfileService _profileService;
		public HomeController(ITokenStringHelper tokenStringHelper, IProfileService profileService)
		{
			_tokenStringHelper = tokenStringHelper;
			_profileService = profileService;
		}


		/// <summary>
		/// This the Calender View Page
		/// </summary>
		/// <returns></returns>
		public IActionResult Index()
		{

			var claims = User.Claims.ToList();
			var model = _profileService.GetFromClaims(claims,_tokenStringHelper);
			 
			return View(model);
		}
		 
		public IActionResult Error()
		{
			return View();
		}
	}
}
