using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Interface.Services;
using System.Security.Claims;
using System.Linq;

namespace Api.Controllers
{
  [Route("api/[controller]")]
  public class UserController : Controller
  {
    private readonly IUserValidationService _userValidationService;
    public UserController(IUserValidationService userValidationService)
    {
      _userValidationService = userValidationService;
    }
    // GET api/values
    [Authorize]
    [HttpGet]
    public IEnumerable<string> Get()
    {
      var result = new List<string>();
      result.Add("Loaded");
      var user = User;
      var claims = User.Claims.ToList();
      if (claims.Any())
      {
        string userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
        result.Add(userId);
        if (!_userValidationService.IsNewUser(userId))
        {
          var role = _userValidationService.CreateAttendee(userId);
          result.Add("create");
          result.Add(role);
        }
      }
      return result;
    }

    //[SwaggerRequestExample(typeof(string), typeof(ValuesController))]
    // GET api/values/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
      return "value";
    }

    // POST api/values
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
