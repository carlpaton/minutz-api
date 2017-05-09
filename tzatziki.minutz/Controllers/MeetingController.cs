using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using tzatziki.minutz.interfaces;
using tzatziki.minutz.models;
using tzatziki.minutz.core;
using tzatziki.minutz.models.Entities;
using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace tzatziki.minutz.Controllers
{
  [Authorize]
  public class MeetingController : BaseController
  {
    private IHostingEnvironment _environment;

    public MeetingController(
      ITokenStringHelper tokenStringHelper,
      IProfileService profileService,
      IInstanceService instanceService,
      IOptions<AppSettings> settings,
      IUserService userService, IHostingEnvironment environment) : base(settings, profileService, tokenStringHelper, instanceService, userService)
    {
      _environment = environment;
    }

    public IActionResult Index()
    {
      this.UserProfile = User.ToProfile(ProfileService, TokenStringHelper, AppSettings);
      return View(new Meeting { });
    }

    [HttpPost]
    public JsonResult Save(Meeting meeting)
    {
      if (string.IsNullOrEmpty(meeting.Name))
        meeting.Name = GetName();
      if (meeting.Id < 1)
        meeting.Id = GetId();

      return Json(meeting);
    }

    [HttpPost]
    public async Task<JsonResult> FileUpload()
    {
      var uploads = Path.Combine(_environment.WebRootPath, "uploads");
      var uploadedFiles = new List<string>();
      foreach (var file in Request.Form.Files)
      {
        if (file.Length > 0)
        {
          using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
          {
            await file.CopyToAsync(fileStream);
            uploadedFiles.Add(file.FileName);
          }
        }
      }
      return Json(uploadedFiles);
    }

    private string GetName()
    {
      return "MeetingName" + new Random().Next();
    }

    private int GetId()
    {
      return new Random().Next();
    }
  }
}