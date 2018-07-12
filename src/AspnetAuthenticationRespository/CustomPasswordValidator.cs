using System;
using System.Text.RegularExpressions;
using Interface;
using Minutz.Models;

namespace AspnetAuthenticationRepository
{
    public class CustomPasswordValidator: ICustomPasswordValidator
    {
        public PasswordScore CheckStrength(string password)
        {
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));
            int score = 3;

            if (password.Length < 1)
                return PasswordScore.Blank;
            if (password.Length < 7)
                return PasswordScore.VeryWeak;
            if (password.Length >= 8)
                score++;
            if (password.Length >= 12)
                score++;
            if (Regex.Match(password, @"/\d+/", RegexOptions.ECMAScript).Success)
                score++;
            if (Regex.Match(password, @"/[a-z]/", RegexOptions.ECMAScript).Success &&
                Regex.Match(password, @"/[A-Z]/", RegexOptions.ECMAScript).Success)
                score++;
            if (Regex.Match(password, @"/.[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]/", RegexOptions.ECMAScript).Success)
                score++;

            return (PasswordScore)score;
        }
        
    }
}