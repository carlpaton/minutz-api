using Api.Extensions;
using Interface;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minutz.Models.Entities;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace Api.Controllers
{
  public class UserController : Controller
  {
    private readonly IUserValidationService _userValidationService;
    private readonly IAuthenticationService _authenticationService;
    private readonly IValidationService _validationService;
    public UserController (
      IUserValidationService userValidationService,
      IAuthenticationService authenticationService,
      IValidationService validationService)
    {
      this._userValidationService = userValidationService;
      this._authenticationService = authenticationService;
      this._validationService = validationService;
    }

    [HttpPost]
    public IActionResult Login (string email, string password, string fullName)
    {
      var emailValidation = this._validationService.ValidEmail(email);
      if(!emailValidation.condition) return StatusCode(404,emailValidation.message);
      return Ok ();
    }

    [Authorize]
    [HttpGet ("api/user", Name = "Get the user profile")]
    [ProducesResponseType (typeof (string), 400)]
    [ProducesResponseType (typeof (AuthRestModel), 200)]
    [SwaggerResponse ((int) System.Net.HttpStatusCode.OK, Type = typeof (AuthRestModel))]
    public IActionResult Get (string reference)
    {
      if (!string.IsNullOrEmpty (reference))
      {
        if (reference.ToLower () == "none") reference = string.Empty;
      }

      var userInfo = _authenticationService.GetUserInfo (Request.Token ());
      if (!_userValidationService.IsNewUser (userInfo.Sub, reference))
        _userValidationService.CreateAttendee (userInfo, reference);

      var result = _userValidationService.GetUser (userInfo.Sub);
      return Ok (result);
    }
  }
}