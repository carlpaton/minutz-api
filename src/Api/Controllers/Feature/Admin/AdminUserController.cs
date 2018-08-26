using System.Collections.Generic;
using Api.Extensions;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Feature.Admin
{
    public class AdminUserController : Controller
    {
        public AdminUserController()
        {
        }

        [Authorize]
        [HttpGet("api/feature/admin/users")]
        public IActionResult GetUsers()
        {
            var user = User.ToRest();
            return Ok(new List<PersonViewModel>());
        }

        [Authorize]
        [HttpPut("api/feature/admin/user")]
        public IActionResult CreateUser([FromBody] PersonViewModel person)
        {
            return Ok(person);
        }

        [Authorize]
        [HttpPost("api/feature/admin/user")]
        public IActionResult UpdateUser([FromBody] PersonViewModel person)
        {
            return Ok(person);
        }

        [Authorize]
        [HttpDelete("api/feature/admin/user")]
        public IActionResult DeleteUser(string email)
        {
            return Ok();
        }
    }
}