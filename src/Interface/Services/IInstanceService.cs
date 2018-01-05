using Minutz.Models.Entities;

namespace Interface.Services
{
	public interface IInstanceService
	{
		Instance SetInstanceDetailsForSchema(
			string token,
			Instance instance);
	}
}