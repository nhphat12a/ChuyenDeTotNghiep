using System.Security.Cryptography;

namespace Aristino.Helper
{
    public static class GenerateComfirmCode
    {
        public static int Create()
        {
            int minimum = 100000;
            int maximum = 999999;
            var randomNumber=RandomNumberGenerator.GetInt32(minimum, maximum);
            return randomNumber;
        }
    }
}
