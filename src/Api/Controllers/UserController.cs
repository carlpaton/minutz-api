using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Interface.Services;
using System.Security.Claims;
using System.Linq;
using Models.Entities;

namespace Api.Controllers
{
  [Route("api/[controller]")]
  public class UserController : Controller
  {
    private readonly IUserValidationService _userValidationService;
    private readonly IAuthenticationService _authenticationService;
    public UserController(IUserValidationService userValidationService, 
                          IAuthenticationService authenticationService)
    {
      _userValidationService = userValidationService;
      _authenticationService = authenticationService;
    }
    [Authorize]
    [HttpGet]
    public AuthRestModel Get()
    {
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      var userInfo = _authenticationService.GetUserInfo(token);
      if (!_userValidationService.IsNewUser(userInfo.sub))
        _userValidationService.CreateAttendee(userInfo);
      return _userValidationService.GetUser(userInfo.sub);
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
      return "value";
    }

    //Create User
    [HttpPost]
    public void Post([FromBody]string value)
    {
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody]string value)
    {
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}
