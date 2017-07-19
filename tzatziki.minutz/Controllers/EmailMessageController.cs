using tzatziki.minutz.models.Entities;
using tzatziki.minutz.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace tzatziki.minutz.Controllers
{
	public class EmailMessageController : Controller
	{
		private readonly IViewRenderService _viewRenderService;
		public EmailMessageController(IViewRenderService viewRenderService)
		{
			_viewRenderService = viewRenderService;
		}

		public IActionResult InvitePerson()
		{
			var person = new Person { FirstName = "Angelica", LastName = "Ashworth", FullName= "Angelica Ashworth", Role = "Attendee" };
			return View(person);
		}

		public IActionResult MeetingInvite()
		{
			var person = new Person { FirstName = "Angelica", LastName = "Ashworth", FullName = "Angelica Ashworth", Role = "Attendee" };
			return View(person);
		}
	}
}