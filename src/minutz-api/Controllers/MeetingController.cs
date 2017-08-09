using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using minutz_interface.Repositories;
using System.Net.Http;
using minutz_interface.Entities;

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

		// GET api/values/5
		[HttpGet("{id}")]
		public string Get(int id)
		{
			return "value";
		}

		// POST api/values
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
