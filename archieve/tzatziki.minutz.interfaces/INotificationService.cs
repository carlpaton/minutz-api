namespace tzatziki.minutz.interfaces
{
	public interface INotificationService
	{
		bool InvitePerson(string email, string message, IHttpService httpService);
	}
}
