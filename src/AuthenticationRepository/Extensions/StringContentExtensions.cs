using System.Net.Http;
using System.Text;

namespace AuthenticationRepository.Extensions
{
    public static class StringContentExtensions
    {
        public static StringContent ToStringContent (
            this string jsonString)
        {
            return new StringContent (jsonString, Encoding.UTF8, "application/json");
        }
    }
}