using Minutz.Models;

namespace Interface
{
    public interface ICustomPasswordValidator
    {
        PasswordScore CheckStrength(string password);
    }
}