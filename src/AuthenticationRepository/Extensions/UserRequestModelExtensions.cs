using AuthenticationRepository.Models;
using AuthRepo.Models;
using System.Net.Http;
using System.Text;

namespace AuthenticationRepository.Extensions
{
    public static class UserRequestModelExtensions
    {
        public static UserRequestModel Prepare (
            this UserRequestModel requestModel, string instanceId, string name, string role)
        {
            requestModel.user_metadata = new UserMetadata
            {
                instance = instanceId,
                name = name,
                role = role
            };
            return requestModel;
        }

        public static string ToJSON (
            this UserRequestModel requestModel)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject (requestModel);
        }
    }
}