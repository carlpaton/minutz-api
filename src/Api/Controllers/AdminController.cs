using System.Collections.Generic;
using Api.Extensions;
using Api.Models;
using Interface;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minutz.Models.Entities;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Controllers
{
    public class AdminController : Controller
    {
        [Authorize]
        [HttpGet("api/admin/users")]
        public IActionResult GetUsers()
        {
            return Ok(new List<PersonViewModel>());
        }
        
        [Authorize]
        [HttpPut("api/admin/user")]
        public IActionResult CreateUser([FromBody] PersonViewModel person)
        {
            return Ok(person);
        }
        
        [Authorize]
        [HttpPost("api/admin/user")]
        public IActionResult UpdateUser([FromBody] PersonViewModel person)
        {
            return Ok(person);
        }

        [Authorize]
        [HttpDelete("api/admin/user")]
        public IActionResult DeleteUser(string email)
        {
            return Ok();
        }
        
        [Authorize]
        [HttpGet("api/admin/cal")]
        public IActionResult GetCal()
        {
            return Ok(10);
        }
        
    }
}