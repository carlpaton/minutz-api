namespace Interface
{
    public interface IValidationService
    {
         (bool condition, string message) ValidEmail (string input);

         (bool condition, string message) ValidPassword (string input);
    }
}