using System.Security.Cryptography;
using System.Text;

namespace Aristino.Helper
{
    public static class SHA256Encryptor
    {
        public  static string Encrypt(string text)
        {
            var sha256=SHA256.Create();
            byte[] bytes=sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
            var hashString = new StringBuilder();
            for(int i = 0; i < bytes.Length; i++)
            {
                hashString.Append(bytes[i].ToString("x2"));
            }
            return hashString.ToString();
        }
    }
}
