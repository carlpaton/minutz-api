using System;

namespace Minutz.Models.Auth0Models
{
    public class TokenCacheModel
    {
        public string Token { get; set; }
        public DateTime Expire { get; set; }
    }
}