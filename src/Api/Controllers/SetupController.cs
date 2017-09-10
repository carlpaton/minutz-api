using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class SetupController : Controller
    {
        [HttpPost]
        public bool Install()
        {
            return true; ;
        }
    }
}
