using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Net.Http;
using System.Linq;
using System.Net;
using System;

namespace minutz.Controllers
{
    [Route("api/[controller]")]
    public class MeetingController : Controller
    {

        [HttpGet("[action]")]
        public JsonResult Get(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
               return Json(new { status = false, message = "oops got it wrong"});
            }
            //var response = new HttpResponseMessage();
            
            
            return Json(new { status = true, message = "boo got it right."});
        }
    }
}