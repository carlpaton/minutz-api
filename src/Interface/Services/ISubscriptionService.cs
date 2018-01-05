using System.Collections.Generic;
using Minutz.Models.Entities;

namespace Interface.Services
{
	public interface ISubscriptionService
	{
		List<Subscription> GetList(
			string token);

		Subscription GetSubscription(
			string token);

		Subscription SetSubscriptionForSchema(
			string token,
			int reminderId);
	}
}