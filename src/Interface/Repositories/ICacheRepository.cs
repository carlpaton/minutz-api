using Minutz.Models.Auth0Models;

namespace Interface.Repositories
{
    public interface ICacheRepository
    {
        bool CheckUserTokenCache
            (string userIdentifier, string connectionString);

        string GetUserTokenCache
            (string userIdentifier, string tokenJson, string connectionString);

        TokenCacheModel GetUserTokenCache
            (string userIdentifier, string connectionString);
    }
}