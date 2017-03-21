using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using tzatziki.minutz.interfaces;
using tzatziki.minutz.interfaces.Repositories;
using tzatziki.minutz.models;

namespace tzatziki.minutz.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private readonly ITokenStringHelper _tokenStringHelper;
		private readonly IProfileService _profileService;
		private AppSettings AppSettings { get; set; }
		private readonly IInstanceRepository _instanceRepository;

		public HomeController(ITokenStringHelper tokenStringHelper, IProfileService profileService, IInstanceRepository instanceRepository, IOptions<AppSettings> settings)
		{
			AppSettings = settings.Value;
			_tokenStringHelper = tokenStringHelper;
			_profileService = profileService;
			_instanceRepository = instanceRepository;
		}


		/// <summary>
		/// This the Calender View Page
		/// </summary>
		/// <returns></returns>
		public IActionResult Index()
		{

			var claims = User.Claims.ToList();

			var model = new CalenderModel
			{
				Instances = _instanceRepository.GetInstances(AppSettings.ConnectionStrings.LiveConnection),
				User = _profileService.GetFromClaims(claims, _tokenStringHelper)
			};
			return View(model);
		}

		public IActionResult Error()
		{
			return View();
		}
	}
}
