using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
 

namespace GDMS_DAL.ComonManager
{
    /// <summary>
    /// Provides methods for encryption and decryption operations.
    /// </summary>
    public class EncryptionManager
    {
        /// <summary>
        /// Decrypts a byte array cipher text using the provided key and initialization vector (IV).
        /// </summary>
        /// <param name="cipherText">The cipher text to decrypt.</param>
        /// <param name="key">The key used for decryption.</param>
        /// <param name="iv">The initialization vector (IV) used for decryption.</param>
        /// <returns>The decrypted plaintext string.</returns>
        public static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            string plaintext = null;
            using (var rijAlg = new System.Security.Cryptography.RijndaelManaged())
            {
                //Settings
                rijAlg.Mode = System.Security.Cryptography.CipherMode.CBC;
                rijAlg.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;
                rijAlg.Key = key;
                rijAlg.IV = iv;
                // Create a decrytor to perform the stream transform.
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
                try
                {
                    // Create the streams used for decryption.
                    using (var msDecrypt = new System.IO.MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new System.Security.Cryptography.CryptoStream(msDecrypt, decryptor, System.Security.Cryptography.CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new System.IO.StreamReader(csDecrypt))
                            {
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }
            return plaintext;
        }
        /// <summary>
        /// Decrypts a Base64 encoded cipher text string using a predefined key and initialization vector (IV).
        /// </summary>
        /// <param name="cipherText">The Base64 encoded cipher text to decrypt.</param>
        /// <returns>The decrypted plaintext string.</returns>
        public static string DecryptStringAES(string cipherText)
        {
            var keybytes = Encoding.UTF8.GetBytes("7061737323313233");
            var iv = Encoding.UTF8.GetBytes("7061737323313233");
            cipherText = cipherText.Replace(' ', '+');
            var encrypted = Convert.FromBase64String(cipherText);
            var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
            return string.Format(decriptedFromJavascript);
        }
        /// <summary>
        /// Computes the SHA1 hash of the input password and returns the Base64 encoded hash string.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>The Base64 encoded hash of the password.</returns>
        public static string EncreptPassword(string password)
        {
            using (var sha1 = new System.Security.Cryptography.SHA1Managed())
            {
                var hash = Encoding.UTF8.GetBytes(password);
                var generatedHash = sha1.ComputeHash(hash);
                var generatedHashString = Convert.ToBase64String(generatedHash);
                return generatedHashString;
            }
        }
        /// <summary>
        /// Encrypts a plaintext string using AES encryption.
        /// </summary>
        /// <param name="text">The plaintext string to encrypt.</param>
        /// <returns>The Base64 encoded encrypted text.</returns>
        public static string EncryptString(string text)
        {
            var key = Encoding.UTF8.GetBytes("7061737323313233");

            using (var aesAlg = Aes.Create())
            {
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
        /// <summary>
        /// Decrypts a Base64 encoded cipher text string encrypted using the EncryptString method.
        /// </summary>
        /// <param name="cipherText">The Base64 encoded cipher text to decrypt.</param>
        /// <returns>The decrypted plaintext string.</returns>
        public static string DecryptString(string cipherText)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            var cipher = new byte[fullCipher.Length - iv.Length];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, fullCipher.Length - iv.Length);

            var key = Encoding.UTF8.GetBytes("7061737323313233");

            using (var aesAlg = Aes.Create())
            {
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
