using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Repositories
{
    public interface IAuthRepository
    {
        (bool condition, string message, AuthRestModel value) CreateUser (
            string name,string email, string password, string role, string instanceId);

        AuthRestModelResponse GetUserInfo 
            (string token);
        
        TokenResponse CreateToken (
            string username, string password);

        AuthUserQueryResponse SearchUserByEmail
            (string email);

        (bool condition, string message, bool value) ValidateUser
            (string email);
    }
}