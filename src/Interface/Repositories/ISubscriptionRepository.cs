using System.Collections.Generic;
using Minutz.Models.Entities;

namespace Interface.Repositories
{
	public interface ISubscriptionRepository
	{
		List<Subscription> GetList(
			string schema,
			string connectionString);

		Subscription GetSubscription(
			string schema,
			string connectionString);

		Subscription SetSubscriptionTypeForSchema(
			string schema,
			string connectionString,
			int subscriptionId,
			string instanceId);
	}
}