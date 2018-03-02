using System.Net.Http;
using System.Text;
using Models.Auth0Models;

namespace AuthenticationRepository.Extensions
{
    public static class UserTokenRequestModelExtensions
    {
        public static string ToJSON (
            this UserTokenRequestModel requestModel)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject (requestModel);
        }
    }
}