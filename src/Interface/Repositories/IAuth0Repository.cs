using Minutz.Models.Auth0Models;
using Minutz.Models.Entities;
using Models.Auth0Models;

namespace Interface.Repositories
{
    public interface IAuth0Repository
    {
        (bool condition, string message, AuthRestModel value) CreateUser (
            string name, string username,string email, string password, string role, string instanceId);

        (bool condition, string message, AuthRestModel infoResponse) GetUserInfo (
            string token);
        (bool condition, string message, UserResponseModel tokenResponse) CreateToken (
            string username, string password);

        (bool condition, string message, UserQueryModel value) SearchUserByEmail
            (string email);

        (bool condition, string message, bool value) ValidateUser
            (string email);
    }
}