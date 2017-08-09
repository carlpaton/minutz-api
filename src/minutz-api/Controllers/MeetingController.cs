using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using minutz_interface.Repositories;
using System.Net.Http;
using minutz_interface.Entities;
using Microsoft.AspNetCore.Authorization;

namespace minutz_api.Controllers
{
	[Route("api/[controller]")]
	public class MeetingController : Controller
	{
		private readonly IInstanceRepository _instanceRepository;
		public MeetingController(IInstanceRepository instanceRepository)
		{
			_instanceRepository = instanceRepository;
		}

		// GET api/values
		[HttpGet]
		public List<IInstance> Get()
		{
			var connectionString = Environment.GetEnvironmentVariable("MINUTZ_CONNECTIONSTRING");
			var data = _instanceRepository.GetAll(connectionString).ToList();
			return data;
		}

		[Authorize]
		[HttpGet("{id}")]
		public string Get(int id)
		{
			return "value";
		}

		// bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6Ik5URkZRVEUwUXpsRk5qQXlSRVUxT0RJNFFUazFORU0yTnprM01qWXdRakJDT0RBNVFqWXdRUSJ9.eyJpc3MiOiJodHRwczovL3R6YXR6aWtpLW1pbnV0ei5hdXRoMC5jb20vIiwic3ViIjoiZ29vZ2xlLW9hdXRoMnwxMTc3ODUwNjc1MzYzMjA4MDcwNzEiLCJhdWQiOiJzcEVRdWNJT0ViU0J2a2l4cVJIemtLREhOWVJhR3kzSiIsImV4cCI6MTUwMjM0OTQ5OCwiaWF0IjoxNTAyMzEzNDk4LCJub25jZSI6InRObHBtSjYzdGZ4SjdSNm03LnpxdFYtenFjVW9MN3lNIiwiYXRfaGFzaCI6IlAwbXRSMUJNanZiSFRodnh5cWpGR2cifQ.BtS668sSY95wLIHDoId-d67dSAqBSVZBcDSJy_BStm45Z8XAJOvxSJIuy5Nb9pT0S88eZRjLzaWW01_2Qz3n9g2bGbZr7WiFcR7_zkfY2GOEo_wDX2a4QXmLLs3NPweaRCngabcyxjaq6C7HQwFybaxySdyilxG7ZX2jfWX64CxAG8-VirTwQuQspCBF4g8i5CsBJTbSmEOenGuHRt_lxOgeR2mUWrsO0DuZoKa_0Y5xgDkqzsApbEso8HMJ12rzMs_MOIwnosac0bhFmAY4wcQbg11CUKN8iOjYnbOmfFNEOWWDs3qF7mZrS1Ajq71LGvLDlWiwr2B3m4YliQUigA
		[HttpPost]
		public void Post([FromBody]string value)
		{
		}

		// PUT api/values/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE api/values/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
