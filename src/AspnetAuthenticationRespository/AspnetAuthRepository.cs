using System;
using Interface.Repositories;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace AspnetAuthenticationRespository
{
    public class AspnetAuthRepository : IAuthRepository
    {
        public (bool condition, string message, AuthRestModel value) CreateUser
            (string name, string email, string password, string role, string instanceId)
        {
            throw new NotImplementedException();
        }

        public AuthRestModelResponse GetUserInfo(string token)
        {
            throw new NotImplementedException();
        }

        public TokenResponse CreateToken(string username, string password)
        {
            throw new NotImplementedException();
        }

        public AuthUserQueryResponse SearchUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public (bool condition, string message, bool value) ValidateUser(string email)
        {
            throw new NotImplementedException();
        }
    }
}