using System.Linq;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
  public class SignUpController : Controller
  {
    private readonly IUserValidationService _userValidationService;
    private readonly IAuthenticationService _authenticationService;
    private readonly IApplicationManagerService _applicationManagerService;
    public SignUpController(IUserValidationService userValidationService,
      IAuthenticationService authenticationService,
      IApplicationManagerService applicationManagerService)
    {
      _userValidationService = userValidationService;
      _authenticationService = authenticationService;
      _applicationManagerService = applicationManagerService;
    }
    /// <summary>
    /// Use this to start the full version.
    /// </summary>
    /// <returns>If it was successful or not</returns>
    [Authorize]
    [HttpPost("api/Signup")]
    public IActionResult Post()
    {
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      var userInfo = _authenticationService.GetUserInfo(token);
      var person = _userValidationService.GetUser(userInfo.Sub);
      var result = _applicationManagerService.StartFullVersion(person);
      if (result.condition)
        return Ok();
      return StatusCode(500, result.message);
    }

    [Authorize]
    [HttpPost("api/reset")]
    public IActionResult ResetAccount()
    {
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      var userInfo = _authenticationService.GetUserInfo(token);
      var person = _userValidationService.GetUser(userInfo.Sub);

      var result = _applicationManagerService.StartFullVersion(person);
      if (result.condition)
        return Ok();
      return StatusCode(500, result.message);
    }
  }
}