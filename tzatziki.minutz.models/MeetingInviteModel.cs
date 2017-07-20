using tzatziki.minutz.models.Auth;
using tzatziki.minutz.models.Entities;

namespace tzatziki.minutz.models
{
	public class MeetingInviteModel
	{
		public UserProfile Person { get; set; }

		public Meeting Meeting { get; set; }
	}
}
