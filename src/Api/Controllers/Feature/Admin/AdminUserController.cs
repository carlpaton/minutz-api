using System.Collections.Generic;
using Api.Extensions;
using Api.Models;
using Interface.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minutz.Models.Entities;

namespace Api.Controllers.Feature.Admin
{
    public class AdminUserController : Controller
    {
        private readonly IInstanceUserService _instanceUserService;

        public AdminUserController(IInstanceUserService instanceUserService)
        {
            _instanceUserService = instanceUserService;
        }

        [Authorize]
        [HttpGet("api/feature/admin/users")]
        public IActionResult GetUsers()
        {
            var user = User.ToRest();
            var people = _instanceUserService.GetInstancePeople(user);
            return people.Condition ? Ok(people) : StatusCode(people.Code, people.Message);
        }

        [Authorize]
        [HttpPut("api/feature/admin/user")]
        public IActionResult CreateUser([FromBody] PersonViewModel person)
        {
            var user = User.ToRest();
            var data = new Person
                       {
                           Active = true, 
                           Email = person.Email,
                           Identityid = person.Email,
                           FirstName = person.FirstName,
                           LastName = person.LastName,
                           FullName = person.Name,
                           Role = person.Role
                       };
            var personResult = _instanceUserService.AddInstancePerson(data,user);
            return personResult.Condition ? Ok(personResult.Person) : StatusCode(personResult.Code, personResult.Message);
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