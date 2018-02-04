using System;
using Interface;
using System.Text.RegularExpressions;

namespace Core.Validation
{
    public class ValidationService : IValidationService
    {
        private const string _invalidEmailMessage = "Please provide valid email address.";
        private const string _validEmail = "Valid email.";
        public (bool condition, string message) ValidEmail (string input)
        {
            if (string.IsNullOrEmpty (input)) return (false, _invalidEmailMessage);

            var result = Regex.IsMatch (input,
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds (250));
            if (result) return (result, _validEmail);
            return (false, _invalidEmailMessage);
        }

        public (bool condition, string message) ValidPassword (string input)
        {
            throw new System.NotImplementedException ();
        }
    }
}