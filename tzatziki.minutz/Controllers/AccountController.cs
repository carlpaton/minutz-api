using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using tzatziki.minutz.interfaces;
using tzatziki.minutz.models;

namespace tzatziki.minutz.Controllers
{
	public class AccountController : BaseController
	{
 

    public AccountController(
      ITokenStringHelper tokenStringHelper,
      IProfileService profileService, 
      IOptions<AppSettings> settings) : base(settings, profileService, tokenStringHelper)
    {
   
    }
    
    [Authorize]
    public IActionResult Index()
		{
      this.UserProfile = User.ToProfile(ProfileService, TokenStringHelper, AppSettings);

      var model = new CalenderModel
      {
        //Instances = _instanceRepository.GetInstances(AppSettings.ConnectionStrings.LiveConnection),
        
      };
      return View(model);
    }

		public IActionResult Login(string returnUrl = "/")
		{
			return new ChallengeResult("Auth0", new AuthenticationProperties() { RedirectUri = returnUrl });
		}

		[Authorize]
		public async Task Logout()
		{
			await HttpContext.Authentication.SignOutAsync("Auth0", new AuthenticationProperties
			{
				// Indicate here where Auth0 should redirect the user after a logout.
				// Note that the resulting absolute Uri must be whitelisted in the 
				// **Allowed Logout URLs** settings for the client.
				RedirectUri = Url.Action("Index", "Home")
			});
			await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
		}

		/// <summary>
		/// This is just a helper action to enable you to easily see all claims related to a user. It helps when debugging your
		/// application to see the in claims populated from the Auth0 ID Token
		/// </summary>
		/// <returns></returns>
		[Authorize]
		public IActionResult Claims()
		{
			return View();
		}
	}
}