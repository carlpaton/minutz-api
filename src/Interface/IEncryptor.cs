namespace Interface
{
    public interface IEncryptor
    {
        string EncryptString(string text);
        
        string EncryptString(string text, string keyString);

        string DecryptString(string cipherText);

        string DecryptString(string cipherText, string keyString);
    }
}