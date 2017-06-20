using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Newtonsoft.Json;

namespace Forwarder.Sources
{
    class Cryptography
    {
        private byte[] KEY;
        private byte[] IV;
        private RSAParameters privateKey;
        private RSAParameters publicKey;

        public String GenerateRSAKeys()
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            this.privateKey = RSA.ExportParameters(true);
            this.publicKey = RSA.ExportParameters(false);
            return JsonConvert.SerializeObject(publicKey);
        }

        public String GenerateAESKey(String publicKey)
        {
            this.publicKey = JsonConvert.DeserializeObject<RSAParameters>(publicKey);

            Aes aes = Aes.Create();
            this.KEY = aes.Key;
            this.IV = aes.IV;

            byte[] encKey = RSAEncrypt(KEY, this.publicKey, false);
            byte[] encIV = RSAEncrypt(IV, this.publicKey, false);
            return JsonConvert.SerializeObject(new byte[][] { encKey, encIV });
        }

        public void GetAESKey(string keys)
        {
            byte[][] byteKeys = JsonConvert.DeserializeObject<byte[][]>(keys);
            this.KEY = RSADecrypt(byteKeys[0], privateKey, false);
            this.IV = RSADecrypt(byteKeys[1], privateKey, false);
        }

        public byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            RSA.ImportParameters(RSAKeyInfo);
            return RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
        }

        public byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            RSA.ImportParameters(RSAKeyInfo);
            return RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
        }

        public byte[] EncryptAES(string plainText)
        {
            byte[] encrypted;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = this.KEY;
                aesAlg.IV = this.IV;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;
        }

        public String DecryptAES(byte[] cipherText)
        {
            String plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = this.KEY;
                aesAlg.IV = this.IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }
            return plaintext;
        }

        public String GetHash(String[] input)
        {
            SHA256 mySHA256 = SHA256Managed.Create();
            String json = JsonConvert.SerializeObject(input);
            byte[] hashValue = mySHA256.ComputeHash(Encoding.Unicode.GetBytes(json));
            return JsonConvert.SerializeObject(hashValue);
        }
    }
}
