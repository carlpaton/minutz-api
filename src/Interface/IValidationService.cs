namespace Interface
{
    public interface IValidationService
    {
        string InvalidEmailMessage { get; }
        
        string ValidEmailMessage { get; }

        string ValidPasswordMessage { get; }

        string InvalidPasswordMessage { get; }

        string InvalidPasswordStrengthMessage { get; }

        (bool condition, string message) ValidEmail (string input);

        (bool condition, string message) ValidPassword (string input);
    }
}