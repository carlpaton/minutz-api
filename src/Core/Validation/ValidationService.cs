using System;
using Interface;
using System.Text.RegularExpressions;

namespace Core.Validation
{
    public class ValidationService : IValidationService
    {
        private enum passwordScore
        {
            Blank = 0,
            VeryWeak = 1,
            Weak = 2,
            Medium = 3,
            Strong = 5,
            VeryStrong = 6
        }

        public  string InvalidEmailMessage => "Please provide valid email address.";
        public  string ValidEmailMessage => "Valid email.";
        public  string ValidPasswordMessage => "Password is string enough.";
        public  string InvalidPasswordMessage => "Please provide a valid password";
        public  string InvalidPasswordStrengthMessage => "The password provided is not string enough, please provide a special character, one upper case and number.";
        public (bool condition, string message) ValidEmail (string input)
        {
            if (string.IsNullOrEmpty (input)) return (false, InvalidEmailMessage);
            var result = Regex.IsMatch (input,
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds (250));
            if (result) return (result, this.ValidEmailMessage);
            return (false, this.InvalidEmailMessage);
        }

        public (bool condition, string message) ValidPassword (string input)
        {
            if(string.IsNullOrEmpty(input)) 
                return (false, this.InvalidPasswordMessage);
            if((int)this.CheckingPasswordStrength(input)>= 5) 
                return (true, this.ValidPasswordMessage);
            return (false, this.InvalidPasswordStrengthMessage);
        }

        private passwordScore CheckingPasswordStrength (string password)
        {
            int score = 1;
            if (password.Length < 1)
                return passwordScore.Blank;
            if (password.Length < 4)
                return passwordScore.VeryWeak;

            if (password.Length >= 8)
                score++;
            if (password.Length >= 12)
                score++;
            if (Regex.IsMatch (password, @"[0-9]+(\.[0-9][0-9]?)?", RegexOptions.ECMAScript)) //number only //"^\d+$" if you need to match more than one digit.
                score++;
            if (Regex.IsMatch (password, @"^(?=.*[a-z])(?=.*[A-Z]).+$", RegexOptions.ECMAScript)) //both, lower and upper case
                score++;
            if (Regex.IsMatch (password, @"[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]", RegexOptions.ECMAScript)) //^[A-Z]+$
                score++;
            return (passwordScore) score;
        }
    }
}