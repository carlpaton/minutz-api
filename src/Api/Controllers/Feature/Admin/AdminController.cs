using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Feature.Admin
{
    public class AdminController : Controller
    {
        [Authorize]
        [HttpGet("api/admin/cal")]
        public IActionResult GetCal()
        {
            return Ok(10);
        }
        
    }
}