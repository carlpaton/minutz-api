using Minutz.Models.Entities;
using Models.Auth0Models;

namespace Minutz.Models.Extensions
{
    public static class AuthRestModelExtentions
    {
        public static AuthRestModel UpdateFromInstance
            (this AuthRestModel model, Person person, Instance instance)
        {
            model.InstanceId = instance.Username;
            model.Company = instance.Company;
            model.Related = person.Related;
            model.Role = person.Role;
            model.FirstName = person.FirstName;
            model.LastName = person.LastName;
            model.Email = person.Email;
            return model;
        }

        public static AuthRestModel UpdateTokenInfo
            (this AuthRestModel model, UserResponseModel tokenResponseModel)
        {
            model.IdToken = tokenResponseModel.id_token;
            model.AccessToken = tokenResponseModel.access_token;
            model.TokenExpire = tokenResponseModel.expires_in;
            return model;
        }
    }
}