using System.Collections.Generic;
using System.Security.Claims;

namespace AspnetAuthenticationRespository.Interfaces
{
    public interface IMinutzClaimManager
    {
        List<Claim> CreateClaims
            (string email, string picture, string name, IEnumerable<string> roles, string instanceId, string userId);
    }
}