using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class MeetingController : Controller
    {
        [HttpGet]
        public Models.ViewModels.Meeting Get()
        {
            return new Models.ViewModels.Meeting();
        }
    }
}
