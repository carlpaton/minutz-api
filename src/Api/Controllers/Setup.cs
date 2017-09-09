using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class Setup
    {
        [HttpPost]
        public bool Install()
        {
            return true; ;
        }
    }
}
