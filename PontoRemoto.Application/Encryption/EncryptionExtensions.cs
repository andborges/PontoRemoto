using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PontoRemoto.Application.Encryption
{
    public static class EncryptionExtensions
    {
        public static string AesEncrypt(this string plainText, ApplicationAesEncryptionInfo aesEncryptionInfo)
        {
            return plainText.AesEncrypt(aesEncryptionInfo.Password,
                                        aesEncryptionInfo.Salt,
                                        aesEncryptionInfo.PasswordIterations,
                                        aesEncryptionInfo.InitialVector,
                                        aesEncryptionInfo.KeySize);
        }

        public static string AesEncrypt(this string plainText, string password, string salt, int passwordIterations, string initialVector, int keySize)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                return "";
            }

            var initialVectorBytes = Encoding.ASCII.GetBytes(initialVector);
            var saltValueBytes = Encoding.ASCII.GetBytes(salt);
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            var derivedPassword = new Rfc2898DeriveBytes(password, saltValueBytes, passwordIterations);

            var keyBytes = derivedPassword.GetBytes(keySize / 8);
            var symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC };

            byte[] cipherTextBytes;

            using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, initialVectorBytes))
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                        cryptoStream.FlushFinalBlock();

                        cipherTextBytes = memoryStream.ToArray();

                        memoryStream.Close();
                        cryptoStream.Close();
                    }
                }
            }

            symmetricKey.Clear();

            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string AesDecrypt(this string cipherText, ApplicationAesEncryptionInfo aesEncryptionInfo)
        {
            return cipherText.AesDecrypt(aesEncryptionInfo.Password,
                                         aesEncryptionInfo.Salt,
                                         aesEncryptionInfo.PasswordIterations,
                                         aesEncryptionInfo.InitialVector,
                                         aesEncryptionInfo.KeySize);
        }

        public static string AesDecrypt(this string cipherText, string password, string salt, int passwordIterations, string initialVector, int keySize)
        {
            if (string.IsNullOrEmpty(cipherText))
            {
                return "";
            }

            var initialVectorBytes = Encoding.ASCII.GetBytes(initialVector);
            var saltValueBytes = Encoding.ASCII.GetBytes(salt);
            var cipherTextBytes = Convert.FromBase64String(cipherText);
            var derivedPassword = new Rfc2898DeriveBytes(password, saltValueBytes, passwordIterations);

            var keyBytes = derivedPassword.GetBytes(keySize / 8);
            var symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC };

            var plainTextBytes = new byte[cipherTextBytes.Length];
            int byteCount;

            using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, initialVectorBytes))
            {
                using (var memoryStream = new MemoryStream(cipherTextBytes))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        byteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                        memoryStream.Close();
                        cryptoStream.Close();
                    }
                }
            }

            symmetricKey.Clear();

            return Encoding.UTF8.GetString(plainTextBytes, 0, byteCount);
        }

        public static string Md5Hash(this string plainText)
        {
            var md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(Encoding.ASCII.GetBytes(plainText));

            //get hash result after compute it
            var result = md5.Hash;

            var strBuilder = new StringBuilder();

            foreach (var t in result)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(t.ToString("x2"));
            }

            return strBuilder.ToString();
        }
    }
}