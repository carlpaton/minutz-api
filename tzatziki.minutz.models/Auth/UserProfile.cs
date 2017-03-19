using System;

namespace tzatziki.minutz.models.Auth
{
	public class UserProfile
	{
		public string UserId{ get; set; }

		public string ClientID{ get; set; }

		public DateTime Created_At{ get; set; }

		public DateTime Updated_At { get; set; }

		public string EmailAddress { get; set; }

		public string Name { get; set; }

		public string ProfileImage { get; set; }
	}
}
