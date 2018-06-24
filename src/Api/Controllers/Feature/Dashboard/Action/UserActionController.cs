using Api.Extensions;
using Interface.Services.Feature.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Feature.Dashboard.Action
{
    public class UserActionController : Controller
    {
        private readonly IUserActionsService _userActionsService;

        public UserActionController(IUserActionsService userActionsService)
        {
            _userActionsService = userActionsService;
        }

        [Authorize]
        [HttpGet("api/feature/dashboard/useractions", Name = "User Actions List")]
        public IActionResult UserMeetingsResult()
        {
            var userInfo = User.ToRest();
            var result = _userActionsService.Actions(userInfo);
            if (result.Condition)
            {
                return Ok(result.Actions);
            }
            return StatusCode(result.Code, result.Message);
        }
    }
}