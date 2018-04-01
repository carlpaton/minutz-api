using System.Net.Http;
using System.Text;
using Minutz.Models.Auth0Models;

namespace Minutz.Models.Extensions
{
    public static class ManagmentApiTokenRequestExtensions
    {
        public static string ToJSON(this ManagmentApiTokenRequestModel jsonObject)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject (jsonObject);
        }
        
        public static StringContent StringContent(this string jsonObject)
        {
            return new StringContent (jsonObject, Encoding.UTF8, "application/json");
        }
    }
}