using System;

namespace Models.ViewModels
{
    public class UserProfile 
	{
		public string UserId { get; set; }

		public string ClientID { get; set; }

		public DateTime Created_At { get; set; }

		public DateTime Updated_At { get; set; }

		public string EmailAddress { get; set; }

		public string Name { get; set; }

		public bool Active { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public Guid InstanceId { get; set; }

		public string ProfileImage { get; set; }

		public AppMetadata App_Metadata { get; set; }

		public string Role { get; set; }
	}
}