using System;
using System.Security.Cryptography;
using System.Text;

namespace Shop2City.Core.Security
{
    public static class PasswordHelper
    {
        public static string EncodePasswordMd5(string pass)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            var originalBytes = Encoding.Default.GetBytes(pass);
            var encodedBytes = md5.ComputeHash(originalBytes); 
            return BitConverter.ToString(encodedBytes);
        }


    }
}
