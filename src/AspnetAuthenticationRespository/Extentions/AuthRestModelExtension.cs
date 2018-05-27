using Minutz.Models.Entities;

namespace AspnetAuthenticationRespository.Extentions
{
    public static class AuthRestModelExtension
    {
        public static AuthRestModel ToModel
            (this Person person, bool isVerfied, string userId , string tokenExpires, string token)
        {
            var result  = new AuthRestModel();
            result.Sub = userId;
            result.Company = person.Company;
            result.Email = person.Email;
            result.FirstName = person.FirstName;
            result.IsVerified = isVerfied;
            result.InstanceId = person.InstanceId;
            result.LastName = person.LastName;
            result.Name = person.FullName;
            result.Nickname = person.FullName;
            result.Picture = person.ProfilePicture;
            result.Role = person.Role;
            result.TokenExpire = tokenExpires;
            result.IdToken = token;
            result.AccessToken = token;
            return result;
        }
    }
}