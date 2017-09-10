using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Interface.Services;
using System.Linq;

namespace Api.Controllers
{
  [Route("api/[controller]")]
  public class SignUpController : Controller
  {
    private readonly IUserValidationService _userValidationService;
    private readonly IAuthenticationService _authenticationService;
    public SignUpController(IUserValidationService userValidationService,
                          IAuthenticationService authenticationService)
    {
      _userValidationService = userValidationService;
      _authenticationService = authenticationService;
    }
    /// <summary>
    /// Use this to start the full version.
    /// </summary>
    /// <returns>If it was successful or not</returns>
    [Authorize]
    [HttpPost]
    public bool Post()
    {
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      var userInfo = _authenticationService.GetUserInfo(token);
      var person = _userValidationService.GetUser(userInfo.sub);
      return false;
    }
  }
}
