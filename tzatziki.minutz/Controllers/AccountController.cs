using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using tzatziki.minutz.interfaces;
using tzatziki.minutz.models;
using tzatziki.minutz.models.Auth;
using System;
using tzatziki.minutz.core;
using tzatziki.minutz.models.Entities;
using tzatziki.minutz.Interfaces;

namespace tzatziki.minutz.Controllers
{
  public class AccountController : BaseController
  {
		private readonly IPersonService _personService;
		private readonly IViewRenderService _viewRenderService;

		public AccountController(
      ITokenStringHelper tokenStringHelper,
      IProfileService profileService,
      IInstanceService instanceService,
			IPersonService personService,
			IOptions<AppSettings> settings,
      IUserService userService, IViewRenderService viewRenderService) : base(settings, profileService, tokenStringHelper, instanceService, userService)
    {
			_personService = personService;
			_viewRenderService = viewRenderService;
		}

    [Authorize]
    public IActionResult Index()
    {
      this.UserProfile = User.ToProfile(ProfileService, TokenStringHelper, AppSettings);
      var user = this.ProfileService.GetFromClaims(User.Claims, TokenStringHelper, AppSettings);

      var model = new CalenderModel
      {
        //Instances = _instanceRepository.GetInstances(AppSettings.ConnectionStrings.LiveConnection),
      };
      return View(user);
    }

    public IActionResult Login(string returnUrl = "/Home")
    {
      return new ChallengeResult("Auth0", new AuthenticationProperties() { RedirectUri = returnUrl });
    }

    [Authorize]
    public async Task Logout()
    {
      await HttpContext.Authentication.SignOutAsync("Auth0", new AuthenticationProperties
      {
        // Indicate here where Auth0 should redirect the user after a logout. Note that the resulting
        // absolute Uri must be whitelisted in the
        // **Allowed Logout URLs** settings for the client.
        RedirectUri = Url.Action("Index", "Home")
      });
      await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    /// <summary>
    /// This is just a helper action to enable you to easily see all claims related to a user. It
    /// helps when debugging your application to see the in claims populated from the Auth0 ID Token
    /// </summary>
    /// <returns></returns>
    [Authorize]
    public IActionResult Claims()
    {
      return View();
    }

		[HttpGet]
		[Authorize]
		public JsonResult People()
		{
			var data = _personService.GetSystemUsers(Environment.GetEnvironmentVariable("SQLCONNECTION"));
			return Json(data);
		}

		[HttpGet]
		[Authorize]
		public JsonResult AccountPeople()
		{
			var user = this.ProfileService.GetFromClaims(User.Claims, TokenStringHelper, AppSettings);
			var schema = user.InstanceId.ToSchemaString();
			var data = _personService.GetSchemaUsers(Environment.GetEnvironmentVariable("SQLCONNECTION"),schema);
			return Json(data);
		}

		[HttpPost]
		[Authorize]
		public JsonResult InvitePerson(Person person)
		{
			var user = this.ProfileService.GetFromClaims(User.Claims, TokenStringHelper, AppSettings);
			var schema = user.InstanceId.ToSchemaString();
			var data = _personService.InvitePerson(person, 
																						 GetInviteMessage(person),
																						 Environment.GetEnvironmentVariable("SQLCONNECTION"),schema);
			return Json(data);
		}

		[HttpPost]
		[Authorize]
		public JsonResult MeetingInvitePerson(Person person)
		{
			var user = this.ProfileService.GetFromClaims(User.Claims, TokenStringHelper, AppSettings);
			var schema = user.InstanceId.ToSchemaString();
			var data = _personService.InvitePerson(person,
																						 GetInviteMessage(person),
																						 Environment.GetEnvironmentVariable("SQLCONNECTION"), schema);
			return Json(data);
		}

		public IActionResult SaveProfile(UserProfile model)
    {
      return View("Index", model);
    }

		public string GetInviteMessage(Person person)
		{
			return _viewRenderService.RenderToStringAsync("EmailMessage/InvitePerson", person).Result;
		}

		public string GetMeetingInvite(Person person)
		{
			return _viewRenderService.RenderToStringAsync("EmailMessage/MeetingInvite", person).Result;
		}

		[Authorize]
    public ActionResult _userProfileForm()
    {
      var user = this.ProfileService.GetFromClaims(User.Claims, TokenStringHelper, AppSettings);
      return PartialView("~/Views/Account/_userProfileForm.cshtml", user);
    }

    [Authorize]
    public ActionResult _userProfileActions()
    {
      return PartialView("~/Views/Account/_userProfileActions.cshtml");
    }

    [Authorize]
    public ActionResult _userProfileActivityLog()
    {
      return PartialView("~/Views/Account/_userProfileActivityLog.cshtml");
    }

    [Authorize]
    public ActionResult StartFullVersion()
    {
      var user = this.ProfileService.GetFromClaims(User.Claims, TokenStringHelper, AppSettings);
      user.InstanceId = Guid.NewGuid();
      user = this.ProfileService.Update(user, AppSettings);
      var instance = this.Instanceservice.Get(user, Environment.GetEnvironmentVariable("SQLCONNECTION"));
      var instanceUser = this.UserService.CopyPersonToUser(user, AppSettings);
			return RedirectToAction("Logout", "Account");
		}

    [Authorize]
    public ActionResult ResetAccount()
    {
      var user = this.ProfileService.GetFromClaims(User.Claims, TokenStringHelper, AppSettings);
      var schema = user.InstanceId.ToSchemaString();
      var instanceUser = this.UserService.ResetAccount(user, AppSettings);
      return RedirectToAction("Logout", "Account");
    }
  }
}