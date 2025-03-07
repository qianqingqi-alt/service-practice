using Microsoft.Extensions.Options;
using NETCore.Encrypt;
using System.Security.Cryptography;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Infrastructure.Utils
{
    public static class EncryptUtil
    {
        /// <summary>
        /// AES对称加密，可解密，用于加密外部密码
        /// </summary>
        public static string AESEncrypt(string plainText, string key = "")
        {
            if (!string.IsNullOrEmpty(plainText))
            {
                var encryptKey = DIServiceUtil.ServiceProvider == null ? key : DIServiceUtil.GetService<IOptions<ApplicationConfig>>().Value.EncryptionKey;
                string encryptedText = EncryptProvider.AESEncrypt(plainText, encryptKey);
                return encryptedText;
            }
            else return "";
        }

        /// <summary>
        /// AES对称解密
        /// </summary>
        public static string AESDecrypt(string encryptedText, string key = "")
        {
            if (!string.IsNullOrEmpty(encryptedText))
            {
                var encryptKey = DIServiceUtil.ServiceProvider == null ? key : DIServiceUtil.GetService<IOptions<ApplicationConfig>>().Value.EncryptionKey;
                string plainText = EncryptProvider.AESDecrypt(encryptedText, encryptKey);
                return plainText;
            }
            else return "";
        }

        /// <summary>
        /// BCrypt哈希密码，不可解密，用于加密内部密码
        /// </summary>
        public static string HashPassword(string password)
        {
            string salt = BCryptNet.GenerateSalt();
            string hashedPassword = BCryptNet.HashPassword(password, salt);
            return hashedPassword;
        }

        /// <summary>
        /// BCrypt验证密码
        /// </summary>
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCryptNet.Verify(password, hashedPassword);
        }

        /// <summary>
        /// SHA256哈希，用于数据完整性校验、数字签名
        /// </summary>
        public static string HMACSHA256(string input)
        {
            var appConfig = DIServiceUtil.GetService<IOptions<ApplicationConfig>>();
            string hash = EncryptProvider.HMACSHA256(input, appConfig.Value.EncryptionKey);
            return hash;
        }

        public class RSAKeys
        {
            public string? PublicKey { get; set; }
            public string? PrivateKey { get; set; }
        }

        public static RSAKeys CreateRSAPemKeys(int keySize = 2048)
        {
            var (publicPem, privatePem) = EncryptProvider.RSAToPem(true, keySize); // pkcs8加密格式密钥
            RSAKeys keys = new() { PrivateKey = privatePem, PublicKey = publicPem };
            return keys;
        }

        /// <summary>
        /// RSA加密（需要加密的内容，公钥）
        /// </summary>
        public static string RSAEncrypt(string srcString, string publicKey)
        {
            //publicKey = GetHandledPemKey(publicKey, true);
            var plainText = EncryptProvider.RSAEncrypt(publicKey, srcString, RSAEncryptionPadding.Pkcs1, true);
            return plainText;
        }

        /// <summary>
        /// RSA解密（需要解密的内容，私钥）
        /// </summary>
        public static string RSADecrypt(string srcString, string privateKey)
        {
            //privateKey = GetHandledPemKey(privateKey, false);
            string plainText = EncryptProvider.RSADecrypt(privateKey, srcString, RSAEncryptionPadding.Pkcs1, true);
            return plainText;
        }

        /// <summary>
        /// 对pem密钥进行处理（密钥文本，是否是公钥）
        /// </summary>
        public static string GetHandledPemKey(string pemKey, bool isPublicKey)
        {
            string currentPemKey = pemKey;
            string pemMarker = "PUBLIC KEY";  // pem密钥标记
            if (!isPublicKey)
            {
                pemMarker = "PRIVATE KEY";
            }
            string beginMarker = "-----BEGIN " + pemMarker + "-----\n";
            string endMarker = "\n-----END " + pemMarker + "-----";
            if (!pemKey.StartsWith(beginMarker) && !pemKey.EndsWith(endMarker))
            {
                currentPemKey = beginMarker + pemKey + endMarker;
            }
            return currentPemKey;
        }
    }
}
