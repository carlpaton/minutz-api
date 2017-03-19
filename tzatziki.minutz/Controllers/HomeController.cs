using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tzatziki.minutz.Models.Auth;

namespace tzatziki.minutz.Controllers
{
	public class HomeController : Controller
	{
		[Authorize]
		public IActionResult Index()
		{
			var claims = User.Claims.ToList();
			var model = new UserProfileViewModel
			{
				Name = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value,
				EmailAddress = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
				ProfileImage = User.Claims.FirstOrDefault(c => c.Type == "picture")?.Value
			};

			

			return View(model);
		}

		public IActionResult About()
		{
			ViewData["Message"] = "Your application description page.";

			return View();
		}

		public IActionResult Contact()
		{
			ViewData["Message"] = "Your contact page.";

			return View();
		}

		public IActionResult Error()
		{
			return View();
		}
	}
}
