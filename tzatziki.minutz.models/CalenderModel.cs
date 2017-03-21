
using System.Collections.Generic;
using tzatziki.minutz.models.Entities;

namespace tzatziki.minutz.models
{
	public class CalenderModel : BaseModel
	{
		public IEnumerable<Instance> Instances { get; set; }
	}
}
