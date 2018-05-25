using System;
using AspnetAuthenticationRespository;
using Microsoft.Extensions.DependencyModel;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class EncryptorFixture
    {
        private readonly string _keyInvalid;
        private readonly string _keyLongValid;
        private readonly string _keyValid;
        private readonly string _ssh;

        public EncryptorFixture()
        {
            _keyInvalid =  "foo";
            _keyLongValid =  "rmGkt0O4EBiqELicx%wPghb@2!#QmNJn#%MMg1X@T#pJlGjC6Ck3PfVsm7DRt@IyI#i40k6aNfiUdNQmD0CUVHCztdZCLu5uPAf1";
            _keyValid = "E546C8DF278CD5931069B522E695D4F2";
        }

        [Test]
        public void EncryptString_Given_EmptyTextString_ShouldThrowNullArgumentException()
        {
            var encryptor = new Encryptor();
            Assert.Throws<ArgumentNullException>(() => encryptor.EncryptString(string.Empty, _keyInvalid));
        }
        
        [Test]
        public void EncryptString_Given__KeyString_EmptyString_ShouldThrowNullArgumentException()
        {
            var encryptor = new Encryptor();
            Assert.Throws<ArgumentNullException>(() => encryptor.EncryptString("foo", string.Empty));
        }
        
        [Test]
        public void EncryptString_Given_AndShortKey__KeyString_String_ShouldThrowNullArgumentException()
        {
            var encryptor = new Encryptor();
            Assert.Throws<ArgumentException>(() => encryptor.EncryptString("foo", _keyLongValid));
        }
        
        [Test]
        public void EncryptString_Given_TextString_ShouldNotMatchInputString()
        {
            var encryptor = new Encryptor();
            var inputText = "@Nathan001";
            var outputText = encryptor.EncryptString(inputText, _keyValid);
            Assert.AreNotSame(inputText, outputText);
        }
        
        [Test]
        public void EncryptString_Given_TextString_Twice_ShouldNotMatch()
        {
            var encryptor = new Encryptor();
            var inputText = "@Nathan001";
            var outputText1 = encryptor.EncryptString(inputText, _keyValid);
            var outputText2 = encryptor.EncryptString(inputText, _keyValid);
            Assert.AreNotEqual(outputText1, outputText2);
        }
        
        
        [Test]
        public void DecryptString_Given_EmptyTextString_ShouldThrowNullArgumentException()
        {
            var encryptor = new Encryptor();
            Assert.Throws<ArgumentNullException>(() => encryptor.DecryptString(string.Empty, _keyInvalid));
        }
        
        [Test]
        public void DecryptString_Given_AndShortKey__KeyString_String_ShouldThrow_FormatException()
        {
            var encryptor = new Encryptor();
            Assert.Throws<FormatException>(() => encryptor.DecryptString("foo", _keyLongValid));
        }
        
        [Test]
        public void DecryptString_Given_Cypher__KeyString_String_Should_DecyriptStringBack()
        {
            var encryptor = new Encryptor();
            var inputText = "@Nathan001";
            var outputText = encryptor.EncryptString(inputText, _keyValid);
            var orginalText = encryptor.DecryptString(outputText, _keyValid);
            Assert.AreEqual(inputText, orginalText);
        }
    }
}