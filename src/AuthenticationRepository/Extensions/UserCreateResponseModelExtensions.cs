using Models;
using Models.Auth0Models;
namespace AuthenticationRepository.Extensions
{
    public static class UserCreateResponseModelExtensions
    {
        public static UserCreateResponseModel ToUserCreateResponseModelModel(this string resultString)
        {
           return Newtonsoft.Json.JsonConvert.DeserializeObject<UserCreateResponseModel>(resultString);
        }
    }
}