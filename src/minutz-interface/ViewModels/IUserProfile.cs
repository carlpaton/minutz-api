using System;

namespace minutz_interface.ViewModels
{
	public interface IUserProfile
	{
		string UserId { get; set; }

		string ClientID { get; set; }

		DateTime Created_At { get; set; }

		DateTime Updated_At { get; set; }

		string EmailAddress { get; set; }

		string Name { get; set; }

		bool Active { get; set; }

		string FirstName { get; set; }

		string LastName { get; set; }

		Guid InstanceId { get; set; }

		string ProfileImage { get; set; }

		IAppMetadata App_Metadata { get; set; }

		string Role { get; set; }
	}
}
