using Models.Auth0Models;

namespace Interface.Repositories
{
    public interface IAuth0Repository
    {
        (bool condition, string message, UserResponseModel tokenResponse) CreateToken (
            string username, string password);
    }
}