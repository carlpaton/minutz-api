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
    private readonly IMeetingService _meetingService;
		private readonly IPersonService _personService;

		public MeetingController(
      ITokenStringHelper tokenStringHelper,
      IProfileService profileService,
      IInstanceService instanceService,
      IOptions<AppSettings> settings,
      IUserService userService, 
      IHostingEnvironment environment,
			IPersonService personService,
			IMeetingService meetingService
    ) : base(settings, profileService, tokenStringHelper, instanceService, userService)
    {
      _environment = environment;
      _meetingService = meetingService;
			_personService = personService;
    }

    public IActionResult Index()
    {
      this.UserProfile = User.ToProfile(ProfileService, TokenStringHelper, AppSettings);
      return View(new Meeting { });
    }

    public IActionResult Runner()
    {
      this.UserProfile = User.ToProfile(ProfileService, TokenStringHelper, AppSettings);
      return View("MeetingRunner",new Meeting { });
    }

    [HttpGet]
    [Authorize]
    public JsonResult GetMeetings()
    {
      var user = this.ProfileService.GetFromClaims(User.Claims, TokenStringHelper, AppSettings);
      var schema = user.InstanceId.ToSchemaString();
      var data = _meetingService.Get(schema, user);
      return Json(data);
    }

    [HttpGet]
    [Authorize]
    public JsonResult GetMeeting(string id)
    {
      var user = this.ProfileService.GetFromClaims(User.Claims, TokenStringHelper, AppSettings);
      var schema = user.InstanceId.ToSchemaString();
      var data = _meetingService.Get(schema, new Meeting { Id =  Guid.Parse(id)  }, true);
      return Json(data);
    }

    [HttpPost]
    [Authorize]
    public JsonResult Save(Meeting meeting)
    {
      if (string.IsNullOrEmpty(meeting.Name))
        meeting.Name = GetName();

      if (meeting.Id == Guid.Empty)
        meeting.Id = Guid.NewGuid();
      var user = this.ProfileService.GetFromClaims(User.Claims, TokenStringHelper, AppSettings);
      var schema = user.InstanceId.ToSchemaString();

      if (meeting.MeetingOwnerId == null) meeting.MeetingOwnerId = user.UserId;

      if (meeting.MeetingAgendaCollection == null) meeting.MeetingAgendaCollection = new List<MeetingAgendaItem>();
      if (meeting.MeetingActionCollection == null) meeting.MeetingActionCollection = new List<ActionItem>();
      if (meeting.MeetingAttachmentCollection == null) meeting.MeetingAttachmentCollection = new List<MeetingAttachmentItem>();
      if (meeting.MeetingAttendeeCollection == null) meeting.MeetingAttendeeCollection = new List<MeetingAttendee>();
      if (meeting.MeetingNoteCollection == null) meeting.MeetingNoteCollection = new List<MeetingNoteItem>();

      _meetingService.Get(schema, meeting);
      return Json(meeting);
    }

		[HttpPost]
		[Authorize]
		public JsonResult DeleteAgenda(string agendaId)
		{
			var user = this.ProfileService.GetFromClaims(User.Claims, TokenStringHelper, AppSettings);
			var schema = user.InstanceId.ToSchemaString();
			_meetingService.DeleteAgenda(schema, agendaId);
			return Json(agendaId);
		}

    public ActionResult AttendeesControl(int meetingId)
    {
      var model = GetUsers();
      return PartialView("~/Views/Meeting/_attendeesControl.cshtml", model);
    }

    public ActionResult AgendaControl(int meetingId)
    {
      return PartialView("~/Views/Meeting/_agendaControl.cshtml");
    }

    public ActionResult AttachmentsControl(int meetingId)
    {
      return PartialView("~/Views/Meeting/_attachmentsControl.cshtml");
    }

    [HttpGet]
		[Authorize]
    public JsonResult Attendees(string meetingId)
    {
			var user = this.ProfileService.GetFromClaims(User.Claims, TokenStringHelper, AppSettings);
			var schema = user.InstanceId.ToSchemaString();
			var meeting = _meetingService.Get(schema, new Meeting { Id = Guid.Parse(meetingId) }, true);
			return Json(meeting.MeetingAttendeeCollection);
    }

		[Authorize]
		[HttpPost]
		public JsonResult FileUpload(string meetingId)
		{
			var user = this.ProfileService.GetFromClaims(User.Claims, TokenStringHelper, AppSettings);
			var schema = user.InstanceId.ToSchemaString();

			var uploads = Path.Combine(_environment.WebRootPath, "uploads");
			var uploadedFiles = new List<string>();
			foreach (var file in Request.Form.Files)
			{
				if (file.Length > 0)
				{
					using (var binaryReader = new BinaryReader(Request.Form.Files[0].OpenReadStream()))
					{
						byte[] result;
						result = binaryReader.ReadBytes((int)Request.Form.Files[0].Length);
						//_meetingService.SaveFile(AppSettings.ConnectionStrings.AzureConnection, schema, user, file.FileName, result, meetingId);
					}
					uploadedFiles.Add(file.FileName);

					//using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
					//     {
					//	await file.CopyToAsync(fileStream);

					//     }
				}
			}
			return Json(uploadedFiles);
		}

		private string GetName()
    {
      return "MeetingName" + new Random().Next();
    }

    private List<User> GetUsers()
    {
      var result = new List<User>();
      result.Add(new models.Entities.User
      {
        Id = 1,
        Active = true,
        Email = "mauro@minutz.co",
        FirstName = "mauro",
        FullName = "mauro@minutz.co",
        Identity = "auth0|58cfe130bdaeed709e2d246e",
        LastName = "",
        Image = "https://s.gravatar.com/avatar/76fc2d2d84214a0ea020016aef705aae?s=480&r=pg&d=https%3A%2F%2Fcdn.auth0.com%2Favatars%2Fma.png",
        Role = "Attendee"
      });

      result.Add(new models.Entities.User
      {
        Id = 2,
        Active = true,
        Email = "j3noriginal@gmail.com",
        FirstName = "Jani",
        FullName = "Jani Rostoll",
        Identity = "google-oauth2|113403253120477542556",
        LastName = "Rostoll",
        Image = "https://lh6.googleusercontent.com/-tlQZis8Q8R4/AAAAAAAAAAI/AAAAAAAAAAA/wPMXfr1sEw0/photo.jpg",
        Role = "Attendee"
      });

      result.Add(new models.Entities.User
      {
        Id = 3,
        Active = true,
        Email = "leeroya@outlook.com",
        FirstName = "",
        FullName = "leeroya@outlook.com",
        Identity = "auth0|58f14a8bb21f0766553879ec",
        LastName = "",
        Image = "https://s.gravatar.com/avatar/169fb08d4870f51a1f201e4a63b0b653?s=480&r=pg&d=https%3A%2F%2Fcdn.auth0.com%2Favatars%2Fle.png",
        Role = "Attendee"
      });

      result.Add(new models.Entities.User
      {
        Id = 4,
        Active = true,
        Email = "leatitia.ashworth@gmail.com",
        FirstName = "Leatitia",
        FullName = "Leatitia Ashworth",
        Identity = "google-oauth2|117399881719269136104",
        LastName = "Ashworth",
        Image = "https://lh5.googleusercontent.com/-tniTuVQcOnc/AAAAAAAAAAI/AAAAAAAAMGU/aCX3BxWOuxA/photo.jpg",
        Role = "Attendee"
      });

      return result;
    }
  }
}