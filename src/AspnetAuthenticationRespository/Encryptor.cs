using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Interface;

namespace AspnetAuthenticationRepository
{
    public class Encryptor : IEncryptor
    {
        private readonly string _key;
        
        public Encryptor()
        {
            _key = "E546C8DF278CD5931069B522E695D4F2";
        }

        public string EncryptString(string text)
        {
            return EncryptString(text, _key);
        }

        public string DecryptString(string cipherText)
        {
            return DecryptString(cipherText, _key);
        }

        public string EncryptString(string text, string keyString)
        {
            if(string.IsNullOrEmpty(keyString)) throw new ArgumentNullException(nameof(keyString));
            if(string.IsNullOrEmpty(text)) throw new ArgumentNullException(nameof(text));
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                if (aesAlg == null) throw new Exception("aesAlg is null");
                using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }

                        var iv = aesAlg.IV;

                        var decryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }
        
        public string DecryptString(string cipherText, string keyString)
        {
            if(string.IsNullOrEmpty(keyString)) throw new ArgumentNullException(nameof(keyString));
            if(string.IsNullOrEmpty(cipherText)) throw new ArgumentNullException(nameof(cipherText));
            
            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            var cipher = new byte[16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                if (aesAlg == null) throw new Exception("aesAlg is null");
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }
    }
}