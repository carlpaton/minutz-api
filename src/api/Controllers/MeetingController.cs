using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    public class MeetingController : Controller
    {
        [HttpGet]
        public minutz.models.ViewModels.Meeting Get()
        {
            return new minutz.models.ViewModels.Meeting();
        }
    }
}
